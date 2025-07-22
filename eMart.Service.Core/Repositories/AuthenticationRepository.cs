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
            var existingHashPassword = loginDto.Password;
            if (!VerifyPasswordHash(loginDto.Password, existingUser.PasswordHash, existingUser.PasswordSolt))
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

            // Generate new Refresh Token
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpriry = DateTime.UtcNow.AddDays(7);

            SetRefreshToken(refreshToken);

            // Save Refresh Token in UserTokens table
            var newUserToken = new UserToken
            {
                UserId = existingUser.Id,
                AccessToken = accessToken,
                RefreshToken = refreshToken.RefreshToken,
                RefreshTokenExpiry = refreshTokenExpriry,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsRevoked= false
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
            // Validate and decode the access token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(refreshToken);
            if (jwtToken == null)
            {
                return null;
            }

            // Extract the UserId from the claims
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            // Find all refresh tokens for the user
            var userTokens = dbContext.UserTokens.Where(x => x.UserId == userId).ToList();
            if (!userTokens.Any())
            {
                return null;
            }

            // Remove all associated tokens from the database
            dbContext.UserTokens.RemoveRange(userTokens);
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
            var userToken = dbContext.UserTokens.Include(ut => ut.User).FirstOrDefault(ut => ut.RefreshToken == token);

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

            // Generate new Refresh Token
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpriry = DateTime.UtcNow.AddDays(7);

            // Revoke the old refresh token
            userToken.IsRevoked = true;

            // Save Refresh Token in UserTokens table
            var newUserToken = new UserToken
            {
                UserId = userToken.User.Id,
                AccessToken = newAccessToken,
                RefreshToken = refreshToken.RefreshToken,
                RefreshTokenExpiry = refreshTokenExpriry,
                IsRevoked = userToken.IsRevoked,
                CreatedAt = userToken.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            };

            dbContext.UserTokens.Add(newUserToken);
            dbContext.SaveChanges();

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
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["DurationInMinutes"])),
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

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSolt)
        {
            using (var hmac = new HMACSHA512(passwordSolt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private void SetRefreshToken(TokenDto newRefreshToken)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Append("access_token", newRefreshToken.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7) // Set cookie expiration
                });
            }
            else
            {
                throw new InvalidOperationException("HttpContext is not available.");
            }
        }

    }
}
