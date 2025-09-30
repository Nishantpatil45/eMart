using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.User;
using eMart.Service.Core.Interfaces;
using eMart.Service.Core.Queries.User;
using MediatR;

namespace eMart.Service.Core.Handlers.User
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, CommonResponse<UserCommonResponseDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<CommonResponse<UserCommonResponseDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Note: GetUserById method doesn't exist in IUserRepository
                // This would need to be implemented in the repository
                return new CommonResponse<UserCommonResponseDto>
                {
                    Code = CommonStatusCode.NotFound,
                    Message = CommonMessages.UserNotFound,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<UserCommonResponseDto>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while retrieving the user",
                    Data = null
                };
            }
        }
    }
}
