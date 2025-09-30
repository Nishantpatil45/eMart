using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;
using MediatR;

namespace eMart.Service.Core.Commands.Authentication
{
    public class LoginCommand : IRequest<CommonResponse<UserAuthResponseDto>>
    {
        public LoginDto LoginRequest { get; set; } = null!;
    }
}
