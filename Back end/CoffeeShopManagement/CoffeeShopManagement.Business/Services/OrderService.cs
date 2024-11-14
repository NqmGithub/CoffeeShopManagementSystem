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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<(IEnumerable<OrderDTO> Orders, int TotalRecords)> GetOrders(
      string? search,
      int? status,
      string? sortColumn,
      bool isDescending,
      int pageNumber,
      int pageSize)
        {
            var orders = await _orderRepository.GetOrders(search, status, null, false, 1, int.MaxValue);
            var totalRecords = orders.Count();

            var orderDtos = orders.Select(o => new OrderDTO
            {
                Id = o.Id,
                UserId = o.UserId,
                UserName = o.User?.UserName ?? "N/A",
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalPrice = o.OrderDetails.Sum(od => od.OrderPrice * od.Quantity),
                TotalQuantity = o.OrderDetails.Sum(od => od.Quantity),
            }).AsQueryable();


            orderDtos = sortColumn?.ToLower() switch
            {
                "totalprice" => isDescending ? orderDtos.OrderByDescending(o => o.TotalPrice) : orderDtos.OrderBy(o => o.TotalPrice),
                "totalquantity" => isDescending ? orderDtos.OrderByDescending(o => o.TotalQuantity) : orderDtos.OrderBy(o => o.TotalQuantity),
                "status" => isDescending ? orderDtos.OrderByDescending(o => o.Status) : orderDtos.OrderBy(o => o.Status),
                _ => isDescending ? orderDtos.OrderByDescending(o => o.OrderDate) : orderDtos.OrderBy(o => o.OrderDate)
            };


            var paginatedOrders = orderDtos.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return (paginatedOrders, totalRecords);
        }

        public async Task<OrderDTO?> GetOrderById(Guid id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null) return null;

            return new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                UserName = order.User?.UserName??"N/A",
                Status = order.Status,
                OrderDate = order.OrderDate,
                TotalPrice = order.OrderDetails.Sum(od => od.OrderPrice * od.Quantity),
                TotalQuantity = order.OrderDetails.Sum(od => od.Quantity),
                OrderDetails = order.OrderDetails.Select(d => new OrderDetailDTO
                {
                    Id = d.Id,
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    OrderPrice = d.OrderPrice
                }).ToList()
            };
        }

        public async Task<(bool IsSuccess, string Message, int NewStatus)> ToggleOrderStatus(Guid id)
        {
            var order = await _orderRepository.GetOrderById(id);

            if (order == null)
            {
                return (false, "Order not found", -1);
            }

            if (order.Status == 2)
            {
                return (false, "Cannot update status of a canceled order", order.Status);
            }

            switch (order.Status)
            {
                case 0: 
                    order.Status = 1; 
                    break;
                case 1:
                    order.Status = 2; 
                    break;
            }

            await _orderRepository.UpdateOrder(order);
            return (true, "Status updated successfully", order.Status);
        }
    }
}
