using eMart.Service.Core.Commands.Product;
using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Product;
using eMart.Service.Core.Interfaces;
using MediatR;

namespace eMart.Service.Core.Handlers.Product
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, CommonResponse<ProductCommonResponseDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository, IUserRepository userRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<CommonResponse<ProductCommonResponseDto>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
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

                // Delete product
                var deletedProduct = await _productRepository.DeleteProduct(request.ProductId, user);
                if (deletedProduct == null)
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
                    Data = deletedProduct,
                    Message = CommonMessages.ProductRemoved
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<ProductCommonResponseDto>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while deleting the product",
                    Data = null
                };
            }
        }
    }
}
