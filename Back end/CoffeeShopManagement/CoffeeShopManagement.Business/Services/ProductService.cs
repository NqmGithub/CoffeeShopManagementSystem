﻿using System;
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
using CoffeeShopManagement.Models.Models;
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
            product.CategotyId = ConvertToCategoryId(productCreateDTO.CategoryName);
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

        public ICollection<ProductDTO> GetListProduct()
        {
            var x = _unitOfWork.ProductRepository.GetQuery().Include(x => x.Categoty.Status == 1).Select(x => x.ToProductDTO(_unitOfWork)).ToList();
            foreach (var product in x)
            {
                var temp = _unitOfWork.OrderDetailRepository.GetQuery()
                    .Where(x => x.Rating > 0 && x.ProductId == product.Id);
                product.numberReviews = temp.Count();
                product.Rate = temp
                    .Select(x => x.Rating)
                    .DefaultIfEmpty(0)
                    .Average();

            }
            return x;
        }

        public async Task<ProductDTO> GetProductById(Guid id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            var productDTO =  product.ToProductDTO(_unitOfWork);
                var temp = _unitOfWork.OrderDetailRepository.GetQuery()
                    .Where(x => x.Rating.HasValue && x.Rating > 0 && x.ProductId == product.Id).ToList();

                productDTO.numberReviews = temp.Count();
                productDTO.Rate = temp
                    .Select(x => x.Rating)
                    .DefaultIfEmpty(0)
                    .Average();
            return productDTO;
        }

        public async Task<bool> UpdateProduct(ProductUpdateDTO productUpdateDTO)
        {
            ValidationHelper.ModelValidation(productUpdateDTO);
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(productUpdateDTO.Id);

            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            product.Id = productUpdateDTO.Id;
            product.ProductName = productUpdateDTO.ProductName;
            product.CategotyId = ConvertToCategoryId(productUpdateDTO.CategoryName);
            product.Price = productUpdateDTO.Price;
            product.Quantity = productUpdateDTO.Quantity;
            product.Description = productUpdateDTO.Description;
            product.Thumbnail = productUpdateDTO.Thumbnail;
            product.Status = ProductHelper.ConvertToStatusInt(productUpdateDTO.Status);

            int result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        private Guid ConvertToCategoryId(string categoryName)
        {
            var id = _unitOfWork.CategoryRepository.GetQuery().Where(x => x.CategoryName.Equals(categoryName)).Select(x => x.Id).FirstOrDefault();
            if (id == Guid.Empty)
            {
                throw new Exception("Category name does not exist");
            }
            return id;
        }

        public async Task<ProductListResponse> GetProductWithCondition(ProductQueryRequest productQueryRequest)
        {
            var query = _unitOfWork.ProductRepository.GetQuery().Include(x => x.Categoty).Where(x => x.Categoty.Status == 1).AsQueryable();

            // Apply search
            if (!string.IsNullOrEmpty(productQueryRequest.Search))
            {
                query = query.Where(p => p.ProductName.Contains(productQueryRequest.Search) || p.Categoty.CategoryName.Contains(productQueryRequest.Search));
            }

            // Apply filter
            if (!string.IsNullOrEmpty(productQueryRequest.FilterCategory))
            {
                query = query.Where(p=>p.Categoty.CategoryName.Contains(productQueryRequest.FilterCategory));
            }

            if (!string.IsNullOrEmpty(productQueryRequest.FilterStatus))
            {
                query = query.Where(p=> p.Status== ProductHelper.ConvertToStatusInt(productQueryRequest.FilterStatus));
            }
            // Apply sorting
            if (productQueryRequest.SortColumn.Equals("CategoryName"))
            {
                // Sort by CategoryName using the navigation property
                query = productQueryRequest.SortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderBy(p => p.Categoty.CategoryName)
                    : query.OrderByDescending(p => p.Categoty.CategoryName);
            }
            else
            {
                // Sort by the property in Product
                query = productQueryRequest.SortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderBy(p => EF.Property<object>(p, productQueryRequest.SortColumn))
                    : query.OrderByDescending(p => EF.Property<object>(p, productQueryRequest.SortColumn));
            }

            // Apply pagination
            var totalProducts = query.Count();
            var productDTOs = query.Skip(productQueryRequest.Page * productQueryRequest.PageSize).Take(productQueryRequest.PageSize)
                .Select(p => new ProductDTO()
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    CategoryName = p.Categoty.CategoryName,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Thumbnail = p.Thumbnail,
                    Description = p.Description,
                    Status = ProductHelper.ConvertToStatusString(p.Status),
                }).ToList();
            for (int i = 0; i < productDTOs.Count; i++)
            {
                var product = productDTOs[i];
                var temp = _unitOfWork.OrderDetailRepository.GetQuery()
                    .Where(x => x.Rating.HasValue && x.Rating > 0 && x.ProductId == product.Id).ToList();

                product.numberReviews = temp.Count();
                product.Rate = temp
                    .Select(x => x.Rating)
                    .DefaultIfEmpty(0)
                    .Average();
            }

            return new ProductListResponse()
            {
                List =  productDTOs,
                Total = totalProducts,
            };
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

            var productDTOs = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                Quantity = p.Quantity,
                Thumbnail = p.Thumbnail,
                Status = ProductHelper.ConvertToStatusString(p.Status),
                Description = p.Description,
                CategoryName = p.Categoty?.CategoryName ?? "Unknown"
            }).ToList();
            for (int i = 0; i < productDTOs.Count; i++)
            {
                var product = productDTOs[i];
                var temp = _unitOfWork.OrderDetailRepository.GetQuery()
                    .Where(x => x.Rating.HasValue && x.Rating > 0 && x.ProductId == product.Id).ToList();

                product.numberReviews = temp.Count();
                product.Rate = temp
                    .Select(x => x.Rating)
                    .DefaultIfEmpty(0)
                    .Average();
            }
            return productDTOs;
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(Guid categoryId)
        {
            var products = await _unitOfWork.ProductRepository.GetProductsByCategoryId(categoryId);

            var productDTOs = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                CategoryName = p.Categoty?.CategoryName ?? "Unknown",
                Thumbnail = p.Thumbnail,
                Status = ProductHelper.ConvertToStatusString(p.Status),
                Description = p.Description
            }).ToList();
            for (int i = 0; i < productDTOs.Count; i++)
            {
                var product = productDTOs[i];
                var temp = _unitOfWork.OrderDetailRepository.GetQuery()
                    .Where(x => x.Rating.HasValue && x.Rating > 0 && x.ProductId == product.Id).ToList();

                product.numberReviews = temp.Count();
                product.Rate = temp
                    .Select(x => x.Rating)
                    .DefaultIfEmpty(0)
                    .Average();
            }
            return productDTOs;
        }

        public async Task<IEnumerable<ProductDTO>> GetTopBestsellerProductsAsync(int top)
        {
            var products = await _unitOfWork.ProductRepository.GetTopBestsellersAsync(top);

            // Chuyển sang List để thay đổi giá trị trong danh sách
            var productDTOs = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                CategoryName = p.Categoty?.CategoryName ?? "Unknown",
                Thumbnail = p.Thumbnail,
                Status = ProductHelper.ConvertToStatusString(p.Status),
                Description = p.Description
            }).ToList(); 

            for (int i = 0; i < productDTOs.Count; i++)
            {
                var product = productDTOs[i];
                var temp = _unitOfWork.OrderDetailRepository.GetQuery()
                    .Where(x => x.Rating.HasValue && x.Rating > 0 && x.ProductId == product.Id).ToList();

                product.numberReviews = temp.Count();
                product.Rate = temp
                    .Select(x => x.Rating)
                    .DefaultIfEmpty(0)
                    .Average();
            }

            return productDTOs; 
        }
        public async Task<int> GetTotalRecords(string search, string category, decimal? minPrice, decimal? maxPrice)
        {
            return await _unitOfWork.ProductRepository.GetTotalRecords(search,category,minPrice,maxPrice);
        }
        public async Task<bool> CheckProductNameExist(string productName)
        {
            var temp = await _unitOfWork.ProductRepository.GetQuery().FirstOrDefaultAsync(x => x.ProductName.Equals(productName));
            if (temp != null) 
            {
                return true;
            }
            return false;
        }
    }
}