using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.DTO
{
    public class CreateContactDTO
    {

        public string  Email { get; set; }
        public DateTime SendDate { get; set; }
        public string Subject { get; set; }
        public string ProblemName { get; set; }
        public string Content { get; set; }

    }
}
