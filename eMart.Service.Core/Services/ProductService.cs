using eMart.Service.Core.Commands.Product;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Product;
using eMart.Service.Core.Queries.Product;
using MediatR;

namespace eMart.Service.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IMediator _mediator;

        public ProductService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CommonResponse<ProductCommonResponseDto>> CreateProductAsync(ProductCreateRequestDto productCreateRequest, string userEmail)
        {
            var command = new CreateProductCommand
            {
                ProductCreateRequest = productCreateRequest,
                UserEmail = userEmail
            };

            return await _mediator.Send(command);
        }

        public async Task<CommonResponse<ProductCommonResponseDto>> UpdateProductAsync(string productId, ProductCreateRequestDto productUpdateRequest, string userEmail)
        {
            var command = new UpdateProductCommand
            {
                ProductId = productId,
                ProductUpdateRequest = productUpdateRequest,
                UserEmail = userEmail
            };

            return await _mediator.Send(command);
        }

        public async Task<CommonResponse<ProductCommonResponseDto>> DeleteProductAsync(string productId, string userEmail)
        {
            var command = new DeleteProductCommand
            {
                ProductId = productId,
                UserEmail = userEmail
            };

            return await _mediator.Send(command);
        }

        public async Task<CommonResponse<List<ProductCommonResponseDto>>> GetAllProductsAsync(string userEmail)
        {
            var query = new GetAllProductsQuery
            {
                UserEmail = userEmail
            };

            return await _mediator.Send(query);
        }

        public async Task<CommonResponse<ProductCommonResponseDto>> GetProductByIdAsync(string productId, string userEmail)
        {
            var query = new GetProductByIdQuery
            {
                ProductId = productId,
                UserEmail = userEmail
            };

            return await _mediator.Send(query);
        }

        public async Task<CommonResponse<List<ProductCommonResponseDto>>> GetProductsByCategoryAsync(string categoryId, string userEmail)
        {
            var query = new GetProductsByCategoryQuery
            {
                CategoryId = categoryId,
                UserEmail = userEmail
            };

            return await _mediator.Send(query);
        }
    }
}
