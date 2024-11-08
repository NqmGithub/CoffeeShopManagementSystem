using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.ServiceContracts
{
    public interface IProductService
    {
        ICollection<ProductDTO> GetBestSeller();

        Task<ProductDTO> GetProductById(Guid id);

        ICollection<ProductDTO> GetListProduct();

        Task<bool> CreateProduct(ProductCreateDTO productCreateDTO);

        Task<bool> UpdateProduct(ProductUpdateDTO productUpdateDTO);

        Task<bool> ChangeStatusProductById(Guid id,string choice);

        Task<ProductListResponse> GetProductWithCondition
            (ProductQueryRequest productQueryRequest);
    }
}
