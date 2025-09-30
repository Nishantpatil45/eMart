using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace eMart.Service.Core.Services
{
    public class TwoFactorAuthService : ITwoFactorAuthService
    {
        private readonly IUserOtpRepository _userOtpRepository;
        private readonly IUserRepository _userRepository;
        private const int SecretKeyLength = 32;

        public TwoFactorAuthService(IUserOtpRepository userOtpRepository, IUserRepository userRepository)
        {
            _userOtpRepository = userOtpRepository;
            _userRepository = userRepository;
        }

        public async Task<TwoFactorSetupDto> GenerateTwoFactorSetupAsync(string userId)
        {
            var secretKey = GenerateSecretKey();
            var user = await _userRepository.GetUserDetailsById(userId);
            
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var qrCodeUrl = GenerateQrCodeUrl(secretKey, user.Email ?? string.Empty);
            var manualEntryKey = secretKey;

            return new TwoFactorSetupDto
            {
                UserId = userId,
                SecretKey = secretKey,
                QrCodeUrl = qrCodeUrl,
                ManualEntryKey = manualEntryKey
            };
        }

        public async Task<bool> VerifyTwoFactorCodeAsync(string userId, string code)
        {
            var userOtp = await _userOtpRepository.GetUserOtpByUserIdAsync(userId);
            
            if (userOtp == null || string.IsNullOrEmpty(userOtp.SecretKey))
            {
                return false;
            }

            return VerifyTotpCode(userOtp.SecretKey, code);
        }

        public async Task<bool> EnableTwoFactorAsync(string userId, string code)
        {
            var userOtp = await _userOtpRepository.GetUserOtpByUserIdAsync(userId);
            
            if (userOtp == null)
            {
                return false;
            }

            if (VerifyTotpCode(userOtp.SecretKey, code))
            {
                userOtp.IsVerified = true;
                await _userOtpRepository.UpdateUserOtpAsync(userOtp);
                return true;
            }

            return false;
        }

        public async Task<bool> DisableTwoFactorAsync(string userId)
        {
            var userOtp = await _userOtpRepository.GetUserOtpByUserIdAsync(userId);
            
            if (userOtp == null)
            {
                return false;
            }

            userOtp.IsVerified = false;
            userOtp.SecretKey = null;
            await _userOtpRepository.UpdateUserOtpAsync(userOtp);
            return true;
        }

        public async Task<bool> IsTwoFactorEnabledAsync(string userId)
        {
            var userOtp = await _userOtpRepository.GetUserOtpByUserIdAsync(userId);
            return userOtp?.IsVerified == true && !string.IsNullOrEmpty(userOtp.SecretKey);
        }

        public string GenerateQrCodeUrl(string secretKey, string userEmail)
        {
            var issuer = "eMart";
            var encodedSecretKey = System.Web.HttpUtility.UrlEncode(secretKey);
            var encodedIssuer = System.Web.HttpUtility.UrlEncode(issuer);
            var encodedUserEmail = System.Web.HttpUtility.UrlEncode(userEmail);
            
            return $"otpauth://totp/{encodedIssuer}:{encodedUserEmail}?secret={encodedSecretKey}&issuer={encodedIssuer}";
        }

        private string GenerateSecretKey()
        {
            var bytes = new byte[SecretKeyLength];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return Base32Encode(bytes);
        }

        private bool VerifyTotpCode(string secretKey, string code)
        {
            var key = Base32Decode(secretKey);
            var timeStep = 30; // 30 seconds
            var window = 1; // Allow 1 time step before and after

            var currentTimeStep = GetCurrentTimeStep(timeStep);
            
            for (int i = -window; i <= window; i++)
            {
                var timeStepToTest = currentTimeStep + i;
                var expectedCode = GenerateTotpCode(key, timeStepToTest, 6);
                
                if (expectedCode == code)
                {
                    return true;
                }
            }

            return false;
        }

        private long GetCurrentTimeStep(int timeStep)
        {
            var unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return unixTimestamp / timeStep;
        }

        private string GenerateTotpCode(byte[] key, long timeStep, int digits)
        {
            var timeStepBytes = BitConverter.GetBytes(timeStep);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timeStepBytes);
            }

            using var hmac = new HMACSHA1(key);
            var hash = hmac.ComputeHash(timeStepBytes);
            
            var offset = hash[hash.Length - 1] & 0x0F;
            var code = ((hash[offset] & 0x7F) << 24) |
                      ((hash[offset + 1] & 0xFF) << 16) |
                      ((hash[offset + 2] & 0xFF) << 8) |
                      (hash[offset + 3] & 0xFF);

            code = code % (int)Math.Pow(10, digits);
            return code.ToString().PadLeft(digits, '0');
        }

        private string Base32Encode(byte[] data)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            var output = new StringBuilder();
            var bits = 0;
            var value = 0;

            foreach (var b in data)
            {
                value = (value << 8) | b;
                bits += 8;

                while (bits >= 5)
                {
                    output.Append(alphabet[(value >> (bits - 5)) & 31]);
                    bits -= 5;
                }
            }

            if (bits > 0)
            {
                output.Append(alphabet[(value << (5 - bits)) & 31]);
            }

            return output.ToString();
        }

        private byte[] Base32Decode(string input)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            input = input.ToUpperInvariant();
            var output = new List<byte>();
            var bits = 0;
            var value = 0;

            foreach (var c in input)
            {
                var index = alphabet.IndexOf(c);
                if (index == -1) continue;

                value = (value << 5) | index;
                bits += 5;

                if (bits >= 8)
                {
                    output.Add((byte)((value >> (bits - 8)) & 255));
                    bits -= 8;
                }
            }

            return output.ToArray();
        }
    }
}
