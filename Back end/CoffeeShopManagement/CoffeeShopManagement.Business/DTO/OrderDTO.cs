using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.DTO
{
    public class OrderCreateDTO
    {
        public Guid UserID { get; set; }
        public List<UserOrderDetailDTO> Details { get; set; }  // List of products in the order
    }





    public class OrderDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int Status { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; } 
    }


    public class OrderUpdateDTO
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
     
    }

    public static class OrderExtension
    {
        public static UserOrderDTO ToOrderDTO(this Order order, decimal price)
        {
            return new UserOrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                Status = order.Status.ToString(),  // Convert status to string (e.g., "1", "2", etc.)
                OrderDate = order.OrderDate,
                TotalPrice = price
            };
        }
    }

    public class UserOrderDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Status { get; set; }  // Status as string (e.g., "Pending", "Completed", etc.)
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
    }

}
