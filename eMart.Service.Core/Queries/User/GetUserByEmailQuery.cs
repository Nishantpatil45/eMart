using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.User;
using MediatR;

namespace eMart.Service.Core.Queries.User
{
    public class GetUserByEmailQuery : IRequest<CommonResponse<UserCommonResponseDto>>
    {
        public string Email { get; set; } = string.Empty;
        public string RequestedBy { get; set; } = string.Empty;
    }
}
