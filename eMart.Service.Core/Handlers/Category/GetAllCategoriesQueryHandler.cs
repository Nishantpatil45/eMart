using eMart.Service.Core.Dtos.Category;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Interfaces;
using eMart.Service.Core.Queries.Category;
using MediatR;

namespace eMart.Service.Core.Handlers.Category
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, CommonResponse<List<CategoryCommonResponseDto>>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CommonResponse<List<CategoryCommonResponseDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var categories = await _categoryRepository.GetAllCategories();

                return new CommonResponse<List<CategoryCommonResponseDto>>
                {
                    Code = CommonStatusCode.Success,
                    Data = categories,
                    Message = CommonMessages.CategoriesFound
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<List<CategoryCommonResponseDto>>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while retrieving categories",
                    Data = new List<CategoryCommonResponseDto>()
                };
            }
        }
    }
}
