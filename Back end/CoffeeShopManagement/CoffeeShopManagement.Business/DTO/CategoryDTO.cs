using CoffeeShopManagement.Business.Helpers;
using CoffeeShopManagement.Data.UnitOfWork;
using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.DTO
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
    }
    public static class categoryExtensions
    {
        public static CategoryDTO ToCategoryDTO(this Category category, IUnitOfWork unitOfWork)
        {
           
            return new CategoryDTO()
            {
                Id = category.Id,
                Name = category.CategoryName,
                Status = category.Status,
            };
        }
    }
}
