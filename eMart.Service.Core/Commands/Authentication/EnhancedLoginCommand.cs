using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;
using MediatR;

namespace eMart.Service.Core.Commands.Authentication
{
    public class EnhancedLoginCommand : IRequest<CommonResponse<EnhancedAuthResponseDto>>
    {
        public LoginDto LoginRequest { get; set; } = null!;
        public string? TwoFactorCode { get; set; }
    }
}
