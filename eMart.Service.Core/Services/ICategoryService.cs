using eMart.Service.Core.Dtos.Category;
using eMart.Service.Core.Dtos.Common;

namespace eMart.Service.Core.Services
{
    public interface ICategoryService
    {
        Task<CommonResponse<CategoryCommonResponseDto>> CreateCategoryAsync(CategoryCreateRequestDto categoryCreateRequest);
        Task<CommonResponse<CategoryCommonResponseDto>> UpdateCategoryAsync(string categoryId, CategoryCreateRequestDto categoryUpdateRequest);
        Task<CommonResponse<CategoryCommonResponseDto>> DeleteCategoryAsync(string categoryId);
        Task<CommonResponse<List<CategoryCommonResponseDto>>> GetAllCategoriesAsync();
        Task<CommonResponse<CategoryCommonResponseDto>> GetCategoryByIdAsync(string categoryId);
    }
}
