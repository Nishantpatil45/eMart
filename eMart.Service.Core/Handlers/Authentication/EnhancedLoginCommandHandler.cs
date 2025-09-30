using eMart.Service.Core.Commands.Authentication;
using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Interfaces;
using eMart.Service.Core.Services;
using MediatR;
using System.Security;

namespace eMart.Service.Core.Handlers.Authentication
{
    public class EnhancedLoginCommandHandler : IRequestHandler<EnhancedLoginCommand, CommonResponse<EnhancedAuthResponseDto>>
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly ITwoFactorAuthService _twoFactorAuthService;

        public EnhancedLoginCommandHandler(
            IAuthenticationRepository authenticationRepository,
            IUserRepository userRepository,
            IJwtService jwtService,
            ITwoFactorAuthService twoFactorAuthService)
        {
            _authenticationRepository = authenticationRepository;
            _userRepository = userRepository;
            _jwtService = jwtService;
            _twoFactorAuthService = twoFactorAuthService;
        }

        public async Task<CommonResponse<EnhancedAuthResponseDto>> Handle(EnhancedLoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // First, authenticate the user
                var authResponse = _authenticationRepository.Login(request.LoginRequest);
                if (authResponse == null)
                {
                    return new CommonResponse<EnhancedAuthResponseDto>
                    {
                        Code = CommonStatusCode.Unauthorized,
                        Message = "Invalid email or password",
                        Data = null
                    };
                }

                var user = await _userRepository.GetUserDetails(request.LoginRequest.Email);
                if (user == null)
                {
                    return new CommonResponse<EnhancedAuthResponseDto>
                    {
                        Code = CommonStatusCode.Unauthorized,
                        Message = "User not found",
                        Data = null
                    };
                }

                // Check if 2FA is enabled for this user
                var twoFactorEnabled = await _twoFactorAuthService.IsTwoFactorEnabledAsync(user.Id ?? string.Empty);

                if (twoFactorEnabled)
                {
                    // If 2FA is enabled but no code provided, return 2FA required
                    if (string.IsNullOrEmpty(request.TwoFactorCode))
                    {
                        return new CommonResponse<EnhancedAuthResponseDto>
                        {
                            Code = CommonStatusCode.Unauthorized,
                            Message = "Two-factor authentication code required",
                            Data = new EnhancedAuthResponseDto
                            {
                                UserId = user.Id ?? string.Empty,
                                Email = user.Email ?? string.Empty,
                                Name = user.Name ?? string.Empty,
                                Role = user.Role ?? string.Empty,
                                RequiresTwoFactor = true,
                                TwoFactorEnabled = true
                            }
                        };
                    }

                    // Verify 2FA code
                    var isTwoFactorValid = await _twoFactorAuthService.VerifyTwoFactorCodeAsync(user.Id ?? string.Empty, request.TwoFactorCode);
                    if (!isTwoFactorValid)
                    {
                        return new CommonResponse<EnhancedAuthResponseDto>
                        {
                            Code = CommonStatusCode.Unauthorized,
                            Message = "Invalid two-factor authentication code",
                            Data = null
                        };
                    }
                }

                // Generate tokens
                var tokenPair = _jwtService.GenerateTokenPair(user, twoFactorEnabled && !string.IsNullOrEmpty(request.TwoFactorCode));

                var response = new EnhancedAuthResponseDto
                {
                    UserId = user.Id ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Name = user.Name ?? string.Empty,
                    Role = user.Role ?? string.Empty,
                    AccessToken = tokenPair.AccessToken,
                    RefreshToken = tokenPair.RefreshToken,
                    AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(15), // Should match JWT config
                    RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7), // Should match JWT config
                    RequiresTwoFactor = false,
                    TwoFactorEnabled = twoFactorEnabled
                };

                return new CommonResponse<EnhancedAuthResponseDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = response,
                    Message = "Login successful"
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<EnhancedAuthResponseDto>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred during login",
                    Data = null
                };
            }
        }
    }
}
