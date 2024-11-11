﻿using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.ServiceContracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>>GetListCategory();
        Task<Category> GetCategory(Guid id);
        Task<bool> AddCategory(CategoryDTO categoryAddDTO);
        Task<bool> UpdateCategory(Guid id, CategoryDTO categoryUpdateDTO);
        Task DeleteCategory(Guid id);

    }
}
