using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;
using MediatR;

namespace eMart.Service.Core.Commands.Authentication
{
    public class LogoutCommand : IRequest<CommonResponse<bool>>
    {
        public string UserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
