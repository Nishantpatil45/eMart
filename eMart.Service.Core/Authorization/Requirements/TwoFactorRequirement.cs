using Microsoft.AspNetCore.Authorization;

namespace eMart.Service.Core.Authorization.Requirements
{
    public class TwoFactorRequirement : IAuthorizationRequirement
    {
        public bool RequireTwoFactor { get; }

        public TwoFactorRequirement(bool requireTwoFactor = true)
        {
            RequireTwoFactor = requireTwoFactor;
        }
    }
}
