using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Data.RepositoryContracts;
using CoffeeShopManagement.Models.Models;
using Microsoft.EntityFrameworkCore;
using CoffeeShopManagement.Data.UnitOfWork;

namespace CoffeeShopManagement.Business.Services
{
    public class OrderService : IOrderService

    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;

        }

        public async Task<(List<OrderDTO> Orders, int TotalCount)> GetOrdersWithCount(
      string search,
      int? status,
      string sortColumn,
      bool isDescending,
      int pageNumber,
      int pageSize)
        {
            // Gọi đến repository để lấy danh sách orders và tổng số lượng
            var (orders, totalCount) = await _orderRepository.GetOrders(search, status, sortColumn, isDescending, pageNumber, pageSize);

            // Mapping thủ công từ Order Entity -> OrderDTO
            var orderDtos = orders.Select(order => new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                UserName = order.User?.UserName ?? "N/A", // Đảm bảo UserName không null
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalPrice = order.OrderDetails.Sum(od => od.OrderPrice * od.Quantity), // Tổng giá trị đơn hàng
                TotalQuantity = order.OrderDetails.Sum(od => od.Quantity), // Tổng số lượng sản phẩm
                                                                           // Thêm chi tiết sản phẩm vào DTO
                OrderDetails = order.OrderDetails.Select(detail => new AdminOrderDetailDTO
                {
                    Id = detail.Id,
                    ProductId = detail.ProductId,
                    ProductName = detail.Product?.ProductName ?? "Unknown", // Đảm bảo tên sản phẩm không null
                    Quantity = detail.Quantity,
                    Price = detail.OrderPrice
                }).ToList()
            }).ToList();

            return (orderDtos, totalCount);
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
                OrderDetails = order.OrderDetails.Select(d => new AdminOrderDetailDTO
                {
                    Id = d.Id,
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    Price = d.OrderPrice
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
        public async Task<IEnumerable<UserOrderDTO>> GetOrdersByUserId(Guid id)
        {
            var orders = _unitOfWork.OrderRepository.GetQuery()
                .Where(x => x.UserId == id)
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return orders.Select(x =>
            {
                var totalPrice = _unitOfWork.OrderDetailRepository.GetQuery()
                    .Where(d => d.OrderId == x.Id)
                    .Sum(d => d.OrderPrice * d.Quantity);

                return x.ToOrderDTO(totalPrice);
            });
        }

    }
}
