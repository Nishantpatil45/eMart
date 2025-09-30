using eMart.Service.Core.Dtos.Category;
using eMart.Service.Core.Dtos.Common;
using MediatR;

namespace eMart.Service.Core.Queries.Category
{
    public class GetAllCategoriesQuery : IRequest<CommonResponse<List<CategoryCommonResponseDto>>>
    {
    }
}
