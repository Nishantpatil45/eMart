using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.DataModels;
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
        UserAuthResponseDto VerifyOtp(string userId, string otpCode);
        
        // Methods for Enhanced Authentication compatibility
        List<UserToken> GetUserTokens(string userId);
        void AddUserToken(UserToken userToken);
    }
}
