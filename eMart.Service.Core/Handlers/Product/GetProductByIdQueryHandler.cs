using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Product;
using eMart.Service.Core.Interfaces;
using eMart.Service.Core.Queries.Product;
using MediatR;

namespace eMart.Service.Core.Handlers.Product
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, CommonResponse<ProductCommonResponseDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRecentlyViewedRepository _recentlyViewedRepository;

        public GetProductByIdQueryHandler(
            IProductRepository productRepository, 
            IUserRepository userRepository,
            IRecentlyViewedRepository recentlyViewedRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            _recentlyViewedRepository = recentlyViewedRepository;
        }

        public async Task<CommonResponse<ProductCommonResponseDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
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

                // Get product by ID
                var product = await _productRepository.GetProductById(request.ProductId, user);
                if (product == null)
                {
                    return new CommonResponse<ProductCommonResponseDto>
                    {
                        Code = CommonStatusCode.NotFound,
                        Message = CommonMessages.ProductNotFound,
                        Data = null
                    };
                }

                // Track recently viewed product
                await _recentlyViewedRepository.AddRecentlyViewed(request.ProductId, user.Id);

                return new CommonResponse<ProductCommonResponseDto>
                {
                    Code = CommonStatusCode.Success,
                    Data = product,
                    Message = CommonMessages.ProductFound
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<ProductCommonResponseDto>
                {
                    Code = CommonStatusCode.InternalServerError,
                    Message = "An unexpected error occurred while retrieving the product",
                    Data = null
                };
            }
        }
    }
}
