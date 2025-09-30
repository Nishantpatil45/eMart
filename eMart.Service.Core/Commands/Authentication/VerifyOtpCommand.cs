using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;
using MediatR;

namespace eMart.Service.Core.Commands.Authentication
{
    public class VerifyOtpCommand : IRequest<CommonResponse<bool>>
    {
        public VerifyOtpDto VerifyOtpRequest { get; set; } = null!;
    }
}
