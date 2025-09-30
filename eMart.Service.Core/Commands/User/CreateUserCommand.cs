using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.User;
using MediatR;

namespace eMart.Service.Core.Commands.User
{
    public class CreateUserCommand : IRequest<CommonResponse<UserCommonResponseDto>>
    {
        public UserCreateRequestDto UserCreateRequest { get; set; } = null!;
        public string? CreatedBy { get; set; }
    }
}
