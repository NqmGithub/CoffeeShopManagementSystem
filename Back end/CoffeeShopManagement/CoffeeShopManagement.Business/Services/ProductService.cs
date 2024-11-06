using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Data.RepositoryContracts;
using CoffeeShopManagement.Data.UnitOfWork;
using CoffeeShopManagement.Models.Models;

namespace CoffeeShopManagement.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsbySearch(string searchTerm, int page, int pageSize)
        {
            var products = await _productRepository.SearchProductsAsync(searchTerm, page, pageSize);
            return products.Select(p => p.ToProductDTO(_unitOfWork)); 
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProducts(int page, int pageSize, string search, string category)
        {
            var products = await _productRepository.GetAllProductsAsync(page, pageSize, search, category);
            return products.Select(p => p.ToProductDTO(_unitOfWork)); 
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsByFilter(string category, decimal? minPrice, decimal? maxPrice, double? minRating, int page, int pageSize)
        {
            var products = await _productRepository.FilterProductsAsync(category, minPrice, maxPrice, minRating, page, pageSize);
            return products.Select(p => p.ToProductDTO(_unitOfWork)); 
        }
    }
}
