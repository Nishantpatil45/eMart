using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Product;
using eMart.Service.Core.Interfaces;
using eMart.Service.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eMart.Service.Api.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    [Authorize]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        
        public ProductController(IUserRepository userRepository, IProductService productService) : base(userRepository)
        {
            _productService = productService;
        }

        [HttpPost("AddProduct")]
        [Authorize(Roles = "Admin,Seller")]
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

                var result = await _productService.CreateProductAsync(productCreateRequestDto, loggedInUser.Email);
                
                return result.Code switch
                {
                    CommonStatusCode.Success => Ok(result),
                    CommonStatusCode.BadRequest => BadRequest(new CommonErrorResponse
                    {
                        Path = "/api/v1/AddProduct",
                        Status = CommonStatusCode.BadRequest,
                        Message = result.Message
                    }),
                    CommonStatusCode.Unauthorized => Unauthorized(),
                    _ => StatusCode(500, new CommonErrorResponse
                    {
                        Path = "/api/v1/AddProduct",
                        Status = CommonStatusCode.InternalServerError,
                        Message = result.Message
                    })
                };
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

                var result = await _productService.GetAllProductsAsync(loggedInUser.Email);
                
                return result.Code switch
                {
                    CommonStatusCode.Success => Ok(result),
                    CommonStatusCode.Unauthorized => Unauthorized(),
                    _ => StatusCode(500, new CommonErrorResponse
                    {
                        Path = "/api/v1/Product/list",
                        Status = CommonStatusCode.InternalServerError,
                        Message = result.Message
                    })
                };
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

                var result = await _productService.GetProductByIdAsync(id, loggedInUser.Email);
                
                return result.Code switch
                {
                    CommonStatusCode.Success => Ok(result),
                    CommonStatusCode.NotFound => NotFound(new CommonErrorResponse
                    {
                        Path = "/api/v1/Product/{id}",
                        Status = CommonStatusCode.NotFound,
                        Message = result.Message
                    }),
                    CommonStatusCode.Unauthorized => Unauthorized(),
                    _ => StatusCode(500, new CommonErrorResponse
                    {
                        Path = "/api/v1/Product/{id}",
                        Status = CommonStatusCode.InternalServerError,
                        Message = result.Message
                    })
                };
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

                var result = await _productService.GetProductsByCategoryAsync(id, loggedInUser.Email);
                
                return result.Code switch
                {
                    CommonStatusCode.Success => Ok(result),
                    CommonStatusCode.Unauthorized => Unauthorized(),
                    _ => StatusCode(500, new CommonErrorResponse
                    {
                        Path = "/api/v1/Product/Category/{id}",
                        Status = CommonStatusCode.InternalServerError,
                        Message = result.Message
                    })
                };
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
        [Authorize(Roles = "Admin,Seller")]
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

                var result = await _productService.UpdateProductAsync(id, productCreateRequestDto, loggedInUser.Email);
                
                return result.Code switch
                {
                    CommonStatusCode.Success => Ok(result),
                    CommonStatusCode.NotFound => NotFound(new CommonErrorResponse
                    {
                        Path = "/api/v1/Product/{id}",
                        Status = CommonStatusCode.NotFound,
                        Message = result.Message
                    }),
                    CommonStatusCode.Unauthorized => Unauthorized(),
                    _ => StatusCode(500, new CommonErrorResponse
                    {
                        Path = "/api/v1/Product/{id}",
                        Status = CommonStatusCode.InternalServerError,
                        Message = result.Message
                    })
                };
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
        [Authorize(Roles = "Admin,Seller")]
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

                var result = await _productService.DeleteProductAsync(id, loggedInUser.Email);
                
                return result.Code switch
                {
                    CommonStatusCode.Success => Ok(result),
                    CommonStatusCode.NotFound => NotFound(new CommonErrorResponse
                    {
                        Path = "/api/v1/Product/{id}",
                        Status = CommonStatusCode.NotFound,
                        Message = result.Message
                    }),
                    CommonStatusCode.Unauthorized => Unauthorized(),
                    _ => StatusCode(500, new CommonErrorResponse
                    {
                        Path = "/api/v1/Product/{id}",
                        Status = CommonStatusCode.InternalServerError,
                        Message = result.Message
                    })
                };
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
