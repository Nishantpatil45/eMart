using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMart.Service.DataModels
{
    public class RecentlyViewed
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public DateTime ViewedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
