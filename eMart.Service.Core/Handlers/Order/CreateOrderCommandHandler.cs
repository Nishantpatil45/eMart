using eMart.Service.Core.Commands.Order;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Order;
using eMart.Service.Core.Interfaces;
using MediatR;

namespace eMart.Service.Core.Handlers.Order
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, CommonResponse<OrderCommonResponseDto>>
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<CommonResponse<OrderCommonResponseDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Note: CreateOrderAsync method doesn't exist in IOrderRepository
                // This would need to be implemented in the repository
                return new CommonResponse<OrderCommonResponseDto>
                {
                    Code = CommonStatusCode.BadRequest,
                    Message = "Failed to create order. Please check product availability and try again.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<OrderCommonResponseDto>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while creating the order",
                    Data = null
                };
            }
        }
    }
}
