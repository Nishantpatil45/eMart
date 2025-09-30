using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace eMart.Service.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EnhancedAuthenticationController : ControllerBase
    {
        private readonly IEnhancedAuthenticationService _authService;

        public EnhancedAuthenticationController(IEnhancedAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new CommonErrorResponse
                {
                    Path = "/api/v1/EnhancedAuthentication/login",
                    Status = CommonStatusCode.BadRequest,
                    Message = "Invalid login request"
                });
            }

            try
            {
                var result = await _authService.LoginAsync(loginRequest);

                return result.Code switch
                {
                    CommonStatusCode.Success => Ok(result),
                    CommonStatusCode.Unauthorized => Unauthorized(new CommonErrorResponse
                    {
                        Path = "/api/v1/EnhancedAuthentication/login",
                        Status = CommonStatusCode.Unauthorized,
                        Message = result.Message
                    }),
                    _ => StatusCode(500, new CommonErrorResponse
                    {
                        Path = "/api/v1/EnhancedAuthentication/login",
                        Status = CommonStatusCode.InternalServerError,
                        Message = result.Message
                    })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CommonErrorResponse
                {
                    Path = "/api/v1/EnhancedAuthentication/login",
                    Status = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred during login"
                });
            }
        }

        [HttpPost("login-with-2fa")]
        public async Task<ActionResult> LoginWithTwoFactor([FromBody] LoginWithTwoFactorDto loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new CommonErrorResponse
                {
                    Path = "/api/v1/EnhancedAuthentication/login-with-2fa",
                    Status = CommonStatusCode.BadRequest,
                    Message = "Invalid login request"
                });
            }

            try
            {
                var result = await _authService.LoginAsync(loginRequest.LoginRequest, loginRequest.TwoFactorCode);

                return result.Code switch
                {
                    CommonStatusCode.Success => Ok(result),
                    CommonStatusCode.Unauthorized => Unauthorized(new CommonErrorResponse
                    {
                        Path = "/api/v1/EnhancedAuthentication/login-with-2fa",
                        Status = CommonStatusCode.Unauthorized,
                        Message = result.Message
                    }),
                    _ => StatusCode(500, new CommonErrorResponse
                    {
                        Path = "/api/v1/EnhancedAuthentication/login-with-2fa",
                        Status = CommonStatusCode.InternalServerError,
                        Message = result.Message
                    })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CommonErrorResponse
                {
                    Path = "/api/v1/EnhancedAuthentication/login-with-2fa",
                    Status = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred during login"
                });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequestDto refreshTokenRequest)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(refreshTokenRequest);

                return result.Code switch
                {
                    CommonStatusCode.Success => Ok(result),
                    CommonStatusCode.Unauthorized => Unauthorized(new CommonErrorResponse
                    {
                        Path = "/api/v1/EnhancedAuthentication/refresh-token",
                        Status = CommonStatusCode.Unauthorized,
                        Message = result.Message
                    }),
                    _ => StatusCode(500, new CommonErrorResponse
                    {
                        Path = "/api/v1/EnhancedAuthentication/refresh-token",
                        Status = CommonStatusCode.InternalServerError,
                        Message = result.Message
                    })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CommonErrorResponse
                {
                    Path = "/api/v1/EnhancedAuthentication/refresh-token",
                    Status = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while refreshing token"
                });
            }
        }

        [HttpPost("setup-2fa")]
        [Authorize]
        public async Task<ActionResult> SetupTwoFactor([FromBody] SetupTwoFactorRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new CommonErrorResponse
                {
                    Path = "/api/v1/EnhancedAuthentication/setup-2fa",
                    Status = CommonStatusCode.BadRequest,
                    Message = "Invalid request"
                });
            }

            try
            {
                var result = await _authService.SetupTwoFactorAsync(request.UserId);

                return result.Code switch
                {
                    CommonStatusCode.Success => Ok(result),
                    CommonStatusCode.Unauthorized => Unauthorized(new CommonErrorResponse
                    {
                        Path = "/api/v1/EnhancedAuthentication/setup-2fa",
                        Status = CommonStatusCode.Unauthorized,
                        Message = result.Message
                    }),
                    _ => StatusCode(500, new CommonErrorResponse
                    {
                        Path = "/api/v1/EnhancedAuthentication/setup-2fa",
                        Status = CommonStatusCode.InternalServerError,
                        Message = result.Message
                    })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CommonErrorResponse
                {
                    Path = "/api/v1/EnhancedAuthentication/setup-2fa",
                    Status = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while setting up two-factor authentication"
                });
            }
        }

        [HttpPost("verify-2fa")]
        [Authorize]
        public async Task<ActionResult> VerifyTwoFactor([FromBody] TwoFactorVerificationDto twoFactorRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new CommonErrorResponse
                {
                    Path = "/api/v1/EnhancedAuthentication/verify-2fa",
                    Status = CommonStatusCode.BadRequest,
                    Message = "Invalid request"
                });
            }

            try
            {
                var result = await _authService.VerifyTwoFactorAsync(twoFactorRequest);

                return result.Code switch
                {
                    CommonStatusCode.Success => Ok(result),
                    CommonStatusCode.Unauthorized => Unauthorized(new CommonErrorResponse
                    {
                        Path = "/api/v1/EnhancedAuthentication/verify-2fa",
                        Status = CommonStatusCode.Unauthorized,
                        Message = result.Message
                    }),
                    _ => StatusCode(500, new CommonErrorResponse
                    {
                        Path = "/api/v1/EnhancedAuthentication/verify-2fa",
                        Status = CommonStatusCode.InternalServerError,
                        Message = result.Message
                    })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CommonErrorResponse
                {
                    Path = "/api/v1/EnhancedAuthentication/verify-2fa",
                    Status = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while verifying two-factor authentication"
                });
            }
        }

        [HttpPost("disable-2fa")]
        [Authorize]
        public async Task<ActionResult> DisableTwoFactor([FromBody] DisableTwoFactorRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new CommonErrorResponse
                {
                    Path = "/api/v1/EnhancedAuthentication/disable-2fa",
                    Status = CommonStatusCode.BadRequest,
                    Message = "Invalid request"
                });
            }

            try
            {
                var result = await _authService.DisableTwoFactorAsync(request.UserId);

                return result.Code switch
                {
                    CommonStatusCode.Success => Ok(result),
                    CommonStatusCode.Unauthorized => Unauthorized(new CommonErrorResponse
                    {
                        Path = "/api/v1/EnhancedAuthentication/disable-2fa",
                        Status = CommonStatusCode.Unauthorized,
                        Message = result.Message
                    }),
                    _ => StatusCode(500, new CommonErrorResponse
                    {
                        Path = "/api/v1/EnhancedAuthentication/disable-2fa",
                        Status = CommonStatusCode.InternalServerError,
                        Message = result.Message
                    })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CommonErrorResponse
                {
                    Path = "/api/v1/EnhancedAuthentication/disable-2fa",
                    Status = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while disabling two-factor authentication"
                });
            }
        }

        [HttpGet("2fa-status/{userId}")]
        [Authorize]
        public async Task<ActionResult> GetTwoFactorStatus(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new CommonErrorResponse
                {
                    Path = "/api/v1/EnhancedAuthentication/2fa-status",
                    Status = CommonStatusCode.BadRequest,
                    Message = "UserId is required"
                });
            }

            try
            {
                var result = await _authService.IsTwoFactorEnabledAsync(userId);

                return result.Code switch
                {
                    CommonStatusCode.Success => Ok(result),
                    CommonStatusCode.Unauthorized => Unauthorized(new CommonErrorResponse
                    {
                        Path = "/api/v1/EnhancedAuthentication/2fa-status",
                        Status = CommonStatusCode.Unauthorized,
                        Message = result.Message
                    }),
                    _ => StatusCode(500, new CommonErrorResponse
                    {
                        Path = "/api/v1/EnhancedAuthentication/2fa-status",
                        Status = CommonStatusCode.InternalServerError,
                        Message = result.Message
                    })
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CommonErrorResponse
                {
                    Path = "/api/v1/EnhancedAuthentication/2fa-status",
                    Status = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while checking two-factor authentication status"
                });
            }
        }
    }

    // Additional DTOs for the controller
    public class LoginWithTwoFactorDto
    {
        [Required]
        public LoginDto LoginRequest { get; set; } = null!;
        
        [Required]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Two-factor code must be exactly 6 digits")]
        public string? TwoFactorCode { get; set; }
    }

    public class SetupTwoFactorRequestDto
    {
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "UserId is required")]
        public string UserId { get; set; } = string.Empty;
    }

    public class DisableTwoFactorRequestDto
    {
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "UserId is required")]
        public string UserId { get; set; } = string.Empty;
    }
}
