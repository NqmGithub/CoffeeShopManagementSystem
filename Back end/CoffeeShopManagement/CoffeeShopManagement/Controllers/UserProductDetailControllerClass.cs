using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Business.Services;
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
        private readonly IOrderService orderService;

        public UserProductDetailController(IProductService productService, ICategoryService categoryService, IOrderService orderService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
            this.orderService = orderService;
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
        [HttpPost()]
        public async Task<IActionResult> AddOrder([FromBody] OrderCreateDTO orderCreateDTO)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (orderCreateDTO == null || orderCreateDTO.OrderDetails == null || !orderCreateDTO.OrderDetails.Any())
                {
                    return BadRequest("Invalid order data. Please provide valid order items.");
                }

                // Gọi service để thêm đơn hàng
                var result = await orderService.AddOrderAsync(orderCreateDTO);

                // Trả về kết quả thành công
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Trả về lỗi server
                return StatusCode(500, new { message = $"An error occurred while creating the order: {ex.Message}" });
            }
        }
    }
}
