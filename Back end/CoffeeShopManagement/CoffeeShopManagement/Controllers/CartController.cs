using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Business.Services;
using CoffeeShopManagement.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace CoffeeShopManagement.WebAPI.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartCookieService _cartService;
        private readonly IProductService _productService;

        public CartController(CartCookieService cartService, IProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        // Thêm sản phẩm vào giỏ hàng
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromQuery] Guid productId, [FromQuery] int quantity = 1)
        {
            var product = await _productService.GetProductById(productId);
            if (product == null) return NotFound("Product not found");

            var cartItem = new CartItem
            {
                ProductId = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                Quantity = quantity
            };

            _cartService.AddToCart(cartItem);
            return Ok("Product added to cart");
        }

        // Lấy danh sách sản phẩm trong giỏ hàng
        [HttpGet]
        public IActionResult GetCart()
        {
            var cartItems = _cartService.GetCartItems();
            return Ok(cartItems);
        }

        // Xóa một sản phẩm khỏi giỏ hàng
        [HttpDelete("{productId}")]
        public IActionResult RemoveFromCart(Guid productId)
        {
            _cartService.RemoveFromCart(productId);
            return Ok("Product removed from cart");
        }

        // Xóa toàn bộ giỏ hàng
        [HttpDelete("clear")]
        public IActionResult ClearCart()
        {
            _cartService.ClearCart();
            return Ok("Cart cleared");
        }
    }

}
