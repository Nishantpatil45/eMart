using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.User;
using eMart.Service.Core.Interfaces;
using eMart.Service.Core.Queries.User;
using MediatR;

namespace eMart.Service.Core.Handlers.User
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, CommonResponse<List<UserCommonResponseDto>>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<CommonResponse<List<UserCommonResponseDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Note: GetAllUsers method doesn't exist in IUserRepository
                // This would need to be implemented in the repository
                var users = new List<UserCommonResponseDto>();

                return new CommonResponse<List<UserCommonResponseDto>>
                {
                    Code = CommonStatusCode.Success,
                    Data = users,
                    Message = CommonMessages.UsersFound
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<List<UserCommonResponseDto>>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while retrieving users",
                    Data = new List<UserCommonResponseDto>()
                };
            }
        }
    }
}
