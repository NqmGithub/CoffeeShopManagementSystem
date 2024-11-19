using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.ServiceContracts
{
    public interface IOrderDetailService
    {
        Task<List<OrderDetailDTO>> GetOrderDetails(Guid orderId);
        Task<bool> SaveOrderDetails(Guid orderId, List<OrderDetailDTO> orderDetails);
        Task<ICollection<OrderDetailDTO>> GetListOrderDetailsByOrderId(Guid id);

        Task<bool> RatingProducts(RatingProductDTO[] list);
    }
}
