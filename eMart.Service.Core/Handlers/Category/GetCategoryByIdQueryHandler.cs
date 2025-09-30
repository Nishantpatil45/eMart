using eMart.Service.Core.Dtos.Category;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Interfaces;
using eMart.Service.Core.Queries.Category;
using MediatR;

namespace eMart.Service.Core.Handlers.Category
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CommonResponse<CategoryCommonResponseDto>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CommonResponse<CategoryCommonResponseDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryById(request.CategoryId);
                if (category == null)
                {
                    return new CommonResponse<CategoryCommonResponseDto>
                    {
                        Code = CommonStatusCode.NotFound,
                        Message = CommonMessages.CategoryNotFound,
                        Data = null
                    };
                }

                return new CommonResponse<CategoryCommonResponseDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = category,
                    Message = CommonMessages.CategoryFound
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<CategoryCommonResponseDto>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while retrieving the category",
                    Data = null
                };
            }
        }
    }
}
