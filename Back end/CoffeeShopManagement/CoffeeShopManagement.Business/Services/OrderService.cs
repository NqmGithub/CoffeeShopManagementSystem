using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Data.RepositoryContracts;
using CoffeeShopManagement.Models.Models;
using CoffeeShopManagement.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // Get Orders with count and pagination
        public async Task<(List<OrderDTO> Orders, int TotalCount)> GetOrdersWithCount(
            string search,
            int? status,
            string sortColumn,
            bool isDescending,
            int pageNumber,
            int pageSize)
        {
            var (orders, totalCount) = await _unitOfWork.OrderRepository.GetOrders(search, status, sortColumn, isDescending, pageNumber, pageSize);

            var orderDtos = orders.Select(order => new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                UserName = order.User?.UserName ?? "N/A",
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalPrice = order.OrderDetails.Sum(od => od.OrderPrice * od.Quantity),
                TotalQuantity = order.OrderDetails.Sum(od => od.Quantity),
                OrderDetails = order.OrderDetails.Select(detail => new OrderDetailDTO
                {
                    Id = detail.Id,
                    ProductId = detail.ProductId,
                    ProductName = detail.Product?.ProductName ?? "Unknown",
                    Quantity = detail.Quantity,
                    OrderPrice = detail.OrderPrice
                }).ToList()
            }).ToList();

            return (orderDtos, totalCount);
        }

        // Get Order by ID
        public async Task<OrderDTO?> GetOrderById(Guid id)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderById(id);
            if (order == null) return null;

            return new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                UserName = order.User?.UserName ?? "N/A",
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

        // Toggle Order Status (Pending -> Completed or Cancelled)
        public async Task<(bool IsSuccess, string Message, int NewStatus)> ToggleOrderStatus(Guid id)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderById(id);

            if (order == null)
            {
                return (false, "Order not found", -1);
            }

            if (order.Status == 2)
            {
                return (false, "Cannot update status of a canceled order", order.Status);
            }

            order.Status = order.Status == 0 ? 1 : 2; // Toggle between Pending (0) and Completed (1) or Cancelled (2)

            await _unitOfWork.OrderRepository.UpdateOrder(order);
            return (true, "Status updated successfully", order.Status);
        }

        // Add a New Order
        public async Task<OrderDTO> AddOrderAsync(OrderCreateDTO orderCreateDTO)
        {
            // Validate User
            var user = await _unitOfWork.UserRepository.GetQuery()
                .FirstOrDefaultAsync(u => u.Id == orderCreateDTO.UserID);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Create the Order
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Status = 0,  // Default status 'Pending'
                OrderDate = DateTime.UtcNow,
                User = user
            };

            // Add Order to the repository
            await _unitOfWork.OrderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();  // Ensure ID is generated for the order

            var orderDetails = new List<OrderDetail>();

            // Validate and Create OrderDetails
            foreach (var item in orderCreateDTO.Details)
            {
                var product = await _unitOfWork.ProductRepository.GetQuery()
                    .FirstOrDefaultAsync(p => p.Id == item.ProductId);

                if (product == null)
                {
                    throw new Exception($"Product {product.ProductName} not found");
                }

                var orderDetail = new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    OrderPrice = product.Price,
                    Quantity = item.Quantity,
                    Rating = 0  // Default value, can be updated later
                };

                orderDetails.Add(orderDetail);
            }

            // Add Order Details to the repository
            await _unitOfWork.OrderDetailRepository.AddAsync(orderDetails);
            // Save all changes to the database
            await _unitOfWork.SaveChangesAsync();

            // Calculate total price and quantity
            var totalPrice = orderDetails.Sum(d => d.OrderPrice * d.Quantity);
            var totalQuantity = orderDetails.Sum(d => d.Quantity);

            // Create DTO for the new order
            var orderDTO = new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                Status = order.Status,
                OrderDate = order.OrderDate,
                TotalPrice = totalPrice,
                TotalQuantity = totalQuantity,
                OrderDetails = orderDetails.Select(d => new OrderDetailDTO
                {
                    ProductId = d.ProductId,
                    ProductName = d.Product?.ProductName ?? "",
                    OrderPrice = d.OrderPrice,
                    Quantity = d.Quantity,
                    TotalPrice = d.OrderPrice * d.Quantity,
                    Img = d.Product?.Thumbnail
                }).ToList()
            };

            return orderDTO;
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
