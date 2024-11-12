using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShopManagement.Business.DTO;

namespace CoffeeShopManagement.Business.ServiceContracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategory();
    }
}
