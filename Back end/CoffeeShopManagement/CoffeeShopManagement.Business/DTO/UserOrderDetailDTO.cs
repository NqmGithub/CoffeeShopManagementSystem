using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.DTO
{
    public class UserOrderDetailDTO
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
