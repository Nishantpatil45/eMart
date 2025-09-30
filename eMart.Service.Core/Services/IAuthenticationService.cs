using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;

namespace eMart.Service.Core.Services
{
    public interface IAuthenticationService
    {
        Task<CommonResponse<UserAuthResponseDto>> LoginAsync(LoginDto loginRequest);
        Task<CommonResponse<TokenDto>> RefreshTokenAsync(string refreshToken);
        Task<CommonResponse<bool>> LogoutAsync(string userId, string token);
        Task<CommonResponse<bool>> GenerateOtpAsync(string email);
        Task<CommonResponse<bool>> VerifyOtpAsync(VerifyOtpDto verifyOtpRequest);
    }
}
