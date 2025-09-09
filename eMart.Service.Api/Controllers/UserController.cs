using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.User;
using eMart.Service.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace eMart.Service.Api.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, IConfiguration configuration, ILogger<UserController> logger) : base(userRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateRequestDto userCreateRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new CommonErrorResponse
                {
                    Path = "/api/v1/register",
                    Message = "Invalid input data.",
                    Status = CommonStatusCode.BadRequest
                });
            }

            try
            {
                var createUser = await _userRepository.CreateUser(userCreateRequestDto, null);

                if (createUser == null)
                {
                    return Conflict(new CommonErrorResponse
                    {
                        Path = "/api/v1/register",
                        Message = CommonMessages.UserIsAlredyExits,
                        Status = CommonStatusCode.Conflict
                    });
                }

                return Ok(new CommonResponse<UserCommonResponseDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = createUser,
                    Message = CommonMessages.UserAddedSuccessfully
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a user.");
                return StatusCode(500, new CommonErrorResponse
                {
                    Path = "/api/v1/register",
                    Message = "An unexpected error occurred. Please try again later.",
                    Status = CommonStatusCode.InternalServerError
                });
            }
        }
    }
}
