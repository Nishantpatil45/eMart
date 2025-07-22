using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMart.Service.Core.Dtos.Product
{
    public class ProductListRequestDto
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string? SortBy { get; set; }
        public string? SortDir { get; set; }
        public ProductColumnValueSearchDto? ColumnValueSearchlist { get; set; }
    }

    public class ProductColumnValueSearchDto
    {
        public string? SearchProductName { get; set; }
        public string? SearchCategory { get; set; }
    }
}
