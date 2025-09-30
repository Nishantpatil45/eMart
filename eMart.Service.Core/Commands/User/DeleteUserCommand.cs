using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.User;
using MediatR;

namespace eMart.Service.Core.Commands.User
{
    public class DeleteUserCommand : IRequest<CommonResponse<UserCommonResponseDto>>
    {
        public string UserId { get; set; } = string.Empty;
        public string DeletedBy { get; set; } = string.Empty;
    }
}
