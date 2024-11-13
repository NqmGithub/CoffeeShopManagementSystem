using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CoffeeShopManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Customer")]
    public class UserProductDetailController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;

        public UserProductDetailController(IProductService productService, ICategoryService categoryService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(Guid productId)
        {
            var product = await productService.GetProductById(productId);
            if (product == null)
            {
                return NotFound($"Product with ID {productId} not found.");
            }
            return Ok(product);
        }

        [HttpGet("related/{productId}")]
        public async Task<IActionResult> GetProductsByCategoryOrBestseller(Guid productId)
        {
            var product = await productService.GetProductById(productId);
            if (product == null)
            {
                return NotFound($"Product with ID {productId} not found.");
            }

            var relatedProducts = await productService.GetProductsByCategoryAsync(productId);

            // Nếu số lượng sản phẩm liên quan < 3, lấy danh sách bestsellers
            if (relatedProducts == null || !relatedProducts.Any() || relatedProducts.Count() < 3)
            {
                relatedProducts = await productService.GetTopBestsellerProductsAsync(3);
            }

            return Ok(relatedProducts);
        }
    }
}
