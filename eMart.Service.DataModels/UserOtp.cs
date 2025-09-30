using System;

namespace eMart.Service.DataModels
{
    public class UserOtp
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public string OtpCode { get; set; }
        public DateTime Expiry { get; set; }
        public bool IsUsed { get; set; } = false;
        
        // Two-Factor Authentication properties
        public string? SecretKey { get; set; }
        public bool IsVerified { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public User User { get; set; }
    }
}
