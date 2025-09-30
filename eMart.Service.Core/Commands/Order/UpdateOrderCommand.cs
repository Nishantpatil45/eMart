using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Order;
using MediatR;

namespace eMart.Service.Core.Commands.Order
{
    public class UpdateOrderCommand : IRequest<CommonResponse<OrderCommonResponseDto>>
    {
        public string OrderId { get; set; } = string.Empty;
        public OrderUpdateRequestDto OrderUpdateRequest { get; set; } = null!;
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
