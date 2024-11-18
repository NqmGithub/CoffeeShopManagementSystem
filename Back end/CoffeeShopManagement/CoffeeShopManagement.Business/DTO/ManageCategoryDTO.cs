using CoffeeShopManagement.Business.Helpers;
using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.DTO
{
    public class ManageCategoryDTO
    {

        [Required(ErrorMessage = "CategoryName is required")]
        public string CategoryName { get; set; }
        public int Status { get; set; }
        public Category ToCategory()
        {
            return new Category
            {
                Id = Guid.NewGuid(),
                CategoryName = CategoryName,
                Status = Status,
            };
        }
    }
}
