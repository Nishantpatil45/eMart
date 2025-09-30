using System;
using System.Threading.Tasks;
using eMart.Service.Core.Interfaces;
using eMart.Service.DataModels;
using eMart.Service.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eMart.Service.Core.Repositories
{
    public class UserOtpRepository : IUserOtpRepository
    {
        private readonly eMartDbContext dbContext;
        public UserOtpRepository(eMartDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<UserOtp> GenerateOtpAsync(string userId)
        {
            var otpCode = new Random().Next(100000, 999999).ToString();
            var expiry = DateTime.UtcNow.AddMinutes(5);
            var userOtp = new UserOtp
            {
                UserId = userId,
                OtpCode = otpCode,
                Expiry = expiry,
                IsUsed = false
            };
            await dbContext.UserOtps.AddAsync(userOtp);
            await dbContext.SaveChangesAsync();
            // For demo: OTP would be sent via email/SMS in production
            // Remove Console.WriteLine
            return userOtp;
        }

        public async Task<bool> VerifyOtpAsync(string userId, string otpCode)
        {
            var otp = await dbContext.UserOtps.FirstOrDefaultAsync(o => o.UserId == userId && o.OtpCode == otpCode && !o.IsUsed && o.Expiry > DateTime.UtcNow);
            if (otp == null)
                return false;
            otp.IsUsed = true;
            await dbContext.SaveChangesAsync();
            return true;
        }

        // Two-Factor Authentication methods
        public async Task<UserOtp?> GetUserOtpByUserIdAsync(string userId)
        {
            return await dbContext.UserOtps.FirstOrDefaultAsync(o => o.UserId == userId);
        }

        public async Task<UserOtp> CreateUserOtpAsync(UserOtp userOtp)
        {
            await dbContext.UserOtps.AddAsync(userOtp);
            await dbContext.SaveChangesAsync();
            return userOtp;
        }

        public async Task<UserOtp> UpdateUserOtpAsync(UserOtp userOtp)
        {
            userOtp.UpdatedAt = DateTime.UtcNow;
            dbContext.UserOtps.Update(userOtp);
            await dbContext.SaveChangesAsync();
            return userOtp;
        }
    }
}
