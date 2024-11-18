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
        Task<ICollection<OrderDetailDTO>> GetListOrderDetailsByOrderId(Guid id);
    }
}
