using eMart.Service.Core.Commands.Category;
using eMart.Service.Core.Dtos.Category;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Interfaces;
using MediatR;

namespace eMart.Service.Core.Handlers.Category
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CommonResponse<CategoryCommonResponseDto>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CommonResponse<CategoryCommonResponseDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Note: UpdateCategory requires UserDto parameter which is not available in the command
                // This would need to be implemented properly in the repository
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
                    Message = "An unexpected error occurred while updating the category",
                    Data = null
                };
            }
        }
    }
}
