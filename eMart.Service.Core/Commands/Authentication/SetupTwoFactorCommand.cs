using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;
using MediatR;

namespace eMart.Service.Core.Commands.Authentication
{
    public class SetupTwoFactorCommand : IRequest<CommonResponse<TwoFactorSetupDto>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
