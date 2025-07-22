using eMart.Service.Core.Dtos.Category;
using eMart.Service.Core.Dtos.Product;
using eMart.Service.Core.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMart.Service.Core.Interfaces
{
    public interface ICategoryRepository
    {
        CategoryCommonResponseDto CreateCategory(CategoryCreateRequestDto categoryCreateRequestDto);

        Task<List<CategoryCommonResponseDto>> GetAllCategories();

        Task<CategoryCommonResponseDto> GetCategoryById(string id);

        Task<CategoryCommonResponseDto> UpdateCategory(string id, CategoryCreateRequestDto categoryCreateRequestDto, UserDto userDto);
    }
}
