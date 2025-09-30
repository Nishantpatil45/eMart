using eMart.Service.Core.Dtos.Common;
using MediatR;

namespace eMart.Service.Core.Commands.Authentication
{
    public class IsTwoFactorEnabledCommand : IRequest<CommonResponse<bool>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
