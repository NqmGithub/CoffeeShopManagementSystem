
﻿using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Business.Services;
using CoffeeShopManagement.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopManagement.WebAPI.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]

    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categorytService)
        {
            _categoryService = categorytService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListCategory()
        {
            var category = await _categoryService.GetListCategory();
            return Ok(category);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var category = await _categoryService.GetCategory(id);
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(ManageCategoryDTO categoryAddDTO)
        {
            var result = await _categoryService.AddCategory(categoryAddDTO);
            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id,ManageCategoryDTO categoryUpdateDTO)
        {
            
            var result = await _categoryService.UpdateCategory(id,categoryUpdateDTO);
            return Ok(result);
        }

        [HttpDelete("detele/{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {

              await _categoryService.DeleteCategory(id);

            return NoContent();
        }

        [HttpGet("name")]
        public async Task<IActionResult> GetAllCategoryNames()
        {
            var categories = await _categoryService.GetAllCategory();
            return Ok(categories.Select(x=> x.CategoryName).Distinct());
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeStatusCategory(Guid id, [FromQuery] int status)
        {
            var result = await _categoryService.ChangeStatusCategoryById(id, status);
            return Ok(result);
        }
        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var category = await _categoryService.GetCategoryCount();
            return Ok(category);
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchCategories(
     [FromQuery] string keyword = "",
     [FromQuery] string status = "all",
     [FromQuery] int pageNumber = 1,
     [FromQuery] int pageSize = 5)
        {
            try
            {
                var result = await _categoryService.SearchCategory(keyword, status, pageNumber, pageSize);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpGet("checkName")]
        public async Task<IActionResult> CheckCategoryNameExists(string categoryName)
        {
            var result = await _categoryService.CheckCategoryNameExist(categoryName);
            return Ok(result);
        }
        [HttpGet("countByName")]
        public async Task<IActionResult> CountCategories([FromQuery] string keyword, [FromQuery] string status)
        {
            try
            {
                int count = await _categoryService.CountSearchCategory(keyword, status);

                return Ok(new { TotalCount = count });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

    }
}

