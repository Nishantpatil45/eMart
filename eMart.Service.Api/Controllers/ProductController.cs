using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Product;
using eMart.Service.Core.Interfaces;
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
        public ProductController(IUserRepository userRepository, IProductRepository productRepository) : base(userRepository)
        {
            _productRepository = productRepository;
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

                var newProduct = await _productRepository.CreateProduct(productCreateRequestDto, loggedInUser);

                return Ok(new CommonResponse<ProductCommonResponseDto>()
                {
                    Code = CommonStatusCode.Success,
                    Data = newProduct,
                    Message = CommonMessages.ProductAddedSuccessfully
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

                return Ok(new CommonResponse<List<ProductCommonResponseDto>>()
                {
                    Code = CommonStatusCode.Success,
                    Data = products,
                    Message = CommonMessages.ProductFound
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

                var product = await _productRepository.GetProductById(id, loggedInUser);
                return Ok(new CommonResponse<ProductCommonResponseDto>()
                {
                    Code = CommonStatusCode.Success,
                    Data = product,
                    Message = CommonMessages.ProductFound
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

                var products = await _productRepository.GetProductsByCategoryId(id, loggedInUser);

                return Ok(new CommonResponse<List<ProductCommonResponseDto>>()
                {
                    Code = CommonStatusCode.Success,
                    Data = products,
                    Message = CommonMessages.ProductFound
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

                var product = await _productRepository.UpdateProduct(id, productCreateRequestDto, loggedInUser);

                if (product == null)
                {
                    return NotFound(new Core.Dtos.Common.CommonErrorResponse()
                    {
                        Path = "/error",
                        Status = CommonStatusCode.NotFound,
                        Message = CommonMessages.ProductNotFound
                    });
                }

                return Ok(new CommonResponse<ProductCommonResponseDto>()
                {
                    Code = CommonStatusCode.Success,
                    Data = product,
                    Message = CommonMessages.ProductEditSuccessfully
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

                var product = await _productRepository.DeleteProduct(id, loggedInUser);

                return Ok(new CommonResponse<ProductCommonResponseDto>()
                {
                    Code = CommonStatusCode.Success,
                    Data = product,
                    Message = CommonMessages.ProductRemoved
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
