using eMart.Service.Core.Authorization.Requirements;
using eMart.Service.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace eMart.Service.Core.Authorization.Handlers
{
    public class TwoFactorAuthorizationHandler : AuthorizationHandler<TwoFactorRequirement>
    {
        private readonly IUserRepository _userRepository;

        public TwoFactorAuthorizationHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            TwoFactorRequirement requirement)
        {
            if (!requirement.RequireTwoFactor)
            {
                context.Succeed(requirement);
                return;
            }

            var userEmail = context.User.FindFirst(ClaimTypes.Email)?.Value;
            
            if (string.IsNullOrEmpty(userEmail))
            {
                context.Fail();
                return;
            }

            var user = await _userRepository.GetUserDetails(userEmail);
            
            if (user == null)
            {
                context.Fail();
                return;
            }

            // Check if user has 2FA enabled and verified
            var twoFactorEnabled = context.User.FindFirst("2fa_enabled")?.Value == "true";
            var twoFactorVerified = context.User.FindFirst("2fa_verified")?.Value == "true";

            if (twoFactorEnabled && twoFactorVerified)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
