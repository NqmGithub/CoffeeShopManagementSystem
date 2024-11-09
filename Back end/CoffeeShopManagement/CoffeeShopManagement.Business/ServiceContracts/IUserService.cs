using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.ServiceContracts
{
    public interface IUserService
    {
        Task<User> GetByEmail(string email);
        Task<IEnumerable<User>> GetAll();
        Task<User> Get(Guid id);
        Task<IEnumerable<User>> GetPagination(int pageNumber, int pageSize);
        Task Add(User user);
        Task Update(User user);
        Task Delete(Guid id);

        Task<int> GetUserCount();
    }
}
