using eMart.Service.Core.Commands.Category;
using eMart.Service.Core.Dtos.Category;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Interfaces;
using MediatR;

namespace eMart.Service.Core.Handlers.Category
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, CommonResponse<CategoryCommonResponseDto>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CommonResponse<CategoryCommonResponseDto>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Note: DeleteCategory method doesn't exist in ICategoryRepository
                // This would need to be implemented in the repository
                return new CommonResponse<CategoryCommonResponseDto>
                {
                    Code = CommonStatusCode.NotFound,
                    Message = CommonMessages.CategoryNotFound,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<CategoryCommonResponseDto>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while deleting the category",
                    Data = null
                };
            }
        }
    }
}
