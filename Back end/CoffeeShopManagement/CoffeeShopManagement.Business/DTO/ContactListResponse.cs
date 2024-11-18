using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.DTO
{
    public class ContactListResponse
    {
        public List<ContactDTO> List { get; set; }
        public int Total { get; set; }
    }
}
