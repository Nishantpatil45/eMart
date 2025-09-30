using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.User;
using MediatR;

namespace eMart.Service.Core.Queries.User
{
    public class GetAllUsersQuery : IRequest<CommonResponse<List<UserCommonResponseDto>>>
    {
        public string RequestedBy { get; set; } = string.Empty;
    }
}
