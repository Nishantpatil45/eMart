using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Favorite;
using eMart.Service.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eMart.Service.Api.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    [Authorize]
    public class FavoriteController : BaseController
    {
        private readonly IFavoriteRepository _favoriteRepository;
        public FavoriteController(IUserRepository userRepository, IFavoriteRepository favoriteRepository) : base(userRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        [HttpPost("Favorite/{id}")]
        public async Task<ActionResult> AddToFavorite(string id)
        {
            try
            {
                var loggedInUser = await this.GetLoggedInUserAsync();
                if (loggedInUser == null)
                {
                    return Unauthorized();
                }
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new CommonErrorResponse
                    {
                        Path = "/api/v1/Favorite/{id}",
                        Status = CommonStatusCode.BadRequest,
                        Message = "Invalid product id."
                    });
                }
                var product = await _favoriteRepository.AddToFavorite(id, loggedInUser);
                if (product == null)
                {
                    return BadRequest(new CommonResponse<FavoriteCommonResponseDto>
                    {
                        Code = CommonStatusCode.BadRequest,
                        Message = CommonMessages.AlreadyInFavorite
                    });
                }
                return Ok(new CommonResponse<FavoriteCommonResponseDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = product,
                    Message = CommonMessages.ProductAddedInFavorite
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddToFavorite: {ex.Message}");
                return StatusCode(500, new CommonErrorResponse
                {
                    Path = "/api/v1/Favorite/{id}",
                    Status = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpGet("Favorite/list")]
        public async Task<ActionResult> GetAllFavoriteProducts()
        {
            try
            {
                var loggedInUser = await this.GetLoggedInUserAsync();
                if (loggedInUser == null)
                {
                    return Unauthorized();
                }
                var favoritesProducts = await _favoriteRepository.GetAllFavoriteForLoggedInUser(loggedInUser);
                return Ok(new CommonResponse<List<FavoriteCommonResponseDto>>
                {
                    Code = CommonStatusCode.Success,
                    Data = favoritesProducts,
                    Message = CommonMessages.FavoriteFound
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllFavoriteProducts: {ex.Message}");
                return StatusCode(500, new CommonErrorResponse
                {
                    Path = "/api/v1/Favorite/list",
                    Status = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpDelete("Favorite/{id}")]
        public async Task<ActionResult> RemoveFromFavorite(string id)
        {
            try
            {
                var loggedInUser = await this.GetLoggedInUserAsync();
                if (loggedInUser == null)
                {
                    return Unauthorized();
                }
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new CommonErrorResponse
                    {
                        Path = "/api/v1/Favorite/{id}",
                        Status = CommonStatusCode.BadRequest,
                        Message = "Invalid product id."
                    });
                }
                var result = await _favoriteRepository.RemoveFromFavorite(id, loggedInUser);
                if (result == null)
                {
                    return NotFound(new CommonErrorResponse
                    {
                        Path = "/api/v1/Favorite/{id}",
                        Status = CommonStatusCode.NotFound,
                        Message = CommonMessages.FavoriteNotFound
                    });
                }
                return Ok(new CommonResponse<FavoriteCommonResponseDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = result,
                    Message = CommonMessages.FavoriteRemoved
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RemoveFromFavorite: {ex.Message}");
                return StatusCode(500, new CommonErrorResponse
                {
                    Path = "/api/v1/Favorite/{id}",
                    Status = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }
    }
}
