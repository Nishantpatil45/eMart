using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Product;
using MediatR;

namespace eMart.Service.Core.Queries.Product
{
    public class GetProductsByCategoryQuery : IRequest<CommonResponse<List<ProductCommonResponseDto>>>
    {
        public string CategoryId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
    }
}
