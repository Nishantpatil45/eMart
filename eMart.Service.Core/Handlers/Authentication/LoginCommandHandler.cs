using eMart.Service.Core.Commands.Authentication;
using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Interfaces;
using MediatR;

namespace eMart.Service.Core.Handlers.Authentication
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, CommonResponse<UserAuthResponseDto>>
    {
        private readonly IAuthenticationRepository _authenticationRepository;

        public LoginCommandHandler(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }

        public async Task<CommonResponse<UserAuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var authResponse = _authenticationRepository.Login(request.LoginRequest);
                if (authResponse == null)
                {
                    return new CommonResponse<UserAuthResponseDto>
                    {
                        Code = CommonStatusCode.Unauthorized,
                        Message = "Invalid email or password",
                        Data = null
                    };
                }

                return new CommonResponse<UserAuthResponseDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = authResponse,
                    Message = "Login successful"
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<UserAuthResponseDto>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred during login",
                    Data = null
                };
            }
        }
    }
}
