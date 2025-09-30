using eMart.Service.Core.Commands.Order;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Order;
using eMart.Service.Core.Queries.Order;
using MediatR;

namespace eMart.Service.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMediator _mediator;

        public OrderService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CommonResponse<OrderCommonResponseDto>> CreateOrderAsync(OrderCreateRequestDto orderCreateRequest, string buyerId, string buyerEmail)
        {
            var command = new CreateOrderCommand
            {
                OrderCreateRequest = orderCreateRequest,
                BuyerId = buyerId,
                BuyerEmail = buyerEmail
            };

            return await _mediator.Send(command);
        }

        public async Task<CommonResponse<OrderCommonResponseDto>> UpdateOrderAsync(string orderId, OrderUpdateRequestDto orderUpdateRequest, string updatedBy)
        {
            var command = new UpdateOrderCommand
            {
                OrderId = orderId,
                OrderUpdateRequest = orderUpdateRequest,
                UpdatedBy = updatedBy
            };

            return await _mediator.Send(command);
        }

        public async Task<CommonResponse<OrderCommonResponseDto>> CancelOrderAsync(string orderId, string cancelledBy, string reason)
        {
            var command = new CancelOrderCommand
            {
                OrderId = orderId,
                CancelledBy = cancelledBy,
                Reason = reason
            };

            return await _mediator.Send(command);
        }

        public async Task<CommonResponse<OrderCommonResponseDto>> GetOrderByIdAsync(string orderId, string requestedBy)
        {
            var query = new GetOrderByIdQuery
            {
                OrderId = orderId,
                RequestedBy = requestedBy
            };

            return await _mediator.Send(query);
        }

        public async Task<CommonResponse<List<OrderCommonResponseDto>>> GetOrdersByBuyerAsync(string buyerId, int page, int pageSize, string requestedBy)
        {
            var query = new GetOrdersByBuyerQuery
            {
                BuyerId = buyerId,
                Page = page,
                PageSize = pageSize,
                RequestedBy = requestedBy
            };

            return await _mediator.Send(query);
        }

        public async Task<CommonResponse<List<OrderCommonResponseDto>>> GetAllOrdersAsync(int page, int pageSize, string requestedBy)
        {
            var query = new GetAllOrdersQuery
            {
                Page = page,
                PageSize = pageSize,
                RequestedBy = requestedBy
            };

            return await _mediator.Send(query);
        }
    }
}
