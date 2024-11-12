using System;
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

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}

