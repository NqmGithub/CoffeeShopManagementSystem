using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.DTO
{
    public class ContactQueryRequest
    {
        public string Search { get; set; } = "";
        public string FilterStatus { get; set; } = "";
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 5;
        public string SortColumn { get; set; } = "SendDate";
        public string SortDirection { get; set; } = "asc";
    }
}
