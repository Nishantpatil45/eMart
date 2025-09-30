using eMart.Service.Core.Dtos.Common;
using MediatR;

namespace eMart.Service.Core.Commands.Authentication
{
    public class DisableTwoFactorCommand : IRequest<CommonResponse<bool>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
