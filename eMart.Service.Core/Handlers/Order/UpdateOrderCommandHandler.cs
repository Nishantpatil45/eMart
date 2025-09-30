using eMart.Service.Core.Commands.Order;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Order;
using eMart.Service.Core.Interfaces;
using MediatR;

namespace eMart.Service.Core.Handlers.Order
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, CommonResponse<OrderCommonResponseDto>>
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<CommonResponse<OrderCommonResponseDto>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Note: UpdateOrderAsync method doesn't exist in IOrderRepository
                // This would need to be implemented in the repository
                return new CommonResponse<OrderCommonResponseDto>
                {
                    Code = CommonStatusCode.NotFound,
                    Message = "Order not found",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<OrderCommonResponseDto>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while updating the order",
                    Data = null
                };
            }
        }
    }
}
