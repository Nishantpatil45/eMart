using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Order;
using MediatR;

namespace eMart.Service.Core.Commands.Order
{
    public class CreateOrderCommand : IRequest<CommonResponse<OrderCommonResponseDto>>
    {
        public OrderCreateRequestDto OrderCreateRequest { get; set; } = null!;
        public string BuyerId { get; set; } = string.Empty;
        public string BuyerEmail { get; set; } = string.Empty;
    }
}
