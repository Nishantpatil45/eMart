using System.ComponentModel.DataAnnotations;

namespace eMart.Service.Core.Dtos.Authentication
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
        
        [Required]
        public string AccessToken { get; set; } = string.Empty;
    }
}
