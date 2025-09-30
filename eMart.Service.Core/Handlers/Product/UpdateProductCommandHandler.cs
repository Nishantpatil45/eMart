using eMart.Service.Core.Commands.Product;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Product;
using eMart.Service.Core.Interfaces;
using MediatR;

namespace eMart.Service.Core.Handlers.Product
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, CommonResponse<ProductCommonResponseDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository, IUserRepository userRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<CommonResponse<ProductCommonResponseDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
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

                // Update product
                var updatedProduct = await _productRepository.UpdateProduct(request.ProductId, request.ProductUpdateRequest, user);
                if (updatedProduct == null)
                {
                    return new CommonResponse<ProductCommonResponseDto>
                    {
                        Code = CommonStatusCode.NotFound,
                        Message = CommonMessages.ProductNotFound,
                        Data = null
                    };
                }

                return new CommonResponse<ProductCommonResponseDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = updatedProduct,
                    Message = CommonMessages.ProductEditSuccessfully
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<ProductCommonResponseDto>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while updating the product",
                    Data = null
                };
            }
        }
    }
}
