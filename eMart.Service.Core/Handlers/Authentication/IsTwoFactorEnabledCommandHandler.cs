using eMart.Service.Core.Commands.Authentication;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Services;
using MediatR;

namespace eMart.Service.Core.Handlers.Authentication
{
    public class IsTwoFactorEnabledCommandHandler : IRequestHandler<IsTwoFactorEnabledCommand, CommonResponse<bool>>
    {
        private readonly ITwoFactorAuthService _twoFactorAuthService;

        public IsTwoFactorEnabledCommandHandler(ITwoFactorAuthService twoFactorAuthService)
        {
            _twoFactorAuthService = twoFactorAuthService;
        }

        public async Task<CommonResponse<bool>> Handle(IsTwoFactorEnabledCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _twoFactorAuthService.IsTwoFactorEnabledAsync(request.UserId);

                return new CommonResponse<bool>
                {
                    Code = CommonStatusCode.Success,
                    Data = result,
                    Message = "Two-factor authentication status retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<bool>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while checking two-factor authentication status",
                    Data = false
                };
            }
        }
    }
}
