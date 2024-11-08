using CoffeeShopManagement.Business.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet("name")]
        public async Task<IActionResult> GetAllCategoryNames()
        {
            var categories = await _categoryService.GetAllCategory();
            return Ok(categories.Select(x=> x.CategoryName).Distinct());
        }
    }
}
