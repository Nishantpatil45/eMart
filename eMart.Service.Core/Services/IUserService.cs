using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.User;

namespace eMart.Service.Core.Services
{
    public interface IUserService
    {
        Task<CommonResponse<UserCommonResponseDto>> CreateUserAsync(UserCreateRequestDto userCreateRequest, string? createdBy = null);
        Task<CommonResponse<UserCommonResponseDto>> UpdateUserAsync(string userId, UserCreateRequestDto userUpdateRequest, string updatedBy);
        Task<CommonResponse<UserCommonResponseDto>> DeleteUserAsync(string userId, string deletedBy);
        Task<CommonResponse<List<UserCommonResponseDto>>> GetAllUsersAsync(string requestedBy);
        Task<CommonResponse<UserCommonResponseDto>> GetUserByIdAsync(string userId, string requestedBy);
        Task<CommonResponse<UserCommonResponseDto>> GetUserByEmailAsync(string email, string requestedBy);
    }
}
