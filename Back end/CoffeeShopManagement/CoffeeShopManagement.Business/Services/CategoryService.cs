using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.Helpers;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Data.UnitOfWork;
using CoffeeShopManagement.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopManagement.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<CategoryDTO>> GetListCategory()
        {
            return _unitOfWork.CategoryRepository.GetAll().Select(x => x.ToCategoryDTO(_unitOfWork)).ToList();
        }

        public async Task<bool> AddCategory(ManageCategoryDTO categoryAddDTO)
        {
            if (categoryAddDTO == null)
            {
                throw new ArgumentNullException(nameof(categoryAddDTO));
            }
            ValidationHelper.ModelValidation(categoryAddDTO);

            var category = categoryAddDTO.ToCategory();
            _unitOfWork.CategoryRepository.Add(category);

            int result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }
        public async Task<bool> UpdateCategory(Guid id, ManageCategoryDTO categoryUpdateDTO)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategoryById(id);

            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            category.CategoryName = categoryUpdateDTO.CategoryName;
            category.Status = categoryUpdateDTO.Status;

            int result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }
        public async Task DeleteCategory(Guid id)
        {
            var exist = _unitOfWork.CategoryRepository.GetCategoryById(id);
            if (exist == null)
            {
                throw new ArgumentException($"No category with this id: {id}");
            }
             await _unitOfWork.CategoryRepository.DeleteCategory(id);
            
        }
        public async Task<CategoryDTO> GetCategory(Guid id)
        {

            var category = await _unitOfWork.CategoryRepository.GetCategoryById(id);
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            return category.ToCategoryDTO(_unitOfWork);
        }
            public async Task<IEnumerable<Category>> GetAllCategory()
        {
            return await _unitOfWork.CategoryRepository.GetAllAsync();
        }
        public async Task<bool> ChangeStatusCategoryById(Guid id, int status)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategoryById(id);
            if (category == null)
            {
                throw new ArgumentException(nameof(category));
            }
            category.Status = status;

            int result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<int> GetCategoryCount()
        {
            return await _unitOfWork.CategoryRepository.GetCategoryCount();
        }
        public async Task<int> CountSearchCategory(string keyword, string status)
        {
            return await _unitOfWork.CategoryRepository.GetCategoryCount(keyword, status);
        }

        public async Task<SearchCategoryResult> SearchCategory(string keyword, string status, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                throw new ArgumentException("Page number and size must be greater than 0");
            }
            if (pageSize > 30)
            {
                throw new ArgumentException("Page size must not be too big (< 30)");
            }

            int totalCount = await _unitOfWork.CategoryRepository.GetCategoryCount(keyword, status);

            if (totalCount == 0)
            {
                return new SearchCategoryResult { TotalCount = 0, Categories = new List<Category>() };
            }

            int maxPageNumber = (int)Math.Ceiling((double)totalCount / pageSize);
            if (pageNumber > maxPageNumber)
            {
                throw new ArgumentException("Page number is out of range.");
            }

            var categories = await _unitOfWork.CategoryRepository.SearchCategory(keyword, status, pageNumber, pageSize);

            return new SearchCategoryResult
            {
                TotalCount = totalCount,
                Categories = categories
            };
        }


        public async Task<bool> CheckCategoryNameExist(string categoryName)
        {
            var temp = await _unitOfWork.CategoryRepository.GetQuery().FirstOrDefaultAsync(x => x.CategoryName.Equals(categoryName));
            if (temp != null)
            {
                return true;
            }
            return false;
        }
    }
}

