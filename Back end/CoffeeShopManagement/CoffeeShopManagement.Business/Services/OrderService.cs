using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Data.UnitOfWork;

namespace CoffeeShopManagement.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<OrderDTO>> GetOrdersByUserId(Guid id)
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
