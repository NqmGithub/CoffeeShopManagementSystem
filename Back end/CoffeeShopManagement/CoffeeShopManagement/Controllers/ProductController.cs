using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

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
        public async Task<IActionResult> GetListProducts(string search = "",
           string filterCategory = "", string filterStatus = "",
             int page = 0, int pageSize = 6,
           string sortColumn = "ProductName",
            string sortDirection = "asc")
        {
            //result include: list products and totalProducts after filter,sort, pagination
            var productQueryRequest = new ProductQueryRequest()
            {
                Search = search,
                FilterCategory = filterCategory,
                FilterStatus = filterStatus,
                Page = page,
                PageSize = pageSize,
                SortColumn = sortColumn,
                SortDirection = sortDirection
            };
            var result = await _productService.GetProductWithCondition(productQueryRequest);
            return Ok(result);
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
        public async Task<IActionResult> ChangStatus(Guid id, [FromQuery] string status)
        {
            var result = await _productService.ChangeStatusProductById(id,status);
            return Ok(result);
        }

        [HttpGet("checkName")]
        public async Task<IActionResult> CheckProductNameExists(string productName)
        {
            var result = await _productService.CheckProductNameExist(productName);
            return Ok(result);
        }
    }
}

