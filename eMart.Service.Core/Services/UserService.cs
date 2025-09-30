using eMart.Service.Core.Commands.User;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.User;
using eMart.Service.Core.Queries.User;
using MediatR;

namespace eMart.Service.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IMediator _mediator;

        public UserService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CommonResponse<UserCommonResponseDto>> CreateUserAsync(UserCreateRequestDto userCreateRequest, string? createdBy = null)
        {
            var command = new CreateUserCommand
            {
                UserCreateRequest = userCreateRequest,
                CreatedBy = createdBy
            };

            return await _mediator.Send(command);
        }

        public async Task<CommonResponse<UserCommonResponseDto>> UpdateUserAsync(string userId, UserCreateRequestDto userUpdateRequest, string updatedBy)
        {
            var command = new UpdateUserCommand
            {
                UserId = userId,
                UserUpdateRequest = userUpdateRequest,
                UpdatedBy = updatedBy
            };

            return await _mediator.Send(command);
        }

        public async Task<CommonResponse<UserCommonResponseDto>> DeleteUserAsync(string userId, string deletedBy)
        {
            var command = new DeleteUserCommand
            {
                UserId = userId,
                DeletedBy = deletedBy
            };

            return await _mediator.Send(command);
        }

        public async Task<CommonResponse<List<UserCommonResponseDto>>> GetAllUsersAsync(string requestedBy)
        {
            var query = new GetAllUsersQuery
            {
                RequestedBy = requestedBy
            };

            return await _mediator.Send(query);
        }

        public async Task<CommonResponse<UserCommonResponseDto>> GetUserByIdAsync(string userId, string requestedBy)
        {
            var query = new GetUserByIdQuery
            {
                UserId = userId,
                RequestedBy = requestedBy
            };

            return await _mediator.Send(query);
        }

        public async Task<CommonResponse<UserCommonResponseDto>> GetUserByEmailAsync(string email, string requestedBy)
        {
            var query = new GetUserByEmailQuery
            {
                Email = email,
                RequestedBy = requestedBy
            };

            return await _mediator.Send(query);
        }
    }
}
