using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMart.Service.Core.Dtos.User
{
    public class UserCommonResponseDto
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public byte[]? PasswordHash { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? ProfilePicture { get; set; }
        public bool DarkMode { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
