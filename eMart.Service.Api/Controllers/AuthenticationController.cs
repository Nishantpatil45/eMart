using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eMart.Service.Api.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepository _authRepository;

        public AuthenticationController(IAuthenticationRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("login")]
        //[AllowAnonymous]
        public IActionResult Login(LoginDto loginDto)
        {
            try
            {
                var existinglogin = this._authRepository.Login(loginDto);
                if (existinglogin.Error == "Not Found")
                {

                    return this.NotFound(new CommonErrorResponse()
                    {
                        Path = "/error",
                        Message = CommonMessages.UserNotFound,
                        Status = CommonStatusCode.NotFound,
                    });
                }
                else if (existinglogin.Error == "PasswordInvalid")
                {
                    return this.BadRequest(new CommonErrorResponse()
                    {
                        Path = "/error",
                        Message = CommonMessages.InvalidPassword,
                        Status = CommonStatusCode.BadRequest,
                    });
                }
                else
                {
                    return this.Ok(new CommonResponse<UserAuthResponseDto>()
                    {
                        Code = CommonStatusCode.Success,
                        Message = "Access Token",
                        Data = existinglogin,
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new CommonErrorResponse()
                {
                    Path = "/error",
                    Status = CommonStatusCode.BadRequest,
                    Message = ex.Message,
                });
            }
        }


        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var accessToken = Request.Cookies["refresh_token"];
                if (string.IsNullOrEmpty(accessToken))
                {
                    return this.BadRequest(new CommonErrorResponse()
                    {
                        Path = "/error",
                        Message = "Refresh token is missing.",
                        Status = CommonStatusCode.BadRequest,
                    });
                }

                var logoutUser = this._authRepository.Logout(accessToken);
                if (logoutUser == null)
                {
                    return this.NotFound(new CommonErrorResponse()
                    {
                        Path = "/error",
                        Message = CommonMessages.TokenSignatureError,
                        Status = CommonStatusCode.NotFound,
                    });
                }
                return Ok(logoutUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new CommonErrorResponse()
                {
                    Path = "/error",
                    Status = CommonStatusCode.BadRequest,
                    Message = ex.Message,
                });
            }
        }

        [HttpGet("refresh-token")]
        //[AllowAnonymous]
        public IActionResult RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refresh_token"];
                var token = this._authRepository.RefreshToken(refreshToken);
                if (token == null)
                {
                    return this.NotFound(new CommonErrorResponse()
                    {
                        Path = "/error",
                        Message = CommonMessages.TokenNotFound,
                        Status = CommonStatusCode.NotFound
                    });
                }
                else if (token.Error != null)
                {
                    return this.BadRequest(new CommonErrorResponse()
                    {
                        Path = "/error",
                        Message = CommonMessages.TokenExpired,
                        Status = CommonStatusCode.BadRequest,
                    });
                }
                else
                {
                    return this.Ok(token);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new CommonErrorResponse()
                {
                    Path = "/error",
                    Status = CommonStatusCode.BadRequest,
                    Message = ex.Message,
                });
            }
        }
    }
}
