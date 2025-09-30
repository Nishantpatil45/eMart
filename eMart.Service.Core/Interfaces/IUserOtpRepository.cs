using System.Threading.Tasks;
using eMart.Service.DataModels;

namespace eMart.Service.Core.Interfaces
{
    public interface IUserOtpRepository
    {
        Task<UserOtp> GenerateOtpAsync(string userId);
        Task<bool> VerifyOtpAsync(string userId, string otpCode);
        
        // Two-Factor Authentication methods
        Task<UserOtp?> GetUserOtpByUserIdAsync(string userId);
        Task<UserOtp> CreateUserOtpAsync(UserOtp userOtp);
        Task<UserOtp> UpdateUserOtpAsync(UserOtp userOtp);
    }
}
