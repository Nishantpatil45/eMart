using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMart.Service.Core.Dtos.Authentication
{
    public class UserAuthResponseDto
    {
        public string UserId { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? AccessToken { get; set; }
        public string? Role { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiresIn { get; set; }
        public string Error { get; set; } = null;
    }
}
