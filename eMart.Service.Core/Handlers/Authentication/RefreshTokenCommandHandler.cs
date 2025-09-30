using eMart.Service.Core.Commands.Authentication;
using eMart.Service.Core.Dtos.Authentication;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Services;
using MediatR;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using Microsoft.IdentityModel.Tokens;

namespace eMart.Service.Core.Handlers.Authentication
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, CommonResponse<TokenDto>>
    {
        private readonly IJwtService _jwtService;

        public RefreshTokenCommandHandler(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public async Task<CommonResponse<TokenDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate the access token (even if expired)
                var principal = _jwtService.GetPrincipalFromExpiredToken(request.RefreshTokenRequest.AccessToken);
                if (principal == null)
                {
                    return new CommonResponse<TokenDto>
                    {
                        Code = CommonStatusCode.Unauthorized,
                        Message = "Invalid access token",
                        Data = null
                    };
                }

                // In a real implementation, you would validate the refresh token against the database
                // For now, we'll generate a new token pair
                var newTokenPair = _jwtService.RefreshTokenPair(
                    request.RefreshTokenRequest.AccessToken, 
                    request.RefreshTokenRequest.RefreshToken);

                return new CommonResponse<TokenDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = newTokenPair,
                    Message = "Token refreshed successfully"
                };
            }
            catch (SecurityTokenException)
            {
                return new CommonResponse<TokenDto>
                {
                    Code = CommonStatusCode.Unauthorized,
                    Message = "Invalid or expired token",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<TokenDto>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while refreshing token",
                    Data = null
                };
            }
        }
    }
}