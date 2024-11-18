using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.DTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Status { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }
    }

    public static class OrderExtension
    {
        public static OrderDTO ToOrderDTO(this Order order, decimal price)
        {
            return new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                Status = order.Status+"",
                OrderDate = order.OrderDate,
                TotalPrice = price
            };
        }
    }
}
