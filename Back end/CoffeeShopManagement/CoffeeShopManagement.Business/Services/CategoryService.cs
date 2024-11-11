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

namespace CoffeeShopManagement.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Category>> GetListCategory()
        {
            var category=  await _unitOfWork.CategoryRepository.GetListCategory();
            return category.Select(c => new Category
            {
                Id = c.Id,
                CategoryName = c.CategoryName
            }).ToList();
        }
        public async Task<bool> AddCategory(CategoryDTO categoryAddDTO)
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
        public async Task<bool> UpdateCategory(Guid id, CategoryDTO categoryUpdateDTO)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategoryById(id);

            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            category.CategoryName = categoryUpdateDTO.CategoryName;

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
        public async Task<Category> GetCategory(Guid id)
        {

            return await _unitOfWork.CategoryRepository.GetCategoryById(id);
        }
    }
}
