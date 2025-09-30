using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;
using MediatR;

namespace eMart.Service.Core.Commands.Authentication
{
    public class RefreshTokenCommand : IRequest<CommonResponse<TokenDto>>
    {
        public RefreshTokenRequestDto RefreshTokenRequest { get; set; } = null!;
    }
}