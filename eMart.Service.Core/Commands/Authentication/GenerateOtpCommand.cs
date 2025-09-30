using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;
using MediatR;

namespace eMart.Service.Core.Commands.Authentication
{
    public class GenerateOtpCommand : IRequest<CommonResponse<bool>>
    {
        public string Email { get; set; } = string.Empty;
    }
}
