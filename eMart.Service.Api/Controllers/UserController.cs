using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.User;
using eMart.Service.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eMart.Service.Api.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public UserController(IUserRepository userRepository, IConfiguration configuration) : base (userRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateRequestDto userCreateRequestDto)
        {
            var loggedInUser = await this.GetLoggedInUserAsync();
            if (loggedInUser == null)
            {
                return Unauthorized();
            }

            var createUser = await _userRepository.CreateUser(userCreateRequestDto, loggedInUser);

            if(createUser == null)
            {
                return NotFound(new CommonErrorResponse()
                {
                    Path = "/error",
                    Message = CommonMessages.UserIsAlredyExits,
                    Status = CommonStatusCode.NotFound
                });
            }

            return Ok(new CommonResponse<UserCommonResponseDto>()
            {
                Code = CommonStatusCode.Success,
                Data = createUser,
                Message = CommonMessages.UserIdGetSuccessfully
            });
        }
    }
}
