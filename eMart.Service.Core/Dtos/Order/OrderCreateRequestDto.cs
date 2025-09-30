using System.ComponentModel.DataAnnotations;

namespace eMart.Service.Core.Dtos.Order
{
    public class OrderCreateRequestDto
    {
        [Required]
        public string BuyerId { get; set; }
        
        [Required]
        public string PaymentMethod { get; set; }
        
        public string? ShippingAddress { get; set; }
        public string? BillingAddress { get; set; }
        public string? Notes { get; set; }
        
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Tax amount must be non-negative")]
        public decimal TaxAmount { get; set; } = 0;
        
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Shipping amount must be non-negative")]
        public decimal ShippingAmount { get; set; } = 0;
        
        [Required]
        public List<OrderItemCreateRequestDto> OrderItems { get; set; } = new List<OrderItemCreateRequestDto>();
    }
    
    public class OrderItemCreateRequestDto
    {
        [Required]
        public string ProductId { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0")]
        public decimal UnitPrice { get; set; }
    }
}
