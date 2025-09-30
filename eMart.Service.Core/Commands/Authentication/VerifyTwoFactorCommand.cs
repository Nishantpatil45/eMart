using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;
using MediatR;

namespace eMart.Service.Core.Commands.Authentication
{
    public class VerifyTwoFactorCommand : IRequest<CommonResponse<bool>>
    {
        public TwoFactorVerificationDto TwoFactorRequest { get; set; } = null!;
    }
}
