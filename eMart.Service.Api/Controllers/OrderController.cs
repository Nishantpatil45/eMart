using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Order;
using eMart.Service.Core.Dtos.Product;
using eMart.Service.Core.Interfaces;
using eMart.Service.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eMart.Service.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderController(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IUserRepository userRepository) : base(userRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Create a new order
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CommonResponse<OrderCommonResponseDto>>> CreateOrder([FromBody] OrderCreateRequestDto request)
        {
            try
            {
                var loggedInUser = await GetLoggedInUserAsync();
                if (loggedInUser == null)
                {
                    return Unauthorized(new CommonErrorResponse()
                    {
                        Path = "/error",
                        Message = CommonMessages.Unauthorized,
                        Status = CommonStatusCode.Unauthorized,
                    });
                }

                // Validate order items
                if (request.OrderItems == null || !request.OrderItems.Any())
                {
                    return BadRequest(new CommonErrorResponse()
                    {
                        Path = "/error",
                        Message = "Order must contain at least one item",
                        Status = CommonStatusCode.BadRequest,
                    });
                }

                // Validate products and calculate totals
                decimal totalAmount = 0;
                var orderItems = new List<OrderItem>();

                foreach (var item in request.OrderItems)
                {
                    var product = await _productRepository.GetProductById(item.ProductId, loggedInUser);
                    if (product == null)
                    {
                        return BadRequest(new CommonErrorResponse()
                        {
                            Path = "/error",
                            Message = $"Product with ID {item.ProductId} not found",
                            Status = CommonStatusCode.BadRequest,
                        });
                    }

                    if (product.Stock < item.Quantity)
                    {
                        return BadRequest(new CommonErrorResponse()
                        {
                            Path = "/error",
                            Message = $"Insufficient stock for product {product.Name}. Available: {product.Stock}, Requested: {item.Quantity}",
                            Status = CommonStatusCode.BadRequest,
                        });
                    }

                    var orderItem = new OrderItem
                    {
                        ProductId = item.ProductId,
                        SellerId = product.SellerId ?? throw new InvalidOperationException("Product must have a seller"),
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.Quantity * item.UnitPrice,
                        ProductName = product.Name,
                        ProductImageUrl = product.ImageUrl,
                        ProductDescription = product.Description
                    };

                    orderItems.Add(orderItem);
                    totalAmount += orderItem.TotalPrice;

                    // Update product stock
                    product.Stock -= item.Quantity;
                    var productUpdateDto = new ProductCreateRequestDto
                    {
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        CategoryId = product.CategoryId,
                        Brand = product.Brand,
                        Stock = (int)product.Stock,
                        ImageUrl = product.ImageUrl,
                        Status = (int)product.Status
                    };

                    await _productRepository.UpdateProduct(product.Id, productUpdateDto, loggedInUser);
                }

                // Create order
                var order = new Order
                {
                    BuyerId = loggedInUser.Id,
                    TotalAmount = totalAmount + request.TaxAmount + request.ShippingAmount,
                    TaxAmount = request.TaxAmount,
                    ShippingAmount = request.ShippingAmount,
                    PaymentMethod = request.PaymentMethod,
                    ShippingAddress = request.ShippingAddress,
                    BillingAddress = request.BillingAddress,
                    Notes = request.Notes,
                    OrderItems = orderItems
                };

                var createdOrder = await _orderRepository.CreateAsync(order);
                var response = MapToOrderResponseDto(createdOrder);

                return Ok(new CommonResponse<OrderCommonResponseDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = response,
                    Message = "Order created successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CommonErrorResponse()
                {
                    Path = "/error",
                    Message = $"An error occurred while creating the order: {ex.Message}",
                    Status = CommonStatusCode.InternalServerError,
                });
            }
        }

        /// <summary>
        /// Get order by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CommonResponse<OrderCommonResponseDto>>> GetOrder(string id)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound(new CommonErrorResponse()
                    {
                        Path = "/error",
                        Message = "Order not found",
                        Status = CommonStatusCode.NotFound,
                    });
                }

                var loggedInUser = await GetLoggedInUserAsync();
                if (loggedInUser == null)
                {
                    return Unauthorized(new CommonErrorResponse()
                    {
                        Path = "/error",
                        Message = CommonMessages.Unauthorized,
                        Status = CommonStatusCode.Unauthorized,
                    });
                }

                // Check if user has access to this order
                if (order.BuyerId != loggedInUser.Id && !IsAdmin(loggedInUser))
                {
                    return Forbid();
                }

                var response = MapToOrderResponseDto(order);
                return Ok(new CommonResponse<OrderCommonResponseDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = response,
                    Message = "Order retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CommonErrorResponse()
                {
                    Path = "/error",
                    Message = $"An error occurred while retrieving the order: {ex.Message}",
                    Status = CommonStatusCode.InternalServerError,
                });
            }
        }

        /// <summary>
        /// Get order by order number
        /// </summary>
        [HttpGet("number/{orderNumber}")]
        public async Task<ActionResult<CommonResponse<OrderCommonResponseDto>>> GetOrderByNumber(string orderNumber)
        {
            try
            {
                var order = await _orderRepository.GetByOrderNumberAsync(orderNumber);
                if (order == null)
                {
                    return NotFound(new CommonErrorResponse()
                    {
                        Path = "/error",
                        Message = "Order not found",
                        Status = CommonStatusCode.NotFound,
                    });
                }

                var loggedInUser = await GetLoggedInUserAsync();
                if (loggedInUser == null)
                {
                    return Unauthorized(new CommonErrorResponse()
                    {
                        Path = "/error",
                        Message = CommonMessages.Unauthorized,
                        Status = CommonStatusCode.Unauthorized,
                    });
                }

                // Check if user has access to this order
                if (order.BuyerId != loggedInUser.Id && !IsAdmin(loggedInUser))
                {
                    return Forbid();
                }

                var response = MapToOrderResponseDto(order);
                return Ok(new CommonResponse<OrderCommonResponseDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = response,
                    Message = "Order retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CommonErrorResponse()
                {
                    Path = "/error",
                    Message = $"An error occurred while retrieving the order: {ex.Message}",
                    Status = CommonStatusCode.InternalServerError,
                });
            }
        }

        /// <summary>
        /// Get orders for the logged-in user
        /// </summary>
        [HttpGet("my-orders")]
        public async Task<ActionResult<CommonResponse<List<OrderSummaryDto>>>> GetMyOrders(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var loggedInUser = await GetLoggedInUserAsync();
                if (loggedInUser == null)
                {
                    return Unauthorized(new CommonErrorResponse()
                    {
                        Path = "/error",
                        Message = CommonMessages.Unauthorized,
                        Status = CommonStatusCode.Unauthorized,
                    });
                }

                var orders = await _orderRepository.GetByBuyerIdAsync(loggedInUser.Id, page, pageSize);
                var response = orders.Select(MapToOrderSummaryDto).ToList();

                return Ok(new CommonResponse<List<OrderSummaryDto>>
                {
                    Code = CommonStatusCode.Success,
                    Data = response,
                    Message = "Orders retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CommonErrorResponse()
                {
                    Path = "/error",
                    Message = $"An error occurred while retrieving orders: {ex.Message}",
                    Status = CommonStatusCode.InternalServerError,
                });
            }
        }

        /// <summary>
        /// Get orders for a seller (admin only)
        /// </summary>
        [HttpGet("seller/{sellerId}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<CommonResponse<List<OrderSummaryDto>>>> GetSellerOrders(
            string sellerId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var orders = await _orderRepository.GetBySellerIdAsync(sellerId, page, pageSize);
                var response = orders.Select(MapToOrderSummaryDto).ToList();

                return Ok(new CommonResponse<List<OrderSummaryDto>>
                {
                    Code = CommonStatusCode.Success,
                    Data = response,
                    Message = "Seller orders retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CommonErrorResponse()
                {
                    Path = "/error",
                    Message = $"An error occurred while retrieving seller orders: {ex.Message}",
                    Status = CommonStatusCode.InternalServerError,
                });
            }
        }

        /// <summary>
        /// Get all orders (admin only)
        /// </summary>
        [HttpGet("GetAllOrders")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<CommonResponse<List<OrderSummaryDto>>>> GetAllOrders(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var orders = await _orderRepository.GetAllAsync(page, pageSize);
                var response = orders.Select(MapToOrderSummaryDto).ToList();

                return Ok(new CommonResponse<List<OrderSummaryDto>>
                {
                    Code = CommonStatusCode.Success,
                    Data = response,
                    Message = "All orders retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CommonErrorResponse()
                {
                    Path = "/error",
                    Message = $"An error occurred while retrieving orders: {ex.Message}",
                    Status = CommonStatusCode.InternalServerError,
                });
            }
        }

        /// <summary>
        /// Update order status (admin only)
        /// </summary>
        [HttpPut("{id}/status")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<CommonResponse<object>>> UpdateOrderStatus(
            string id, [FromBody] OrderStatusUpdateDto request)
        {
            try
            {
                var success = await _orderRepository.UpdateOrderStatusAsync(id, request.Status, request.Notes);
                if (!success)
                {
                    return NotFound(new CommonErrorResponse()
                    {
                        Path = "/error",
                        Message = "Order not found",
                        Status = CommonStatusCode.NotFound,
                    });
                }

                return Ok(new CommonResponse<object>
                {
                    Code = CommonStatusCode.Success,
                    Data = null,
                    Message = "Order status updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CommonErrorResponse()
                {
                    Path = "/error",
                    Message = $"An error occurred while updating order status: {ex.Message}",
                    Status = CommonStatusCode.InternalServerError,
                });
            }
        }

        /// <summary>
        /// Update payment status (admin only)
        /// </summary>
        [HttpPut("{id}/payment-status")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<CommonResponse<object>>> UpdatePaymentStatus(
            string id, [FromBody] OrderPaymentUpdateDto request)
        {
            try
            {
                var success = await _orderRepository.UpdatePaymentStatusAsync(id, request.PaymentStatus, request.Notes);
                if (!success)
                {
                    return NotFound(new CommonErrorResponse()
                    {
                        Path = "/error",
                        Message = "Order not found",
                        Status = CommonStatusCode.NotFound,
                    });
                }

                return Ok(new CommonResponse<object>
                {
                    Code = CommonStatusCode.Success,
                    Data = null,
                    Message = "Payment status updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CommonErrorResponse()
                {
                    Path = "/error",
                    Message = $"An error occurred while updating payment status: {ex.Message}",
                    Status = CommonStatusCode.InternalServerError,
                });
            }
        }

        /// <summary>
        /// Cancel order (buyer only)
        /// </summary>
        [HttpPut("{id}/cancel")]
        public async Task<ActionResult<CommonResponse<object>>> CancelOrder(string id)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound(new CommonErrorResponse()
                    {
                        Path = "/error",
                        Message = "Order not found",
                        Status = CommonStatusCode.NotFound,
                    });
                }

                var loggedInUser = await GetLoggedInUserAsync();
                if (loggedInUser == null)
                {
                    return Unauthorized(new CommonErrorResponse()
                    {
                        Path = "/error",
                        Message = CommonMessages.Unauthorized,
                        Status = CommonStatusCode.Unauthorized,
                    });
                }

                if (order.BuyerId != loggedInUser.Id)
                {
                    return Forbid();
                }

                if (order.Status == OrderStatus.Cancelled)
                {
                    return BadRequest(new CommonErrorResponse()
                    {
                        Path = "/error",
                        Message = "Order is already cancelled",
                        Status = CommonStatusCode.BadRequest,
                    });
                }

                if (order.Status == OrderStatus.Delivered)
                {
                    return BadRequest(new CommonErrorResponse()
                    {
                        Path = "/error",
                        Message = "Cannot cancel a delivered order",
                        Status = CommonStatusCode.BadRequest,
                    });
                }

                var success = await _orderRepository.UpdateOrderStatusAsync(id, OrderStatus.Cancelled, "Cancelled by buyer");
                if (!success)
                {
                    return NotFound(new CommonErrorResponse()
                    {
                        Path = "/error",
                        Message = "Order not found",
                        Status = CommonStatusCode.NotFound,
                    });
                }

                // Restore product stock
                foreach (var item in order.OrderItems)
                {
                    var product = await _productRepository.GetProductById(item.ProductId, loggedInUser);
                    if (product != null)
                    {
                        product.Stock += item.Quantity;
                        var productUpdateDto = new ProductCreateRequestDto
                        {
                            Name = product.Name,
                            Description = product.Description,
                            Price = product.Price,
                            CategoryId = product.CategoryId,
                            Brand = product.Brand,
                            Stock = (int)product.Stock,
                            ImageUrl = product.ImageUrl,
                            Status = (int)product.Status
                        };
                        await _productRepository.UpdateProduct(product.Id, productUpdateDto, loggedInUser);
                    }
                }

                return Ok(new CommonResponse<object>
                {
                    Code = CommonStatusCode.Success,
                    Data = null,
                    Message = "Order cancelled successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CommonErrorResponse()
                {
                    Path = "/error",
                    Message = $"An error occurred while cancelling the order: {ex.Message}",
                    Status = CommonStatusCode.InternalServerError,
                });
            }
        }

        /// <summary>
        /// Get order statistics (admin only)
        /// </summary>
        [HttpGet("statistics")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<CommonResponse<object>>> GetOrderStatistics(
            [FromQuery] string? sellerId = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var totalSales = await _orderRepository.GetTotalSalesAsync(sellerId, startDate, endDate);
                var totalOrders = await _orderRepository.GetOrderCountAsync(sellerId);

                var statistics = new
                {
                    TotalSales = totalSales,
                    TotalOrders = totalOrders,
                    AverageOrderValue = totalOrders > 0 ? totalSales / totalOrders : 0
                };

                return Ok(new CommonResponse<object>
                {
                    Code = CommonStatusCode.Success,
                    Data = statistics,
                    Message = "Order statistics retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CommonErrorResponse()
                {
                    Path = "/error",
                    Message = $"An error occurred while retrieving statistics: {ex.Message}",
                    Status = CommonStatusCode.InternalServerError,
                });
            }
        }

        #region Private Methods

        private static OrderCommonResponseDto MapToOrderResponseDto(Order order)
        {
            return new OrderCommonResponseDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                BuyerId = order.BuyerId,
                BuyerName = order.Buyer?.Name ?? "",
                BuyerEmail = order.Buyer?.Email ?? "",
                TotalAmount = order.TotalAmount,
                TaxAmount = order.TaxAmount,
                ShippingAmount = order.ShippingAmount,
                Status = order.Status,
                PaymentStatus = order.PaymentStatus,
                PaymentMethod = order.PaymentMethod,
                ShippingAddress = order.ShippingAddress,
                BillingAddress = order.BillingAddress,
                Notes = order.Notes,
                ShippedAt = order.ShippedAt,
                DeliveredAt = order.DeliveredAt,
                CancelledAt = order.CancelledAt,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderItems = order.OrderItems?.Select(oi => new OrderItemResponseDto
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    ProductName = oi.ProductName ?? "",
                    ProductImageUrl = oi.ProductImageUrl,
                    ProductDescription = oi.ProductDescription,
                    SellerId = oi.SellerId,
                    SellerName = oi.Seller?.Name ?? "",
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.TotalPrice,
                    CreatedAt = oi.CreatedAt
                }).ToList() ?? new List<OrderItemResponseDto>()
            };
        }

        private static OrderSummaryDto MapToOrderSummaryDto(Order order)
        {
            return new OrderSummaryDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                PaymentStatus = order.PaymentStatus,
                CreatedAt = order.CreatedAt,
                ItemCount = order.OrderItems?.Count ?? 0
            };
        }

        private static bool IsAdmin(eMart.Service.Core.Dtos.User.UserDto user)
        {
            return user.Role?.ToLower() == "admin";
        }

        #endregion
    }
}