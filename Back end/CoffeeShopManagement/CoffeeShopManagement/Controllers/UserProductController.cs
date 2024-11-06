using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Business.Services;
using CoffeeShopManagement.Data.RepositoryContracts;
using CoffeeShopManagement.Data.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
namespace CoffeeShopManagement.WebAPI.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/controller")]
    [ApiController]
    public class UserProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IProductService productService;
        public UserProductController(IProductRepository productRepository, IUnitOfWork unitOfWork, IProductService productService)
        {
            this.productRepository = productRepository;
            this.unitOfWork = unitOfWork;
            this.productService = productService;
        }
        [HttpGet("search")]
        public async Task<IActionResult> GetProductBySearch(
               [FromQuery] string searchTerm,
               [FromQuery] int page = 1,
               [FromQuery] int pageSize = 10)
        {
            var products = await productService.GetProductsbySearch(searchTerm, page, pageSize);
            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "",
            [FromQuery] string category = "")
        {
            var products = await productService.GetAllProducts(page, pageSize, search, category);
            return Ok(products);
        }
        [HttpGet("filter")]
        public async Task<IActionResult> GetProductByFilter(
           [FromQuery] string category = "",
           [FromQuery] decimal? minPrice = null,
           [FromQuery] decimal? maxPrice = null,
           [FromQuery] double? minRating = null,
           [FromQuery] int page = 1,
           [FromQuery] int pageSize = 10)
        {
            var products = await productService.GetProductsByFilter(category, minPrice, maxPrice, minRating, page, pageSize);
            return Ok(products);
        }
    }
}

