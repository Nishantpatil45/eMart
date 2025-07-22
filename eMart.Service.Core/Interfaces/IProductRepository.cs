using eMart.Service.Core.Dtos.Product;
using eMart.Service.Core.Dtos.User;
using System.Globalization;

namespace eMart.Service.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductCommonResponseDto> CreateProduct(ProductCreateRequestDto productCreateRequestDto, UserDto userDto);

        Task<List<ProductCommonResponseDto>> GetProduct(UserDto userDto);

        Task<ProductCommonResponseDto> GetProductById(String id, UserDto userDto);

        Task<List<ProductCommonResponseDto>> GetProductsByCategoryId(string id, UserDto userDto);

        Task<ProductCommonResponseDto> UpdateProduct(string id, ProductCreateRequestDto productCreateRequestDto, UserDto userDto);

        Task<ProductCommonResponseDto> DeleteProduct(string id, UserDto userDto);
    }
}
