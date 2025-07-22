using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMart.Service.DataModels
{
    public class Product
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryId { get; set; }
        public string Brand { get; set; }
        public int Stock { get; set; } = 0;
        public string? ImageUrl { get; set; }
        public string? SellerId { get; set; }
        public int Status { get; set; }
        public decimal? Rating { get; set; } = 0;
        public int? NumReviews { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public string? DeletedBy { get; set; }

        // Navigation Properties
        public User? Seller { get; set; }
        public Category? Category { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Bid>? Bids { get; set; }
        public ICollection<Favorite>? Favorites { get; set; }
        public ICollection<RecentlyViewed>? RecentlyViewedItems { get; set; }
    }
}
