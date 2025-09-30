using eMart.Service.Core.Commands.Authentication;
using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;
using MediatR;

namespace eMart.Service.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMediator _mediator;

        public AuthenticationService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CommonResponse<UserAuthResponseDto>> LoginAsync(LoginDto loginRequest)
        {
            var command = new LoginCommand
            {
                LoginRequest = loginRequest
            };

            return await _mediator.Send(command);
        }

        public async Task<CommonResponse<TokenDto>> RefreshTokenAsync(string refreshToken)
        {
            var command = new RefreshTokenCommand
            {
                RefreshTokenRequest = new RefreshTokenRequestDto
                {
                    RefreshToken = refreshToken,
                    AccessToken = string.Empty // This should be provided by the caller
                }
            };

            return await _mediator.Send(command);
        }

        public async Task<CommonResponse<bool>> LogoutAsync(string userId, string token)
        {
            var command = new LogoutCommand
            {
                UserId = userId,
                Token = token
            };

            return await _mediator.Send(command);
        }

        public async Task<CommonResponse<bool>> GenerateOtpAsync(string email)
        {
            var command = new GenerateOtpCommand
            {
                Email = email
            };

            return await _mediator.Send(command);
        }

        public async Task<CommonResponse<bool>> VerifyOtpAsync(VerifyOtpDto verifyOtpRequest)
        {
            var command = new VerifyOtpCommand
            {
                VerifyOtpRequest = verifyOtpRequest
            };

            return await _mediator.Send(command);
        }
    }
}
