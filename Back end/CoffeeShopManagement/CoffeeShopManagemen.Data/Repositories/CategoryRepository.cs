
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShopManagement.Data.RepositoryContracts;
using CoffeeShopManagement.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopManagement.Data.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly CoffeeShopDbContext _context;
        public CategoryRepository(CoffeeShopDbContext db) : base(db)
        {
            _context = db;
        }

        public async Task<IEnumerable<Category>> GetListCategory()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task<Category> GetCategoryById(Guid id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => (c.Id == id));
        }
        public async Task AddCategory(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }
        public IQueryable<Category> GetAll()
        {
            return _context.Categories.AsQueryable();
        }

        public async Task DeleteCategory(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }    
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task<int> GetCategoryCount()
        {
            return await _context.Categories.CountAsync();
        }
        public async Task<IEnumerable<Category>> SearchCategory(string keyword, string status, int pageNumber, int pageSize)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.CategoryName.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(status))
            {
                if (status.ToLower() == "active")
                {
                    query = query.Where(c => c.Status == 1);
                }
                else if (status.ToLower() == "inactive")
                {
                    query = query.Where(c => c.Status != 1);
                }
            }

            return await query
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<int> GetCategoryCount(string keyword, string status)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.CategoryName.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(status))
            {
                if (status.ToLower() == "active")
                {
                    query = query.Where(c => c.Status == 1);
                }
                else if (status.ToLower() == "inactive")
                {
                    query = query.Where(c => c.Status != 1);
                }
            }

            return await query.CountAsync();
        }



    }

}



