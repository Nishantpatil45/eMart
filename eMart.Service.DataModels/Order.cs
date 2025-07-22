using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMart.Service.DataModels
{
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string BuyerId { get; set; }
        public string SellerId { get; set; }
        public string ProductId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public User Buyer { get; set; }
        public User Seller { get; set; }
        public Product Product { get; set; }
    }
}
