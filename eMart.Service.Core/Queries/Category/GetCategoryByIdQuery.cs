using eMart.Service.Core.Dtos.Category;
using eMart.Service.Core.Dtos.Common;
using MediatR;

namespace eMart.Service.Core.Queries.Category
{
    public class GetCategoryByIdQuery : IRequest<CommonResponse<CategoryCommonResponseDto>>
    {
        public string CategoryId { get; set; } = string.Empty;
    }
}
