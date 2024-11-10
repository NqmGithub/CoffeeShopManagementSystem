using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeeShopManagement.Models.Enums;

namespace CoffeeShopManagement.Data.RepositoryContracts
{
    public interface IProductRepository : IGenericRepository<Product>
    {

        Task<IEnumerable<Product>> GetAllProductsAsync(string search,
           string category,
           decimal? minPrice,
           decimal? maxPrice,
           int page,
           int pageSize,
           SortBy sortBy,
           bool isDescending);
        Task<IEnumerable<Product>> GetProductsByCategoryId(Guid categoryId);
        Task<IEnumerable<Product>> GetTopBestsellersAsync(int top);
    }
}

