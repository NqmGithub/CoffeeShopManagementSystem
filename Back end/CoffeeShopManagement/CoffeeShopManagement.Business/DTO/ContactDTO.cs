using CoffeeShopManagement.Business.Helpers;
using CoffeeShopManagement.Data.UnitOfWork;
using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.DTO
{
    public class ContactDTO
    {
        public Guid Id { get; set; }

        public UserContactDTO Customer { get; set; }

        public string? AdminName { get; set; }

        public DateTime SendDate { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public string ProblemName { get; set; }

        public string Response { get; set; }
        public string Status { get; set; }
    }
}
