using eMart.Service.Core.Commands.Order;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Order;
using eMart.Service.Core.Interfaces;
using MediatR;

namespace eMart.Service.Core.Handlers.Order
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, CommonResponse<OrderCommonResponseDto>>
    {
        private readonly IOrderRepository _orderRepository;

        public CancelOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<CommonResponse<OrderCommonResponseDto>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Note: CancelOrderAsync method doesn't exist in IOrderRepository
                // This would need to be implemented in the repository
                return new CommonResponse<OrderCommonResponseDto>
                {
                    Code = CommonStatusCode.NotFound,
                    Message = "Order not found or cannot be cancelled",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<OrderCommonResponseDto>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while cancelling the order",
                    Data = null
                };
            }
        }
    }
}
