using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Product;
using MediatR;

namespace eMart.Service.Core.Commands.Product
{
    public class DeleteProductCommand : IRequest<CommonResponse<ProductCommonResponseDto>>
    {
        public string ProductId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
    }
}
