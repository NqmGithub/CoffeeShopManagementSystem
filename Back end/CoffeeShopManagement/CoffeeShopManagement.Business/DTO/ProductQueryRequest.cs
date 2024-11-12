using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.DTO
{
    public class ProductQueryRequest
    {
        public string Search { get; set; } = "";
        public string FilterCategory { get; set; } = "";
        public string FilterStatus { get; set; } = "";
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 5;
        public string SortColumn { get; set; } = "ProductName";
        public string SortDirection { get; set; } = "asc";
    }
}
