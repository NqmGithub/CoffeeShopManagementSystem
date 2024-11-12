using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoffeeShopManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Customer")]
    public class UserProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;

        public UserProductController(IProductService productService, ICategoryService categoryService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
        }

        private void ValidateProductParameters(
            ref string search,
            ref string category,
            ref decimal? minPrice,
            ref decimal? maxPrice,
            ref SortBy sortBy,
            ref bool isDescending,
            ref int page,
            ref int pageSize)
        {
            search = search ?? string.Empty;
            category = category ?? string.Empty;
            minPrice = minPrice ?? 0;
            maxPrice = maxPrice ?? 1000000;
            page = page <= 0 ? 1 : page;
            pageSize = (pageSize <= 0 || pageSize > 100) ? 10 : pageSize;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryService.GetAllCategories();
            if (categories == null || !categories.Any())
            {
                return NoContent();
            }
            return Ok(categories);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts(
            [FromQuery] string search = "",
            [FromQuery] string category = "",
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] SortBy sortBy = SortBy.Id,
            [FromQuery] bool isDescending = false)
        {
            ValidateProductParameters(ref search, ref category, ref minPrice, ref maxPrice, ref sortBy, ref isDescending, ref page, ref pageSize);

            var products = await productService.GetAllProductsAsync(search, category, minPrice, maxPrice, page, pageSize, sortBy, isDescending);
            return Ok(products);
        }
    }
}
