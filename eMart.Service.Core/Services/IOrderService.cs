using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Order;

namespace eMart.Service.Core.Services
{
    public interface IOrderService
    {
        Task<CommonResponse<OrderCommonResponseDto>> CreateOrderAsync(OrderCreateRequestDto orderCreateRequest, string buyerId, string buyerEmail);
        Task<CommonResponse<OrderCommonResponseDto>> UpdateOrderAsync(string orderId, OrderUpdateRequestDto orderUpdateRequest, string updatedBy);
        Task<CommonResponse<OrderCommonResponseDto>> CancelOrderAsync(string orderId, string cancelledBy, string reason);
        Task<CommonResponse<OrderCommonResponseDto>> GetOrderByIdAsync(string orderId, string requestedBy);
        Task<CommonResponse<List<OrderCommonResponseDto>>> GetOrdersByBuyerAsync(string buyerId, int page, int pageSize, string requestedBy);
        Task<CommonResponse<List<OrderCommonResponseDto>>> GetAllOrdersAsync(int page, int pageSize, string requestedBy);
    }
}
