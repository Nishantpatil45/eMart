using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMart.Service.DataModels
{
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string BuyerId { get; set; }
        
        [Required]
        public string OrderNumber { get; set; } = GenerateOrderNumber();
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; } = 0;
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingAmount { get; set; } = 0;
        
        [Required]
        public string Status { get; set; } = OrderStatus.Pending;
        
        [Required]
        public string PaymentStatus { get; set; } = PaymentStatusValues.Pending;
        
        [Required]
        public string PaymentMethod { get; set; }
        
        public string? ShippingAddress { get; set; }
        public string? BillingAddress { get; set; }
        public string? Notes { get; set; }
        
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public string? DeletedBy { get; set; }

        // Navigation Properties
        public User Buyer { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        
        private static string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        }
    }
    
    public static class OrderStatus
    {
        public const string Pending = "pending";
        public const string Confirmed = "confirmed";
        public const string Processing = "processing";
        public const string Shipped = "shipped";
        public const string Delivered = "delivered";
        public const string Cancelled = "cancelled";
        public const string Refunded = "refunded";
    }
    
    public static class PaymentStatusValues
    {
        public const string Pending = "pending";
        public const string Paid = "paid";
        public const string Failed = "failed";
        public const string Refunded = "refunded";
        public const string PartiallyRefunded = "partially_refunded";
    }
}
