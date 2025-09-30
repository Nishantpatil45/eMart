using System.ComponentModel.DataAnnotations;

namespace eMart.Service.Core.Dtos.Order
{
    public class OrderUpdateRequestDto
    {
        public string? Status { get; set; }
        public string? PaymentStatus { get; set; }
        public string? ShippingAddress { get; set; }
        public string? BillingAddress { get; set; }
        public string? Notes { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? CancelledAt { get; set; }
    }
    
    public class OrderStatusUpdateDto
    {
        [Required]
        public string Status { get; set; }
        
        public string? Notes { get; set; }
    }
    
    public class OrderPaymentUpdateDto
    {
        [Required]
        public string PaymentStatus { get; set; }
        
        public string? Notes { get; set; }
    }
}
