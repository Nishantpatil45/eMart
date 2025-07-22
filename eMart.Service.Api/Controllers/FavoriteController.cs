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

                var product = await _favoriteRepository.AddToFavorite(id, loggedInUser);

                if (product == null)
                {
                    return BadRequest(new CommonResponse<FavoriteCommonResponseDto>()
                    {
                        Code = CommonStatusCode.BadRequest,
                        Message = CommonMessages.AlreadyInFavorite
                    });
                }

                return Ok(new CommonResponse<FavoriteCommonResponseDto>()
                {
                    Code = CommonStatusCode.Success,
                    Data = product,
                    Message = CommonMessages.ProductAddedInFavorite
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new CommonErrorResponse()
                {
                    Path = "/error",
                    Status = CommonStatusCode.BadRequest,
                    Message = ex.Message,
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

                return Ok(new CommonResponse<List<FavoriteCommonResponseDto>>()
                {
                    Code = CommonStatusCode.Success,
                    Data = favoritesProducts,
                    Message = CommonMessages.FavoriteFound
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new CommonErrorResponse()
                {
                    Path = "/error",
                    Status = CommonStatusCode.BadRequest,
                    Message = ex.Message,
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

                var result = await _favoriteRepository.RemoveFromFavorite(id, loggedInUser);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(new CommonResponse<FavoriteCommonResponseDto>()
                {
                    Code = CommonStatusCode.Success,
                    Data = result,
                    Message = CommonMessages.FavoriteRemoved
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new CommonErrorResponse()
                {
                    Path = "/error",
                    Status = CommonStatusCode.BadRequest,
                    Message = ex.Message,
                });
            }
        }
    }
}
