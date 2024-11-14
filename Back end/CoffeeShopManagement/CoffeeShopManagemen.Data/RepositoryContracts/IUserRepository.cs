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
        Task Update(User user);
        Task<int> GetUserCount();
        Task<IEnumerable<User>> SearchUser(string keyword, string status, int pageNumber, int pageSize);
    }
}
