using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.DTO
{
    public class RatingProductDTO
    {
        public Guid Id { get; set; }

        public int Rating { get; set; } = 0;
    }
}
