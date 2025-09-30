using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eMart.Service.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize] // Require authentication for all endpoints
    public class AdminController : ControllerBase
    {
        [HttpGet("admin-only")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminOnly()
        {
            return Ok(new { message = "This endpoint is only accessible by Admin users", timestamp = DateTime.UtcNow });
        }

        [HttpGet("admin-or-seller")]
        [Authorize(Policy = "AdminOrSeller")]
        public IActionResult AdminOrSeller()
        {
            return Ok(new { message = "This endpoint is accessible by Admin or Seller users", timestamp = DateTime.UtcNow });
        }

        [HttpGet("user-or-above")]
        [Authorize(Policy = "UserOrAbove")]
        public IActionResult UserOrAbove()
        {
            return Ok(new { message = "This endpoint is accessible by User, Seller, or Admin users", timestamp = DateTime.UtcNow });
        }

        [HttpGet("require-2fa")]
        [Authorize(Policy = "RequireTwoFactor")]
        public IActionResult RequireTwoFactor()
        {
            return Ok(new { message = "This endpoint requires two-factor authentication", timestamp = DateTime.UtcNow });
        }

        [HttpGet("admin-with-2fa")]
        [Authorize(Policy = "AdminWithTwoFactor")]
        public IActionResult AdminWithTwoFactor()
        {
            return Ok(new { message = "This endpoint requires Admin role and two-factor authentication", timestamp = DateTime.UtcNow });
        }

        [HttpGet("seller-with-2fa")]
        [Authorize(Policy = "SellerWithTwoFactor")]
        public IActionResult SellerWithTwoFactor()
        {
            return Ok(new { message = "This endpoint requires Seller role and two-factor authentication", timestamp = DateTime.UtcNow });
        }

        [HttpGet("user-info")]
        public IActionResult GetUserInfo()
        {
            var userClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(new { 
                message = "User information", 
                claims = userClaims,
                timestamp = DateTime.UtcNow 
            });
        }
    }
}
