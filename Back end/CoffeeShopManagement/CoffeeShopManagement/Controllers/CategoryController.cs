using CoffeeShopManagement.Business.DTO;
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
        public async Task<IActionResult> AddCategory(CategoryDTO categoryAddDTO)
        {
            var result = await _categoryService.AddCategory(categoryAddDTO);
            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id,CategoryDTO categoryUpdateDTO)
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


    }
}
