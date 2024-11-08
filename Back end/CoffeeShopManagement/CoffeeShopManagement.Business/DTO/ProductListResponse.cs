using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.DTO
{
    public class ProductListResponse
    {
        public List<ProductDTO> List { get; set; }
        public int Total { get; set; }
    }
}
