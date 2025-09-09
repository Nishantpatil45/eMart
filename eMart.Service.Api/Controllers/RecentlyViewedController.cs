using eMart.Service.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eMart.Service.Api.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    [Authorize]
    public class RecentlyViewedController : BaseController
    {
        private readonly IRecentlyViewedRepository _recentlyViewedRepository;

        public RecentlyViewedController(IRecentlyViewedRepository recentlyViewedRepository, IUserRepository userRepository)
            : base(userRepository)
        {
            _recentlyViewedRepository = recentlyViewedRepository;
        }

        [HttpPost("RecentlyViewed/{productId}")]
        public async Task<IActionResult> AddRecentlyViewed(string productId)
        {
            var loggedInUser = await GetLoggedInUserAsync();
            if (loggedInUser == null)
                return Unauthorized();

            await _recentlyViewedRepository.AddRecentlyViewed(productId, loggedInUser.Id);
            return Ok(new { Message = "Product added to recently viewed." });
        }

        [HttpGet("RecentlyViewed")]
        public async Task<IActionResult> GetRecentlyViewed()
        {
            var loggedInUser = await GetLoggedInUserAsync();
            if (loggedInUser == null)
                return Unauthorized();

            var products = await _recentlyViewedRepository.GetRecentlyViewedProducts(loggedInUser.Id);
            return Ok(products);
        }
    }
}
