using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMart.Service.DataModels
{
    public class OrderItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string OrderId { get; set; }
        
        [Required]
        public string ProductId { get; set; }
        
        [Required]
        public string SellerId { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        
        public string? ProductName { get; set; } // Store product name at time of order
        public string? ProductImageUrl { get; set; } // Store product image at time of order
        public string? ProductDescription { get; set; } // Store product description at time of order
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public string? DeletedBy { get; set; }

        // Navigation Properties
        public Order Order { get; set; }
        public Product Product { get; set; }
        public User Seller { get; set; }
    }
}
