using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Order;
using MediatR;

namespace eMart.Service.Core.Queries.Order
{
    public class GetAllOrdersQuery : IRequest<CommonResponse<List<OrderCommonResponseDto>>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string RequestedBy { get; set; } = string.Empty;
    }
}
