using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShopManagement.Business.DTO;

namespace CoffeeShopManagement.Business.ServiceContracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProductsbySearch(string searchTerm, int page, int pageSize);
        Task<IEnumerable<ProductDTO>> GetAllProducts(int page, int pageSize, string search, string category);
        Task<IEnumerable<ProductDTO>> GetProductsByFilter(string category, decimal? minPrice, decimal? maxPrice, double? minRating, int page, int pageSize);
    }
}
