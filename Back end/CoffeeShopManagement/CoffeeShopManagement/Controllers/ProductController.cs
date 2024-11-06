using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopManagement.WebAPI.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
      
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _productService.GetProductById(id);
            return Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetAlllProducts()
        {
            var products =  _productService.GetListProduct();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(ProductCreateDTO productCreateDTO)
        {
            var result = await _productService.CreateProduct(productCreateDTO);
            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, ProductUpdateDTO productUpdateDTO)
        {
            var result = await _productService.UpdateProduct(productUpdateDTO);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> DeactiveUser(Guid id, [FromQuery] string status)
        {
            var result = await _productService.ChangeStatusProductById(id,status);
            return Ok(result);
        }
    }
}

