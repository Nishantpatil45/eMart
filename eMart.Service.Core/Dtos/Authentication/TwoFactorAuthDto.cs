using System.ComponentModel.DataAnnotations;

namespace eMart.Service.Core.Dtos.Authentication
{
    public class TwoFactorAuthDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [Required]
        public string Code { get; set; } = string.Empty;
    }
    
    public class TwoFactorSetupDto
    {
        public string UserId { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string QrCodeUrl { get; set; } = string.Empty;
        public string ManualEntryKey { get; set; } = string.Empty;
    }
    
    public class TwoFactorVerificationDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [Required]
        public string Code { get; set; } = string.Empty;
        
        public bool IsSetup { get; set; } = false;
    }
}
