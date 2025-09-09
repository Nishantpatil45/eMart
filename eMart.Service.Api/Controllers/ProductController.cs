using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Product;
using eMart.Service.Core.Interfaces;
using eMart.Service.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eMart.Service.Api.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    [Authorize]
    public class ProductController : BaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly IRecentlyViewedRepository _recentlyViewedRepository;
        public ProductController(IUserRepository userRepository, IProductRepository productRepository, IRecentlyViewedRepository recentlyViewedRepository) : base(userRepository)
        {
            _productRepository = productRepository;
            _recentlyViewedRepository = recentlyViewedRepository;
        }

        [HttpPost("AddProduct")]
        public async Task<ActionResult> AddProduct([FromBody] ProductCreateRequestDto productCreateRequestDto)
        {
            try
            {
                var loggedInUser = await this.GetLoggedInUserAsync();
                if (loggedInUser == null)
                {
                    return Unauthorized();
                }
                if (productCreateRequestDto == null)
                {
                    return BadRequest(new CommonErrorResponse
                    {
                        Path = "/api/v1/AddProduct",
                        Status = CommonStatusCode.BadRequest,
                        Message = "Invalid product data."
                    });
                }
                var newProduct = await _productRepository.CreateProduct(productCreateRequestDto, loggedInUser);
                if (newProduct == null)
                {
                    return BadRequest(new CommonErrorResponse
                    {
                        Path = "/api/v1/AddProduct",
                        Status = CommonStatusCode.BadRequest,
                        Message = "Failed to create product."
                    });
                }
                return Ok(new CommonResponse<ProductCommonResponseDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = newProduct,
                    Message = CommonMessages.ProductAddedSuccessfully
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddProduct: {ex.Message}");
                return StatusCode(500, new CommonErrorResponse
                {
                    Path = "/api/v1/AddProduct",
                    Status = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpGet("Product/list")]
        public async Task<ActionResult> GetAllProducts()
        {
            try
            {
                var loggedInUser = await this.GetLoggedInUserAsync();
                if (loggedInUser == null)
                {
                    return Unauthorized();
                }
                var products = await _productRepository.GetProduct(loggedInUser);
                return Ok(new CommonResponse<List<ProductCommonResponseDto>>
                {
                    Code = CommonStatusCode.Success,
                    Data = products,
                    Message = CommonMessages.ProductFound
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllProducts: {ex.Message}");
                return StatusCode(500, new CommonErrorResponse
                {
                    Path = "/api/v1/Product/list",
                    Status = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpGet("Product/{id}")]
        public async Task<ActionResult> GetProductById(string id)
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
                        Path = "/api/v1/Product/{id}",
                        Status = CommonStatusCode.BadRequest,
                        Message = "Invalid product id."
                    });
                }
                var product = await _productRepository.GetProductById(id, loggedInUser);
                if (product == null)
                {
                    return NotFound(new CommonErrorResponse
                    {
                        Path = "/api/v1/Product/{id}",
                        Status = CommonStatusCode.NotFound,
                        Message = CommonMessages.ProductNotFound
                    });
                }
                // Track recently viewed product
                await _recentlyViewedRepository.AddRecentlyViewed(id, loggedInUser.Id);
                return Ok(new CommonResponse<ProductCommonResponseDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = product,
                    Message = CommonMessages.ProductFound
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetProductById: {ex.Message}");
                return StatusCode(500, new CommonErrorResponse
                {
                    Path = "/api/v1/Product/{id}",
                    Status = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpGet("Product/Category/{id}")]
        public async Task<ActionResult> GetAllProductsByCategoryId(string id)
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
                        Path = "/api/v1/Product/Category/{id}",
                        Status = CommonStatusCode.BadRequest,
                        Message = "Invalid category id."
                    });
                }
                var products = await _productRepository.GetProductsByCategoryId(id, loggedInUser);
                return Ok(new CommonResponse<List<ProductCommonResponseDto>>
                {
                    Code = CommonStatusCode.Success,
                    Data = products,
                    Message = CommonMessages.ProductFound
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllProductsByCategoryId: {ex.Message}");
                return StatusCode(500, new CommonErrorResponse
                {
                    Path = "/api/v1/Product/Category/{id}",
                    Status = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpPut("Product/{id}")]
        public async Task<ActionResult> UpdateProduct(string id, ProductCreateRequestDto productCreateRequestDto)
        {
            try
            {
                var loggedInUser = await this.GetLoggedInUserAsync();
                if (loggedInUser == null)
                {
                    return Unauthorized();
                }
                if (string.IsNullOrWhiteSpace(id) || productCreateRequestDto == null)
                {
                    return BadRequest(new CommonErrorResponse
                    {
                        Path = "/api/v1/Product/{id}",
                        Status = CommonStatusCode.BadRequest,
                        Message = "Invalid product id or data."
                    });
                }
                var product = await _productRepository.UpdateProduct(id, productCreateRequestDto, loggedInUser);
                if (product == null)
                {
                    return NotFound(new Core.Dtos.Common.CommonErrorResponse()
                    {
                        Path = "/api/v1/Product/{id}",
                        Status = CommonStatusCode.NotFound,
                        Message = CommonMessages.ProductNotFound
                    });
                }
                return Ok(new CommonResponse<ProductCommonResponseDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = product,
                    Message = CommonMessages.ProductEditSuccessfully
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateProduct: {ex.Message}");
                return StatusCode(500, new CommonErrorResponse
                {
                    Path = "/api/v1/Product/{id}",
                    Status = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpDelete("Product/{id}")]
        public async Task<ActionResult> DeleteProduct(string id)
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
                        Path = "/api/v1/Product/{id}",
                        Status = CommonStatusCode.BadRequest,
                        Message = "Invalid product id."
                    });
                }
                var product = await _productRepository.DeleteProduct(id, loggedInUser);
                if (product == null)
                {
                    return NotFound(new CommonErrorResponse
                    {
                        Path = "/api/v1/Product/{id}",
                        Status = CommonStatusCode.NotFound,
                        Message = CommonMessages.ProductNotFound
                    });
                }
                return Ok(new CommonResponse<ProductCommonResponseDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = product,
                    Message = CommonMessages.ProductRemoved
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteProduct: {ex.Message}");
                return StatusCode(500, new CommonErrorResponse
                {
                    Path = "/api/v1/Product/{id}",
                    Status = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }
    }
}
