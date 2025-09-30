using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Order;
using MediatR;

namespace eMart.Service.Core.Commands.Order
{
    public class CancelOrderCommand : IRequest<CommonResponse<OrderCommonResponseDto>>
    {
        public string OrderId { get; set; } = string.Empty;
        public string CancelledBy { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }
}
