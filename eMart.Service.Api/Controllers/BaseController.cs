using eMart.Service.Core.Dtos.User;
using eMart.Service.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eMart.Service.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IUserRepository _userRepository;

        public BaseController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected async Task<UserDto?> GetLoggedInUserAsync()
        {
            var userEmail = HttpContext.User.Claims.Single(x => x.Type == ClaimTypes.Email).Value;

            // Assuming GetUserDetails returns Task<UserDto?>
            var loggedInUser = await _userRepository.GetUserDetails(userEmail);

            return loggedInUser;
        }
    }
}
