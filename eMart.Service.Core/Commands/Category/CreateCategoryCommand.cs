using eMart.Service.Core.Dtos.Category;
using eMart.Service.Core.Dtos.Common;
using MediatR;

namespace eMart.Service.Core.Commands.Category
{
    public class CreateCategoryCommand : IRequest<CommonResponse<CategoryCommonResponseDto>>
    {
        public CategoryCreateRequestDto CategoryCreateRequest { get; set; } = null!;
    }
}
