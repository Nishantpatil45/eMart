using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;

namespace eMart.Service.Core.Services
{
    public interface IEnhancedAuthenticationService
    {
        Task<CommonResponse<EnhancedAuthResponseDto>> LoginAsync(LoginDto loginRequest, string? twoFactorCode = null);
        Task<CommonResponse<TokenDto>> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequest);
        Task<CommonResponse<TwoFactorSetupDto>> SetupTwoFactorAsync(string userId);
        Task<CommonResponse<bool>> VerifyTwoFactorAsync(TwoFactorVerificationDto twoFactorRequest);
        Task<CommonResponse<bool>> DisableTwoFactorAsync(string userId);
        Task<CommonResponse<bool>> IsTwoFactorEnabledAsync(string userId);
    }
}
