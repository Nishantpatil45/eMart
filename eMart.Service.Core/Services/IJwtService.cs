using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.User;
using System.Security.Claims;

namespace eMart.Service.Core.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(UserDto user, bool twoFactorVerified = false);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        bool ValidateToken(string token);
        TokenDto GenerateTokenPair(UserDto user, bool twoFactorVerified = false);
        TokenDto RefreshTokenPair(string accessToken, string refreshToken);
    }
}
