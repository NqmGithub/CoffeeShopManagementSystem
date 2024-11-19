using CoffeeShopManagement.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.ServiceContracts
{
    public interface IOrderDetailService
    {
        Task<List<AdminOrderDetailDTO>> GetOrderDetails(Guid orderId);
        Task<bool> SaveOrderDetails(Guid orderId, List<AdminOrderDetailDTO> orderDetails);
        Task<ICollection<OrderDetailDTO>> GetListOrderDetailsByOrderId(Guid id);

        Task<bool> RatingProducts(RatingProductDTO[] list);
    }
}
