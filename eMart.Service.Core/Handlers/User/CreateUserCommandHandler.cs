using eMart.Service.Core.Commands.User;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.User;
using eMart.Service.Core.Interfaces;
using MediatR;

namespace eMart.Service.Core.Handlers.User
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CommonResponse<UserCommonResponseDto>>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<CommonResponse<UserCommonResponseDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newUser = await _userRepository.CreateUser(request.UserCreateRequest, null);
                if (newUser == null)
                {
                    return new CommonResponse<UserCommonResponseDto>
                    {
                        Code = CommonStatusCode.Conflict,
                        Message = CommonMessages.UserIsAlredyExits,
                        Data = null
                    };
                }

                return new CommonResponse<UserCommonResponseDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = newUser,
                    Message = CommonMessages.UserAddedSuccessfully
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<UserCommonResponseDto>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while creating the user",
                    Data = null
                };
            }
        }
    }
}
