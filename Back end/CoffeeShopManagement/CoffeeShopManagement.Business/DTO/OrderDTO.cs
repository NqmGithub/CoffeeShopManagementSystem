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
        public int Status { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; }
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
    }

    public class OrderDetailDTO
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public decimal TotalPrice => Price * Quantity;    

    }
    public class OrderCreateDto
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
    }
    public class OrderUpdateDto
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
    }
}
