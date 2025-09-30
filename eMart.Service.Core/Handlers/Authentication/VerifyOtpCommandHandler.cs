using eMart.Service.Core.Commands.Authentication;
using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Interfaces;
using MediatR;

namespace eMart.Service.Core.Handlers.Authentication
{
    public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, CommonResponse<bool>>
    {
        private readonly IAuthenticationRepository _authenticationRepository;

        public VerifyOtpCommandHandler(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }

        public async Task<CommonResponse<bool>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = _authenticationRepository.VerifyOtp(request.VerifyOtpRequest.UserId, request.VerifyOtpRequest.OtpCode);
                
                return new CommonResponse<bool>
                {
                    Code = CommonStatusCode.Success,
                    Data = result != null,
                    Message = result != null ? "OTP verified successfully" : "Invalid or expired OTP"
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<bool>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while verifying OTP",
                    Data = false
                };
            }
        }
    }
}
