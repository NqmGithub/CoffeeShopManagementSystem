using CoffeeShopManagement.Data.RepositoryContracts;
using CoffeeShopManagement.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Data.Repositories
{
    public class OtpRepository : GenericRepository<Otp>, IOtpRepository
    {
        private readonly CoffeeShopDbContext _context;

        public OtpRepository(CoffeeShopDbContext db): base(db)
        {
            _context = db;
        }

        public async Task<Otp> GetByEmail(string email)
        {
            return await _context.Otps
                .Where(o => o.Email == email && o.Status == 0)
                .OrderByDescending(o => o.ExpirationTime)
                .FirstOrDefaultAsync();
        }

        public async Task Update(Otp otp)
        {
            _context.Otps.Update(otp);
            await _context.SaveChangesAsync();
        }

        public async Task Add(Otp otp)
        {
            await _context.Otps.AddAsync(otp);
            await _context.SaveChangesAsync();
        }
    }
}
