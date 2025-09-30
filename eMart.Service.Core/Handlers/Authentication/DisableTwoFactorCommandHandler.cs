using eMart.Service.Core.Commands.Authentication;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Services;
using MediatR;

namespace eMart.Service.Core.Handlers.Authentication
{
    public class DisableTwoFactorCommandHandler : IRequestHandler<DisableTwoFactorCommand, CommonResponse<bool>>
    {
        private readonly ITwoFactorAuthService _twoFactorAuthService;

        public DisableTwoFactorCommandHandler(ITwoFactorAuthService twoFactorAuthService)
        {
            _twoFactorAuthService = twoFactorAuthService;
        }

        public async Task<CommonResponse<bool>> Handle(DisableTwoFactorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _twoFactorAuthService.DisableTwoFactorAsync(request.UserId);

                return new CommonResponse<bool>
                {
                    Code = result ? CommonStatusCode.Success : CommonStatusCode.BadRequest,
                    Data = result,
                    Message = result ? "Two-factor authentication disabled successfully" : "Failed to disable two-factor authentication"
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<bool>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while disabling two-factor authentication",
                    Data = false
                };
            }
        }
    }
}
