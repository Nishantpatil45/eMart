using eMart.Service.Core.Commands.Authentication;
using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Services;
using MediatR;

namespace eMart.Service.Core.Handlers.Authentication
{
    public class VerifyTwoFactorCommandHandler : IRequestHandler<VerifyTwoFactorCommand, CommonResponse<bool>>
    {
        private readonly ITwoFactorAuthService _twoFactorAuthService;

        public VerifyTwoFactorCommandHandler(ITwoFactorAuthService twoFactorAuthService)
        {
            _twoFactorAuthService = twoFactorAuthService;
        }

        public async Task<CommonResponse<bool>> Handle(VerifyTwoFactorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                bool result;

                if (request.TwoFactorRequest.IsSetup)
                {
                    // This is for enabling 2FA after setup
                    result = await _twoFactorAuthService.EnableTwoFactorAsync(
                        request.TwoFactorRequest.UserId, 
                        request.TwoFactorRequest.Code);
                }
                else
                {
                    // This is for verifying 2FA during login
                    result = await _twoFactorAuthService.VerifyTwoFactorCodeAsync(
                        request.TwoFactorRequest.UserId, 
                        request.TwoFactorRequest.Code);
                }

                return new CommonResponse<bool>
                {
                    Code = CommonStatusCode.Success,
                    Data = result,
                    Message = result ? "Two-factor authentication verified successfully" : "Invalid two-factor authentication code"
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<bool>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while verifying two-factor authentication",
                    Data = false
                };
            }
        }
    }
}
