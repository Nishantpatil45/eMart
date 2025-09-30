using eMart.Service.Core.Interfaces;
using eMart.Service.DataModels;
using eMart.Service.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eMart.Service.Core.Repositories
{
    public class OrderRepository : RepositoryBase<OrderRepository>, IOrderRepository
    {
        public OrderRepository(eMartDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Order?> GetByIdAsync(string id)
        {
            return await dbContext.Orders
                .Include(o => o.Buyer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Seller)
                .FirstOrDefaultAsync(o => o.Id == id && o.IsDeleted != true);
        }

        public async Task<Order?> GetByOrderNumberAsync(string orderNumber)
        {
            return await dbContext.Orders
                .Include(o => o.Buyer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Seller)
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber && o.IsDeleted != true);
        }

        public async Task<List<Order>> GetByBuyerIdAsync(string buyerId, int page = 1, int pageSize = 10)
        {
            return await dbContext.Orders
                .Include(o => o.Buyer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.BuyerId == buyerId && o.IsDeleted != true)
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Order>> GetBySellerIdAsync(string sellerId, int page = 1, int pageSize = 10)
        {
            return await dbContext.Orders
                .Include(o => o.Buyer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Seller)
                .Where(o => o.OrderItems.Any(oi => oi.SellerId == sellerId) && o.IsDeleted != true)
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Order>> GetAllAsync(int page = 1, int pageSize = 10)
        {
            return await dbContext.Orders
                .Include(o => o.Buyer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Seller)
                .Where(o => o.IsDeleted != true)
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Order>> GetByStatusAsync(string status, int page = 1, int pageSize = 10)
        {
            return await dbContext.Orders
                .Include(o => o.Buyer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Seller)
                .Where(o => o.Status == status && o.IsDeleted != true)
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Order>> GetByPaymentStatusAsync(string paymentStatus, int page = 1, int pageSize = 10)
        {
            return await dbContext.Orders
                .Include(o => o.Buyer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Seller)
                .Where(o => o.PaymentStatus == paymentStatus && o.IsDeleted != true)
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Order> CreateAsync(Order order)
        {
            // Ensure unique order number
            while (await OrderNumberExistsAsync(order.OrderNumber))
            {
                order.OrderNumber = GenerateOrderNumber();
            }

            order.CreatedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;

            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            order.UpdatedAt = DateTime.UtcNow;
            dbContext.Orders.Update(order);
            await dbContext.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var order = await dbContext.Orders.FindAsync(id);
            if (order == null) return false;

            order.IsDeleted = true;
            order.DeletedBy = "system"; // This should be passed as parameter
            order.UpdatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateOrderStatusAsync(string orderId, string status, string? notes = null)
        {
            var order = await dbContext.Orders.FindAsync(orderId);
            if (order == null) return false;

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;

            if (status == OrderStatus.Shipped)
                order.ShippedAt = DateTime.UtcNow;
            else if (status == OrderStatus.Delivered)
                order.DeliveredAt = DateTime.UtcNow;
            else if (status == OrderStatus.Cancelled)
                order.CancelledAt = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(notes))
                order.Notes = notes;

            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePaymentStatusAsync(string orderId, string paymentStatus, string? notes = null)
        {
            var order = await dbContext.Orders.FindAsync(orderId);
            if (order == null) return false;

            order.PaymentStatus = paymentStatus;
            order.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(notes))
                order.Notes = notes;

            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate, int page = 1, int pageSize = 10)
        {
            return await dbContext.Orders
                .Include(o => o.Buyer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Seller)
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate && o.IsDeleted != true)
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalSalesAsync(string? sellerId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = dbContext.Orders.Where(o => o.IsDeleted != true);

            if (sellerId != null)
            {
                query = query.Where(o => o.OrderItems.Any(oi => oi.SellerId == sellerId));
            }

            if (startDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt <= endDate.Value);
            }

            return await query.SumAsync(o => o.TotalAmount);
        }

        public async Task<int> GetOrderCountAsync(string? sellerId = null, string? status = null)
        {
            var query = dbContext.Orders.Where(o => o.IsDeleted != true);

            if (sellerId != null)
            {
                query = query.Where(o => o.OrderItems.Any(oi => oi.SellerId == sellerId));
            }

            if (status != null)
            {
                query = query.Where(o => o.Status == status);
            }

            return await query.CountAsync();
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await dbContext.Orders.AnyAsync(o => o.Id == id && o.IsDeleted != true);
        }

        public async Task<bool> OrderNumberExistsAsync(string orderNumber)
        {
            return await dbContext.Orders.AnyAsync(o => o.OrderNumber == orderNumber && o.IsDeleted != true);
        }

        private static string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        }
    }
}
