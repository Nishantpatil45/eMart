using eMart.Service.Core.Dtos.Product;
using eMart.Service.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMart.Service.Core.Interfaces
{
    public interface IRecentlyViewedRepository
    {
        Task AddRecentlyViewed(string productId, string userId);

        Task<List<ProductCommonResponseDto>> GetRecentlyViewedProducts(string userId, int limit = 10);
    }
}
