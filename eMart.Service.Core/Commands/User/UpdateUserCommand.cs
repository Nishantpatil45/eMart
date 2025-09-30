using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.User;
using MediatR;

namespace eMart.Service.Core.Commands.User
{
    public class UpdateUserCommand : IRequest<CommonResponse<UserCommonResponseDto>>
    {
        public string UserId { get; set; } = string.Empty;
        public UserCreateRequestDto UserUpdateRequest { get; set; } = null!;
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
