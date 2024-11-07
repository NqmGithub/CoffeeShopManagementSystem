using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Data.RepositoryContracts
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByEmail(string email);
        Task Add(User user);
        Task Delete(Guid id);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(Guid id);
        Task<IEnumerable<User>> GetPagination(int pageNumber, int pageSize);
        Task Update(User user);

        Task<int> GetUserCount();

    }
}
