using eMart.Service.Core.Dtos.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMart.Service.Core.Interfaces
{
    public interface IAuthenticationRepository
    {
        public UserAuthResponseDto Login(LoginDto loginDto);
        public UserAuthResponseDto Logout(string refreshToken);
        public TokenDto RefreshToken(string token);
    }
}
