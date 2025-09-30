using eMart.Service.Core.Commands.Category;
using eMart.Service.Core.Dtos.Category;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Queries.Category;
using MediatR;

namespace eMart.Service.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMediator _mediator;

        public CategoryService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CommonResponse<CategoryCommonResponseDto>> CreateCategoryAsync(CategoryCreateRequestDto categoryCreateRequest)
        {
            var command = new CreateCategoryCommand
            {
                CategoryCreateRequest = categoryCreateRequest
            };

            return await _mediator.Send(command);
        }

        public async Task<CommonResponse<CategoryCommonResponseDto>> UpdateCategoryAsync(string categoryId, CategoryCreateRequestDto categoryUpdateRequest)
        {
            var command = new UpdateCategoryCommand
            {
                CategoryId = categoryId,
                CategoryUpdateRequest = categoryUpdateRequest
            };

            return await _mediator.Send(command);
        }

        public async Task<CommonResponse<CategoryCommonResponseDto>> DeleteCategoryAsync(string categoryId)
        {
            var command = new DeleteCategoryCommand
            {
                CategoryId = categoryId
            };

            return await _mediator.Send(command);
        }

        public async Task<CommonResponse<List<CategoryCommonResponseDto>>> GetAllCategoriesAsync()
        {
            var query = new GetAllCategoriesQuery();

            return await _mediator.Send(query);
        }

        public async Task<CommonResponse<CategoryCommonResponseDto>> GetCategoryByIdAsync(string categoryId)
        {
            var query = new GetCategoryByIdQuery
            {
                CategoryId = categoryId
            };

            return await _mediator.Send(query);
        }
    }
}
