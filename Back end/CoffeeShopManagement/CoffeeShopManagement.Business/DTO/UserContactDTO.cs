using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.DTO
{
    public class UserContactDTO
    {
        public string UserName { get; set; } = null!;
        public string? Avatar { get; set; }
    }
}
