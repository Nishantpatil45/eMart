using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Order;
using MediatR;

namespace eMart.Service.Core.Queries.Order
{
    public class GetOrderByIdQuery : IRequest<CommonResponse<OrderCommonResponseDto>>
    {
        public string OrderId { get; set; } = string.Empty;
        public string RequestedBy { get; set; } = string.Empty;
    }
}
