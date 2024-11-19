using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShopManagement.Data.RepositoryContracts;
using CoffeeShopManagement.Models.Models;
using CoffeeShopManagement.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CoffeeShopManagement.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly CoffeeShopDbContext _context;

        public ProductRepository(CoffeeShopDbContext db) : base(db)
        {
            _context = db;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(
      string search,
      string category,
      decimal? minPrice,
      decimal? maxPrice,
      int page,
      int pageSize,
      SortBy sortBy,
      bool isDescending)
        {
            var query = _context.Products
                                .Include(p => p.Categoty)
                                .Where(p => p.Status==1)
                                .Where(p => p.Categoty.Status == 1)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.ProductName.Contains(search));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Categoty.CategoryName.Contains(category));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            query = sortBy switch
            {
                SortBy.Price => isDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                SortBy.Bestseller => isDescending ? query.OrderByDescending(p => p.OrderDetails.Sum(od => od.Quantity)) : query.OrderBy(p => p.OrderDetails.Sum(od => od.Quantity)),
                _ => isDescending ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id),
            };

            return await query.Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryId(Guid categoryId)
        {
            return await _context.Products
                .Include(p => p.Categoty)
                .Where(p => p.CategotyId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetTopBestsellersAsync(int top)
        {
            return await _context.Products
                .Include(p => p.Categoty)
                .Where(p => p.Categoty.Status == 1)
                .Where(p => p.Status == 1)
                .OrderByDescending(p => p.OrderDetails.Sum(od => od.Quantity))
                .Take(top)
                .ToListAsync();
        }
        public async Task<int> GetTotalRecords(string search, string category, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Products.AsQueryable();

            // Filter by search term, category, and price range
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.ProductName.Contains(search));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Categoty.CategoryName.Contains(category));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            // Only count products with status = 1 (active products)
            query = query.Where(p => p.Status == 1);

            return await query.CountAsync();
        }

    }
}

