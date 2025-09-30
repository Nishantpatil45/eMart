namespace eMart.Service.Core.Dtos.Enums
{
    public enum OrderStatusEnum
    {
        Pending = 0,
        Confirmed = 1,
        Processing = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5,
        Refunded = 6
    }
    
    public enum PaymentStatusEnum
    {
        Pending = 0,
        Paid = 1,
        Failed = 2,
        Refunded = 3,
        PartiallyRefunded = 4
    }
}
