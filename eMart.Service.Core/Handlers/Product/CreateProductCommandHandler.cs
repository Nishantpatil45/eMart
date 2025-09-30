using eMart.Service.Core.Commands.Product;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Product;
using eMart.Service.Core.Interfaces;
using MediatR;

namespace eMart.Service.Core.Handlers.Product
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CommonResponse<ProductCommonResponseDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public CreateProductCommandHandler(IProductRepository productRepository, IUserRepository userRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<CommonResponse<ProductCommonResponseDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get user details
                var user = await _userRepository.GetUserDetails(request.UserEmail);
                if (user == null)
                {
                    return new CommonResponse<ProductCommonResponseDto>
                    {
                        Code = CommonStatusCode.Unauthorized,
                        Message = "User not found",
                        Data = null
                    };
                }

                // Create product
                var newProduct = await _productRepository.CreateProduct(request.ProductCreateRequest, user);
                if (newProduct == null)
                {
                    return new CommonResponse<ProductCommonResponseDto>
                    {
                        Code = CommonStatusCode.BadRequest,
                        Message = "Failed to create product",
                        Data = null
                    };
                }

                return new CommonResponse<ProductCommonResponseDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = newProduct,
                    Message = CommonMessages.ProductAddedSuccessfully
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<ProductCommonResponseDto>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while creating the product",
                    Data = null
                };
            }
        }
    }
}
