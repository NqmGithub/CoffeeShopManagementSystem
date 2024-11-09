using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.Helpers;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Data.Repositories;
using CoffeeShopManagement.Data.RepositoryContracts;
using CoffeeShopManagement.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using CoffeeShopManagement.Models.Enums;
using static CoffeeShopManagement.Models.Models.Product;



namespace CoffeeShopManagement.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;

        public ProductService(IUnitOfWork unitOfWork, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateProduct(ProductCreateDTO productCreateDTO)
        {
            if (productCreateDTO == null)
            {
                throw new ArgumentNullException(nameof(productCreateDTO));
            }
            ValidationHelper.ModelValidation(productCreateDTO);

            var product = productCreateDTO.ToProduct();
            product.CategoryId = ConvertToCategoryId(productCreateDTO.CategoryName);
            _unitOfWork.ProductRepository.Add(product);

            int result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> ChangeStatusProductById(Guid id, string choice)
        {
            var product = _unitOfWork.ProductRepository.GetById(id);
            if (product == null)
            {
                throw new ArgumentException(nameof(product));
            }
            product.Status = choice.Equals("Active", StringComparison.OrdinalIgnoreCase) ? 1 : 0;
            int result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public ICollection<ProductDTO> GetBestSeller()
        {
            var _context = _unitOfWork.Context;
            var topProducts = _unitOfWork.OrderDetailRepository.GetQuery()
                .Include(x => x.Order).Where(x => x.Order.Status != 3)
                .GroupBy(x => x.ProductId).Select(x => new
                {
                    ProductId = x.Key,
                    TotalQuantity = x.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.TotalQuantity).Take(3)
                .Join(
                    _context.Products,
                    x => x.ProductId,
                    y => y.Id,
                    (x, y) => y
                )
                .Select(x => x.ToProductDTO(_unitOfWork)).ToList();
            return topProducts;
        }

        public ICollection<ProductDTO> GetListProduct()
        {
            return _unitOfWork.ProductRepository.GetAll().Select(x => x.ToProductDTO(_unitOfWork)).ToList();
        }

        public async Task<ProductDTO> GetProductById(Guid id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            return product.ToProductDTO(_unitOfWork);
        }

        public async Task<bool> UpdateProduct(ProductUpdateDTO productUpdateDTO)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(productUpdateDTO.Id);

            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            product.Id = productUpdateDTO.Id;
            product.ProductName = productUpdateDTO.ProductName;
            product.CategoryId = ConvertToCategoryId(productUpdateDTO.CategoryName);
            product.Price = productUpdateDTO.Price;
            product.Quantity = productUpdateDTO.Quantity;
            product.Thumbnail = productUpdateDTO.Thumbnail;
            product.Status = ProductHelper.ConvertToStatusInt(productUpdateDTO.Status);

            int result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        private Guid ConvertToCategoryId(string categoryName)
        {
            var id = _unitOfWork.ProductRepository.GetQuery().Include(x => x.Category).Where(x => x.Category.CategoryName.Equals(categoryName)).Select(x => x.Id).FirstOrDefault();
            if (id == Guid.Empty)
            {
                throw new Exception("Category name does not exist");
            }
            return id;
        }
        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(
           string search,
           string category,
           decimal? minPrice,
           decimal? maxPrice,
           int page,
           int pageSize,
           SortBy sortBy,
           bool isDescending)
        {
            var products = await _unitOfWork.ProductRepository.GetAllProductsAsync(
                search, category, minPrice, maxPrice, page, pageSize, sortBy, isDescending);

            return products.Select(p => new ProductDTO
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                Quantity = p.Quantity,
                Thumbnail = p.Thumbnail,
                Status = p.Status,
                Description = p.Description,
                CategoryName = p.Category?.CategoryName ?? "Unknown"
            });
        }
    }
}