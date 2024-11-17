using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Data.RepositoryContracts;
using CoffeeShopManagement.Models.Models;

namespace CoffeeShopManagement.Business.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _repository;

        public OrderDetailService(IOrderDetailRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<OrderDetailDTO>> GetOrderDetails(Guid orderId)
        {
            var details = await _repository.GetOrderDetailsByOrderId(orderId);
            return details.Select(d => new OrderDetailDTO
            {
                ProductId = d.ProductId,
                ProductName = d.Product != null ? d.Product.ProductName : "Unknown Product",
                Price = d.OrderPrice,
                Quantity = d.Quantity
            }).ToList();
        }

        public async Task<bool> SaveOrderDetails(Guid orderId, List<OrderDetailDTO> orderDetails)
        {
            var updatedDetails = orderDetails.Select(d => new OrderDetail
            {
                OrderId = orderId,
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                OrderPrice = d.Price,
            }).ToList();

            return await _repository.UpdateOrderDetails(orderId, updatedDetails);
        }
    }
}
