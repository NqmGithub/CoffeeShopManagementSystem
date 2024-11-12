using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShopManagement.Models.Enums;

namespace CoffeeShopManagement.Business.ServiceContracts
{
    public interface IProductService
    {
        ICollection<ProductDTO> GetBestSeller();

        Task<ProductDTO> GetProductById(Guid id);

        ICollection<ProductDTO> GetListProduct();

        Task<bool> CreateProduct(ProductCreateDTO productCreateDTO);

        Task<bool> UpdateProduct(ProductUpdateDTO productUpdateDTO);

        Task<bool> ChangeStatusProductById(Guid id, string choice);

        Task<IEnumerable<ProductDTO>> GetAllProductsAsync(
             string search,
             string category,
             decimal? minPrice,
             decimal? maxPrice,
             int page,
             int pageSize,
             SortBy sortBy,
             bool isDescending);
        Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(Guid categoryId);
        Task<IEnumerable<ProductDTO>> GetTopBestsellerProductsAsync(int top);
    }
   
}

