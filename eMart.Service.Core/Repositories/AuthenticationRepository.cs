using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.User;
using eMart.Service.Core.Interfaces;
using eMart.Service.DataModels;
using eMart.Service.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace eMart.Service.Core.Repositories
{
    public class AuthenticationRepository : RepositoryBase<AuthenticationRepository>, IAuthenticationRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticationRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, eMartDbContext dbContext) : base(dbContext)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public UserAuthResponseDto Login(LoginDto loginDto)
        {
            var existingUser = dbContext.Users.FirstOrDefault(x => (x.Email == loginDto.Email || x.Name == loginDto.Email.ToLower()) && (x.IsDeleted == null || x.IsDeleted == false));
            if (existingUser == null)
            {
                return new UserAuthResponseDto
                {
                    Error = "Not Found",
                };
            }

            if (!VerifyPasswordHash(loginDto.Password, existingUser.PasswordHash, existingUser.PasswordSalt))
            {
                return new UserAuthResponseDto
                {
                    Error = "PasswordInvalid",
                };
            }

            // Generate new Access Token
            var userCommonResponseDto = new UserCommonResponseDto
            {
                Id = existingUser.Id,
                Email = existingUser.Email,
                PasswordHash = existingUser.PasswordHash,
                Name = existingUser.Name,
                Role = existingUser.Role,
                ProfilePicture = existingUser.ProfilePicture,
                DarkMode = existingUser.DarkMode,
                CreatedAt = existingUser.CreatedAt,
                UpdatedAt = existingUser.UpdatedAt
            };
            var accessToken = GenerateJwtToken(userCommonResponseDto);

            // Generate new Refresh Token (raw)
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpriry = DateTime.UtcNow.AddDays(7);

            // Set cookie with raw refresh token
            SetRefreshToken(refreshToken.RefreshToken, refreshTokenExpriry);

            // Hash the refresh token before storing in DB
            var refreshTokenHash = ComputeSha256Hash(refreshToken.RefreshToken);

            // Save Refresh Token in UserTokens table (store hashed token)
            var newUserToken = new UserToken
            {
                UserId = existingUser.Id,
                AccessToken = accessToken,
                RefreshToken = refreshTokenHash,
                RefreshTokenExpiry = refreshTokenExpriry,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsRevoked = false
            };

            dbContext.UserTokens.Add(newUserToken);
            dbContext.SaveChanges();

            var loginResponse = new UserAuthResponseDto
            {
                UserId = existingUser.Id,
                Email = existingUser?.Email,
                Name = existingUser?.Name,
                Role = existingUser?.Role,
                AccessToken = accessToken,
                RefreshToken = refreshToken.RefreshToken,
                ExpiresIn = refreshTokenExpriry
            };
            return loginResponse;
        }

        public UserAuthResponseDto Logout(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return null;
            }

            // Always hash the incoming refresh token before searching
            var refreshTokenHash = ComputeSha256Hash(refreshToken);
            var tokenRecord = dbContext.UserTokens.FirstOrDefault(ut => ut.RefreshToken == refreshTokenHash);
            if (tokenRecord == null)
            {
                return null;
            }

            var userId = tokenRecord.UserId;

            // Find all refresh tokens for the user and revoke them
            var userTokens = dbContext.UserTokens.Where(x => x.UserId == userId).ToList();
            if (!userTokens.Any())
            {
                return null;
            }

            foreach (var t in userTokens)
            {
                t.IsRevoked = true;
                t.UpdatedAt = DateTime.UtcNow;
            }

            dbContext.UserTokens.UpdateRange(userTokens);
            dbContext.SaveChanges();

            var logoutResponse = new UserAuthResponseDto
            {
                UserId = userId,
                Email = null,
                Name = null,
                Role = null,
                AccessToken = null,
                RefreshToken = null
            };

            return logoutResponse;
        }

        public TokenDto RefreshToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var tokenHash = ComputeSha256Hash(token);
            var userToken = dbContext.UserTokens.Include(ut => ut.User).FirstOrDefault(ut => ut.RefreshToken == tokenHash);

            if (userToken == null || userToken.IsRevoked || userToken.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return null;
            }

            // Generate new Access Token
            var userCommonResponseDto = new UserCommonResponseDto
            {
                Id = userToken.User.Id,
                Email = userToken.User.Email,
                PasswordHash = userToken.User.PasswordHash,
                Name = userToken.User.Name,
                Role = userToken.User.Role,
                ProfilePicture = userToken.User.ProfilePicture,
                DarkMode = userToken.User.DarkMode,
                CreatedAt = userToken.User.CreatedAt,
                UpdatedAt = userToken.User.UpdatedAt
            };
            var newAccessToken = GenerateJwtToken(userCommonResponseDto);

            // Generate new Refresh Token (raw)
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpriry = DateTime.UtcNow.AddDays(7);

            // Revoke the old refresh token
            userToken.IsRevoked = true;
            userToken.UpdatedAt = DateTime.UtcNow;
            dbContext.UserTokens.Update(userToken);

            // Hash the new refresh token before storing
            var newRefreshTokenHash = ComputeSha256Hash(refreshToken.RefreshToken);

            // Save new Refresh Token in UserTokens table (not revoked)
            var newUserToken = new UserToken
            {
                UserId = userToken.User.Id,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshTokenHash,
                RefreshTokenExpiry = refreshTokenExpriry,
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            dbContext.UserTokens.Add(newUserToken);
            dbContext.SaveChanges();

            // Update cookie with new raw refresh token
            SetRefreshToken(refreshToken.RefreshToken, refreshTokenExpriry);

            var newToken = new TokenDto
            {
                AccessToken = newAccessToken,
                RefreshToken = refreshToken.RefreshToken
            };

            return newToken;
        }

        public string GenerateJwtToken(UserCommonResponseDto user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var keyValue = jwtSettings["Key"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var duration = jwtSettings["DurationInMinutes"];

            if (string.IsNullOrEmpty(keyValue) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(duration))
            {
                throw new InvalidOperationException("JWT configuration is missing or incomplete. Please check JwtSettings in configuration.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.Name ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(duration)),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private TokenDto GenerateRefreshToken()
        {
            var refreshToken = new TokenDto
            {
                RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
            };

            return refreshToken;
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private void SetRefreshToken(string rawRefreshToken, DateTime expires)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Append("refresh_token", rawRefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = expires
                });
            }
            else
            {
                throw new InvalidOperationException("HttpContext is not available.");
            }
        }

    }
}
