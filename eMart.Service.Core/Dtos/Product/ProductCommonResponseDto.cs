using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMart.Service.Core.Dtos.Product
{
    public class ProductCommonResponseDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? CategoryId { get; set; }
        public string? Brand { get; set; }
        public int? Stock { get; set; } = 0;
        public string? ImageUrl { get; set; }
        public string? SellerId { get; set; }
        public int? Status { get; set; }
        public decimal? Rating { get; set; }
        public int? NumReviews { get; set; }
        public bool? IsFavorite { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
    }
}
