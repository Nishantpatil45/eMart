using eMart.Service.Core.Dtos.Category;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.User;
using eMart.Service.Core.Interfaces;
using eMart.Service.DataModels;
using eMart.Service.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eMart.Service.Core.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly eMartDbContext dbContext;
        public CategoryRepository(eMartDbContext _dbContext) 
        {
            dbContext = _dbContext;
        }
        public async Task<CategoryCommonResponseDto> CreateCategory(CategoryCreateRequestDto categoryCreateRequestDto)
        {
            var categoryExits = await dbContext.Categorys.FirstOrDefaultAsync(x => x.Name == categoryCreateRequestDto.Name && (x.IsDeleted == null || x.IsDeleted == false));
            if (categoryExits == null)
            {
                var Category = new Category
                {
                    Name = categoryCreateRequestDto.Name
                };
                dbContext.Categorys.Add(Category);
                await dbContext.SaveChangesAsync();
                return new CategoryCommonResponseDto
                {
                    Id = Category.Id,
                    CategoryName = categoryCreateRequestDto.Name
                };
            }
            else
            {
                return null;
            }
        }

        public async Task<List<CategoryCommonResponseDto>> GetAllCategories()
        {
            var categories = await dbContext.Categorys.Where(x => x.IsDeleted == null || x.IsDeleted == false).ToListAsync();
            return categories.Select(category => new CategoryCommonResponseDto
            {
                Id = category.Id,
                CategoryName = category.Name
            }).ToList();
        }

        public async Task<CategoryCommonResponseDto> GetCategoryById(string id)
        {
            var category = await dbContext.Categorys.FirstOrDefaultAsync(x => x.Id == id && (x.IsDeleted == null || x.IsDeleted == false));
            if (category == null)
            {
                // Do not throw raw exception, return null
                return null;
            }
            var categoryDto = new CategoryCommonResponseDto
            {
                Id = category.Id,
                CategoryName = category.Name
            };
            return categoryDto;
        }

        public async Task<CategoryCommonResponseDto> UpdateCategory(string id, CategoryCreateRequestDto categoryCreateRequestDto, UserDto userDto)
        {
            var category = await dbContext.Categorys.FirstOrDefaultAsync(x => x.Id == id && (x.IsDeleted == null || x.IsDeleted == false));
            if (category == null)
            {
                return null;
            }
            category.Name = categoryCreateRequestDto.Name;
            await dbContext.SaveChangesAsync();
            return new CategoryCommonResponseDto()
            {
                Id = category.Id,
                CategoryName = categoryCreateRequestDto.Name
            };
        }
    }
}
