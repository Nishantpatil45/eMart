using eMart.Service.Core.Dtos.User;

namespace eMart.Service.Core.Interfaces
{
    public interface IUserRepository
    {
        //UserCommonResponseDto GetUserById(long id);

        Task<UserCommonResponseDto> CreateUser(UserCreateRequestDto userCreateRequestDto, UserDto userDto);

        Task<UserDto> GetUserDetails(string emailId);
    }
}
