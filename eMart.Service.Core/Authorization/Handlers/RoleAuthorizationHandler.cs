using eMart.Service.Core.Authorization.Requirements;
using eMart.Service.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace eMart.Service.Core.Authorization.Handlers
{
    public class RoleAuthorizationHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly IUserRepository _userRepository;

        public RoleAuthorizationHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RoleRequirement requirement)
        {
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

            if (requirement.AllowedRoles.Contains(user.Role, StringComparer.OrdinalIgnoreCase))
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
