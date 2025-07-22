using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMart.Service.Core.Dtos.Product
{
    public class ProductCreateRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryId { get; set; }
        public int Stock { get; set; }
        public string Brand { get; set; }
        public string ImageUrl { get; set; }
        public int Status { get; set; }
    }
}
