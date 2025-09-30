using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.User;
using eMart.Service.Core.Interfaces;
using eMart.Service.Core.Queries.User;
using MediatR;

namespace eMart.Service.Core.Handlers.User
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, CommonResponse<UserCommonResponseDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByEmailQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<CommonResponse<UserCommonResponseDto>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserDetails(request.Email);
                if (user == null)
                {
                    return new CommonResponse<UserCommonResponseDto>
                    {
                        Code = CommonStatusCode.NotFound,
                        Message = CommonMessages.UserNotFound,
                        Data = null
                    };
                }

                // Convert UserDto to UserCommonResponseDto
                var userResponse = new UserCommonResponseDto
                {
                    Id = user.Id ?? string.Empty,
                    Name = user.Name ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Role = user.Role ?? string.Empty,
                    ProfilePicture = null, // UserDto doesn't have ProfilePicture
                    DarkMode = user.DarkMode ?? false,
                    CreatedAt = DateTime.UtcNow, // UserDto doesn't have CreatedAt
                    UpdatedAt = DateTime.UtcNow  // UserDto doesn't have UpdatedAt
                };

                return new CommonResponse<UserCommonResponseDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = userResponse,
                    Message = CommonMessages.UserFound
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
