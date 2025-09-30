using eMart.Service.Core.Dtos.Authentication;

namespace eMart.Service.Core.Services
{
    public interface ITwoFactorAuthService
    {
        Task<TwoFactorSetupDto> GenerateTwoFactorSetupAsync(string userId);
        Task<bool> VerifyTwoFactorCodeAsync(string userId, string code);
        Task<bool> EnableTwoFactorAsync(string userId, string code);
        Task<bool> DisableTwoFactorAsync(string userId);
        Task<bool> IsTwoFactorEnabledAsync(string userId);
        string GenerateQrCodeUrl(string secretKey, string userEmail);
    }
}
