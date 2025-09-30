using eMart.Service.Core.Dtos.Category;
using eMart.Service.Core.Dtos.Common;
using MediatR;

namespace eMart.Service.Core.Commands.Category
{
    public class UpdateCategoryCommand : IRequest<CommonResponse<CategoryCommonResponseDto>>
    {
        public string CategoryId { get; set; } = string.Empty;
        public CategoryCreateRequestDto CategoryUpdateRequest { get; set; } = null!;
    }
}
