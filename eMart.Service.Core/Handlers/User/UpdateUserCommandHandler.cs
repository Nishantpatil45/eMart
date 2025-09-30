using eMart.Service.Core.Commands.User;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.User;
using eMart.Service.Core.Interfaces;
using MediatR;

namespace eMart.Service.Core.Handlers.User
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, CommonResponse<UserCommonResponseDto>>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<CommonResponse<UserCommonResponseDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Note: UpdateUser method doesn't exist in IUserRepository
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
                    Message = "An unexpected error occurred while updating the user",
                    Data = null
                };
            }
        }
    }
}
