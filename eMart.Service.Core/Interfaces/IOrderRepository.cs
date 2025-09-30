using eMart.Service.Core.Dtos.Order;
using eMart.Service.DataModels;

namespace eMart.Service.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(string id);
        Task<Order?> GetByOrderNumberAsync(string orderNumber);
        Task<List<Order>> GetByBuyerIdAsync(string buyerId, int page = 1, int pageSize = 10);
        Task<List<Order>> GetBySellerIdAsync(string sellerId, int page = 1, int pageSize = 10);
        Task<List<Order>> GetAllAsync(int page = 1, int pageSize = 10);
        Task<List<Order>> GetByStatusAsync(string status, int page = 1, int pageSize = 10);
        Task<List<Order>> GetByPaymentStatusAsync(string paymentStatus, int page = 1, int pageSize = 10);
        Task<Order> CreateAsync(Order order);
        Task<Order> UpdateAsync(Order order);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateOrderStatusAsync(string orderId, string status, string? notes = null);
        Task<bool> UpdatePaymentStatusAsync(string orderId, string paymentStatus, string? notes = null);
        Task<List<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate, int page = 1, int pageSize = 10);
        Task<decimal> GetTotalSalesAsync(string? sellerId = null, DateTime? startDate = null, DateTime? endDate = null);
        Task<int> GetOrderCountAsync(string? sellerId = null, string? status = null);
        Task<bool> ExistsAsync(string id);
        Task<bool> OrderNumberExistsAsync(string orderNumber);
    }
}
