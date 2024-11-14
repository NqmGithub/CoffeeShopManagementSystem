using CoffeeShopManagement.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.ServiceContracts
{
    public interface IOrderService
    {
        Task<(IEnumerable<OrderDTO> Orders, int TotalRecords)> GetOrders(
       string? search,
       int? status,
       string? sortColumn,
       bool isDescending,
       int pageNumber,
       int pageSize);

        Task<OrderDTO?> GetOrderById(Guid id);
        Task<(bool IsSuccess, string Message, int NewStatus)> ToggleOrderStatus(Guid id);
    }
}
