using eMart.Service.Core.Dtos.Product;
using eMart.Service.Core.Interfaces;
using eMart.Service.DataModels;
using eMart.Service.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMart.Service.Core.Repositories
{
    public class RecentlyViewedRepository : IRecentlyViewedRepository
    {
        private readonly eMartDbContext dbContext;
        public RecentlyViewedRepository(eMartDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task AddRecentlyViewed(string productId, string userId)
        {
            if (string.IsNullOrWhiteSpace(productId) || string.IsNullOrWhiteSpace(userId))
                return;

            // Check if the product is already in RecentlyViewed
            var existingEntry = await dbContext.RecentlyViewedItems
                .FirstOrDefaultAsync(rv => rv.UserId == userId && rv.ProductId == productId);

            if (existingEntry != null)
            {
                // Update the timestamp
                existingEntry.ViewedAt = DateTime.UtcNow;
            }
            else
            {
                // Add a new entry
                var recentlyViewed = new RecentlyViewed
                {
                    UserId = userId,
                    ProductId = productId,
                    ViewedAt = DateTime.UtcNow
                };
                await dbContext.RecentlyViewedItems.AddAsync(recentlyViewed);
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task<List<ProductCommonResponseDto>> GetRecentlyViewedProducts(string userId, int limit = 10)
        {
            var products = await dbContext.RecentlyViewedItems
                .Where(rv => rv.UserId == userId)
                .OrderByDescending(rv => rv.ViewedAt)
                .Take(limit)
                .Select(rv => rv.Product)
                .ToListAsync();
            return products.Select(product => new ProductCommonResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Brand = product.Brand,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                SellerId = product.SellerId,
                Status = product.Status,
                Rating = product.Rating,
                NumReviews = product.NumReviews,
                CreatedAt = product.CreatedAt,
                CreatedBy = product.CreatedBy,
                UpdatedAt = product.UpdatedAt,
                UpdatedBy = product.UpdatedBy,
                IsDeleted = product.IsDeleted,
                DeletedBy = product.DeletedBy
            }).ToList();
        }
    }
}
