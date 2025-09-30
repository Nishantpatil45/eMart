using eMart.Service.Core.Commands.Authentication;
using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eMart.Service.Core.Services
{
    public class EnhancedAuthenticationService : IEnhancedAuthenticationService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EnhancedAuthenticationService> _logger;

        public EnhancedAuthenticationService(IMediator mediator, ILogger<EnhancedAuthenticationService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<CommonResponse<EnhancedAuthResponseDto>> LoginAsync(LoginDto loginRequest, string? twoFactorCode = null)
        {
            _logger.LogInformation("Login attempt for user: {Email}", loginRequest.Email);
            
            var command = new EnhancedLoginCommand
            {
                LoginRequest = loginRequest,
                TwoFactorCode = twoFactorCode
            };

            var result = await _mediator.Send(command);
            
            if (result.Code == CommonStatusCode.Success)
            {
                _logger.LogInformation("Successful login for user: {Email}", loginRequest.Email);
            }
            else
            {
                _logger.LogWarning("Failed login attempt for user: {Email}, Reason: {Message}", loginRequest.Email, result.Message);
            }

            return result;
        }

        public async Task<CommonResponse<TokenDto>> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequest)
        {
            var command = new RefreshTokenCommand
            {
                RefreshTokenRequest = refreshTokenRequest
            };

            return await _mediator.Send(command);
        }

        public async Task<CommonResponse<TwoFactorSetupDto>> SetupTwoFactorAsync(string userId)
        {
            var command = new SetupTwoFactorCommand
            {
                UserId = userId
            };

            return await _mediator.Send(command);
        }

        public async Task<CommonResponse<bool>> VerifyTwoFactorAsync(TwoFactorVerificationDto twoFactorRequest)
        {
            var command = new VerifyTwoFactorCommand
            {
                TwoFactorRequest = twoFactorRequest
            };

            return await _mediator.Send(command);
        }

        public async Task<CommonResponse<bool>> DisableTwoFactorAsync(string userId)
        {
            var command = new DisableTwoFactorCommand
            {
                UserId = userId
            };

            return await _mediator.Send(command);
        }

        public async Task<CommonResponse<bool>> IsTwoFactorEnabledAsync(string userId)
        {
            var command = new IsTwoFactorEnabledCommand
            {
                UserId = userId
            };

            return await _mediator.Send(command);
        }
    }
}
