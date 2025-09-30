using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Product;
using eMart.Service.Core.Interfaces;
using eMart.Service.Core.Queries.Product;
using MediatR;

namespace eMart.Service.Core.Handlers.Product
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, CommonResponse<List<ProductCommonResponseDto>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public GetAllProductsQueryHandler(IProductRepository productRepository, IUserRepository userRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<CommonResponse<List<ProductCommonResponseDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get user details
                var user = await _userRepository.GetUserDetails(request.UserEmail);
                if (user == null)
                {
                    return new CommonResponse<List<ProductCommonResponseDto>>
                    {
                        Code = CommonStatusCode.Unauthorized,
                        Message = "User not found",
                        Data = new List<ProductCommonResponseDto>()
                    };
                }

                // Get all products
                var products = await _productRepository.GetProduct(user);

                return new CommonResponse<List<ProductCommonResponseDto>>
                {
                    Code = CommonStatusCode.Success,
                    Data = products,
                    Message = CommonMessages.ProductFound
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<List<ProductCommonResponseDto>>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while retrieving products",
                    Data = new List<ProductCommonResponseDto>()
                };
            }
        }
    }
}
