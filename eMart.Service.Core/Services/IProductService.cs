using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Product;

namespace eMart.Service.Core.Services
{
    public interface IProductService
    {
        Task<CommonResponse<ProductCommonResponseDto>> CreateProductAsync(ProductCreateRequestDto productCreateRequest, string userEmail);
        Task<CommonResponse<ProductCommonResponseDto>> UpdateProductAsync(string productId, ProductCreateRequestDto productUpdateRequest, string userEmail);
        Task<CommonResponse<ProductCommonResponseDto>> DeleteProductAsync(string productId, string userEmail);
        Task<CommonResponse<List<ProductCommonResponseDto>>> GetAllProductsAsync(string userEmail);
        Task<CommonResponse<ProductCommonResponseDto>> GetProductByIdAsync(string productId, string userEmail);
        Task<CommonResponse<List<ProductCommonResponseDto>>> GetProductsByCategoryAsync(string categoryId, string userEmail);
    }
}
