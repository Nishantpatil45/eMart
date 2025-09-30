using eMart.Service.Core.Commands.Category;
using eMart.Service.Core.Dtos.Category;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Interfaces;
using MediatR;

namespace eMart.Service.Core.Handlers.Category
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CommonResponse<CategoryCommonResponseDto>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CommonResponse<CategoryCommonResponseDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newCategory = await _categoryRepository.CreateCategory(request.CategoryCreateRequest);
                if (newCategory == null)
                {
                    return new CommonResponse<CategoryCommonResponseDto>
                    {
                        Code = CommonStatusCode.Conflict,
                        Message = CommonMessages.CategoryIsAlredyExits,
                        Data = null
                    };
                }

                return new CommonResponse<CategoryCommonResponseDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = newCategory,
                    Message = CommonMessages.CategoryAddedSuccessfully
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<CategoryCommonResponseDto>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while creating the category",
                    Data = null
                };
            }
        }
    }
}
