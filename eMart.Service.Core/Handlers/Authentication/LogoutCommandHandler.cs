using eMart.Service.Core.Commands.Authentication;
using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Interfaces;
using MediatR;

namespace eMart.Service.Core.Handlers.Authentication
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, CommonResponse<bool>>
    {
        private readonly IAuthenticationRepository _authenticationRepository;

        public LogoutCommandHandler(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }

        public async Task<CommonResponse<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = _authenticationRepository.Logout(request.Token);
                
                return new CommonResponse<bool>
                {
                    Code = CommonStatusCode.Success,
                    Data = result != null,
                    Message = result != null ? "Logout successful" : "Logout failed"
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<bool>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred during logout",
                    Data = false
                };
            }
        }
    }
}
