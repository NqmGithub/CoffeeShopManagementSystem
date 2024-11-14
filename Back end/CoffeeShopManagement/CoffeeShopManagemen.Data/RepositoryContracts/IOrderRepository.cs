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
        Task<IEnumerable<Order>> GetOrders(
        string? search,
        int? status,
        string? sortColumn,
        bool isDescending,
        int pageNumber,
        int pageSize);

        Task<int> GetTotalRecords(string? search, int? status);
        Task<Order?> GetOrderById(Guid id);
        Task UpdateOrder(Order order);
    }
}
