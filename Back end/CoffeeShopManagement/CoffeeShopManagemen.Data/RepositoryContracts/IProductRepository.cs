using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Data.RepositoryContracts
{
    public interface IProductRepository :IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm, int page, int pageSize);
        Task<IEnumerable<Product>> GetAllProductsAsync(int page, int pageSize, string search, string category);
        Task<IEnumerable<Product>> FilterProductsAsync(string category, decimal? minPrice, decimal? maxPrice, double? minRating, int page, int pageSize);
    }
}
