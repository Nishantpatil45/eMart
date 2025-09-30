using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Order;
using eMart.Service.Core.Interfaces;
using eMart.Service.Core.Queries.Order;
using MediatR;

namespace eMart.Service.Core.Handlers.Order
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, CommonResponse<OrderCommonResponseDto>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<CommonResponse<OrderCommonResponseDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(request.OrderId);
                if (order == null)
                {
                    return new CommonResponse<OrderCommonResponseDto>
                    {
                        Code = CommonStatusCode.NotFound,
                        Message = "Order not found",
                        Data = null
                    };
                }

                // Convert Order entity to OrderCommonResponseDto
                var orderResponse = new OrderCommonResponseDto
                {
                    Id = order.Id,
                    OrderNumber = order.OrderNumber,
                    BuyerId = order.BuyerId,
                    BuyerName = string.Empty, // Order entity doesn't have BuyerName
                    BuyerEmail = string.Empty, // Order entity doesn't have BuyerEmail
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
                        ProductName = string.Empty, // OrderItem doesn't have ProductName
                        ProductImageUrl = null, // OrderItem doesn't have ProductImageUrl
                        ProductDescription = null, // OrderItem doesn't have ProductDescription
                        SellerId = oi.SellerId,
                        SellerName = string.Empty, // OrderItem doesn't have SellerName
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        TotalPrice = oi.TotalPrice,
                        CreatedAt = oi.CreatedAt
                    }).ToList() ?? new List<OrderItemResponseDto>()
                };

                return new CommonResponse<OrderCommonResponseDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = orderResponse,
                    Message = "Order retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<OrderCommonResponseDto>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while retrieving the order",
                    Data = null
                };
            }
        }
    }
}
