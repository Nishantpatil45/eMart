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
        public CategoryCommonResponseDto CreateCategory(CategoryCreateRequestDto categoryCreateRequestDto)
        {
            var categoryExits = dbContext.Categorys.FirstOrDefault(x => x.Name == categoryCreateRequestDto.Name);

            if (categoryExits == null)
            {
                var Category = new Category
                {
                    Name = categoryCreateRequestDto.Name
                };

                // Save to database
                dbContext.Categorys.Add(Category);
                dbContext.SaveChanges();
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
            var categories = dbContext.Categorys.ToList();

            if (categories.Any())
            {
                return categories.Select(category => new CategoryCommonResponseDto
                {
                    Id = category.Id,
                    CategoryName = category.Name
                }).ToList();
            }
            else
            {
                return null;
            }
        }

        public async Task<CategoryCommonResponseDto> GetCategoryById(string id)
        {
            var category = await dbContext.Categorys.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                throw new Exception(CommonMessages.CategoryNotFound);
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
            var category = await dbContext.Categorys.FirstOrDefaultAsync(x => x.Id == id);

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
