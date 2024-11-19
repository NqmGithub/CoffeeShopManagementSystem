using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShopManagement.Business.DTO;

namespace CoffeeShopManagement.Business.ServiceContracts
{
    public interface ICategoryService
    {

        Task<IEnumerable<CategoryDTO>>GetListCategory();
        Task<CategoryDTO> GetCategory(Guid id);
        Task<bool> AddCategory(ManageCategoryDTO categoryAddDTO);
        Task<bool> UpdateCategory(Guid id, ManageCategoryDTO categoryUpdateDTO);
        Task DeleteCategory(Guid id);
        Task<bool> ChangeStatusCategoryById(Guid id, int choice);
        Task<int> GetCategoryCount();
        Task<IEnumerable<Category>> GetAllCategory();
        Task<SearchCategoryResult> SearchCategory(string keyword, string status, int pageNumber, int pageSize);
        Task<int> CountSearchCategory(string keyword, string status);
        Task<bool> CheckCategoryNameExist(string categoryName);
    }
}
