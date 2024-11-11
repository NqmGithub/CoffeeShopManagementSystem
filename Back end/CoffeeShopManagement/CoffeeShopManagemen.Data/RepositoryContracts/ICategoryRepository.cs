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
    }
}
