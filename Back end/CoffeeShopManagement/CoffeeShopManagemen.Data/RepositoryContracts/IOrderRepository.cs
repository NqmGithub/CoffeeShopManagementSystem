using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Data.RepositoryContracts
{
    public interface IOrderRepository:IGenericRepository<Order>
    {
        Task<(List<Order>, int TotalCount)> GetOrders(
         string search,
         int? status,
         string sortColumn,
         bool isDescending,
         int pageNumber,
         int pageSize);

        Task<Order?> GetOrderById(Guid id);
        Task UpdateOrder(Order order);
        Task AddAsync(Order order);
    }
}
