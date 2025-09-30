using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Product;
using MediatR;

namespace eMart.Service.Core.Queries.Product
{
    public class GetAllProductsQuery : IRequest<CommonResponse<List<ProductCommonResponseDto>>>
    {
        public string UserId { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
    }
}
