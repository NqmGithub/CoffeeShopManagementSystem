using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Data.RepositoryContracts
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task AddCategory(Category category);
        Task DeleteCategory(Guid id);
        Task<IEnumerable<Category>> GetListCategory();
        Task<Category> GetCategoryById(Guid id);       
        Task UpdateCategory(Category category);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<int> GetCategoryCount();
        Task<IEnumerable<Category>> SearchCategory(string keyword, string status, int pageNumber, int pageSize);
        IQueryable<Category> GetAll();
        Task<int> GetCategoryCount(string keyword, string status);
    }
}
