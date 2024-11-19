using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.ServiceContracts
{
    public interface IOrderService
    {
        Task<(List<OrderDTO> Orders, int TotalCount)> GetOrdersWithCount(
         string search,
         int? status,
         string sortColumn,
         bool isDescending,
         int pageNumber,
         int pageSize);

        Task<OrderDTO?> GetOrderById(Guid id);
        Task<(bool IsSuccess, string Message, int NewStatus)> ToggleOrderStatus(Guid id);
       
        Task<IEnumerable<UserOrderDTO>> GetOrdersByUserId(Guid id);
        Task<OrderDTO> AddOrderAsync(OrderCreateDTO orderCreateDTO);
    }
}
