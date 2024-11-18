using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.DTO
{
    public class SearchCategoryResult
    {
        public int TotalCount { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
