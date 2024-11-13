﻿using CoffeeShopManagement.Models.Models;
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

        Task<IEnumerable<Category>>GetListCategory();
        Task<Category> GetCategory(Guid id);
        Task<bool> AddCategory(ManageCategoryDTO categoryAddDTO);
        Task<bool> UpdateCategory(Guid id, ManageCategoryDTO categoryUpdateDTO);
        Task DeleteCategory(Guid id);


        Task<IEnumerable<Category>> GetAllCategory();

    }
}
