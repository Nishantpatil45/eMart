using eMart.Service.Core.Dtos.Category;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Product;
using eMart.Service.Core.Interfaces;
using eMart.Service.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.VisualBasic;

namespace eMart.Service.Api.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    [Authorize]
    public class CategoryController : BaseController
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository, IUserRepository userRepository) : base(userRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpPost("AddCategory")]
        public ActionResult AddCategory(CategoryCreateRequestDto categoryCreateRequestDto)
        {
            try
            {
                var newCategory = _categoryRepository.CreateCategory(categoryCreateRequestDto);

                return Ok(new CommonResponse<CategoryCommonResponseDto>()
                {
                    Code = CommonStatusCode.Success,
                    Data = newCategory,
                    Message = CommonMessages.CategoryAddedSuccessfully
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new CommonErrorResponse()
                {
                    Path = "/error",
                    Status = CommonStatusCode.BadRequest,
                    Message = ex.Message,
                });
            }
        }

        [HttpGet("Category/list")]
        public async Task<ActionResult> GetAllCategories()
        {
            try
            {
                var loggedInUser = await this.GetLoggedInUserAsync();
                if (loggedInUser == null)
                {
                    return Unauthorized();
                }

                var categories = await _categoryRepository.GetAllCategories();

                return Ok(new CommonResponse<List<CategoryCommonResponseDto>>()
                {
                    Code = CommonStatusCode.Success,
                    Data = categories,
                    Message = CommonMessages.CategoryFound
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new CommonErrorResponse()
                {
                    Path = "/error",
                    Status = CommonStatusCode.BadRequest,
                    Message = ex.Message,
                });
            }
        }

        [HttpGet("Category/{id}")]
        public async Task<ActionResult> GetCategoryById(string id)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryById(id);
                return Ok(new CommonResponse<CategoryCommonResponseDto>()
                {
                    Code = CommonStatusCode.Success,
                    Data = category,
                    Message = CommonMessages.CategoryFound
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new CommonErrorResponse()
                {
                    Path = "/error",
                    Status = CommonStatusCode.BadRequest,
                    Message = ex.Message,
                });
            }
        }

        [HttpPut("Category/{id}")]
        public async Task<ActionResult> UpdateCategory(string id, CategoryCreateRequestDto categoryCreateRequestDto)
        {
            try
            {
                var loggedInUser = await this.GetLoggedInUserAsync();
                if (loggedInUser == null)
                {
                    return Unauthorized();
                }

                var category = await _categoryRepository.UpdateCategory(id, categoryCreateRequestDto, loggedInUser);

                if (category == null)
                {
                    return NotFound(new Core.Dtos.Common.CommonErrorResponse()
                    {
                        Path = "/error",
                        Status = CommonStatusCode.NotFound,
                        Message = CommonMessages.CategoryNotFound
                    });
                }

                return Ok(new CommonResponse<CategoryCommonResponseDto>()
                {
                    Code = CommonStatusCode.Success,
                    Data = category,
                    Message = CommonMessages.CategoryEditSuccessfully
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new CommonErrorResponse()
                {
                    Path = "/error",
                    Status = CommonStatusCode.BadRequest,
                    Message = ex.Message,
                });
            }
        }
    }
}
