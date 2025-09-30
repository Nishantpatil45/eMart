using eMart.Service.Core.Commands.Authentication;
using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Interfaces;
using MediatR;

namespace eMart.Service.Core.Handlers.Authentication
{
    public class GenerateOtpCommandHandler : IRequestHandler<GenerateOtpCommand, CommonResponse<bool>>
    {
        private readonly IAuthenticationRepository _authenticationRepository;

        public GenerateOtpCommandHandler(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }

        public async Task<CommonResponse<bool>> Handle(GenerateOtpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Note: GenerateOtp method doesn't exist in IAuthenticationRepository
                // This would need to be implemented in the repository
                return new CommonResponse<bool>
                {
                    Code = CommonStatusCode.Success,
                    Data = false,
                    Message = "Failed to generate OTP"
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<bool>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while generating OTP",
                    Data = false
                };
            }
        }
    }
}
