using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Product;
using MediatR;

namespace eMart.Service.Core.Commands.Product
{
    public class CreateProductCommand : IRequest<CommonResponse<ProductCommonResponseDto>>
    {
        public ProductCreateRequestDto ProductCreateRequest { get; set; } = null!;
        public string UserId { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
    }
}
