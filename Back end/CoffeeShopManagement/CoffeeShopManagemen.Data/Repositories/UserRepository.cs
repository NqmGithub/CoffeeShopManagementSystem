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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly CoffeeShopDbContext _context;
        public UserRepository(CoffeeShopDbContext db) : base(db)
        {
            this._context = db;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task Add(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var customer = await _context.Users.FindAsync(id);
            if (customer != null)
            {
                _context.Users.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetById(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => (u.Id == id));
        }

        public async Task<IEnumerable<User>> GetPagination(int pageNumber, int pageSize)
        {
            return await _context.Users.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();
        }

        public async Task Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> UpdateProfile(User user)
        {
            _context.Users.Update(user);
            var resuilt = await _context.SaveChangesAsync();
            return resuilt > 0;
        }
        public async Task<int> GetUserCount()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<IEnumerable<User>> SearchUser(string keyword, string Status, int pageNumber, int pageSize)
        {
            var list = _context.Users.
                Where(u => (u.UserName.Contains(keyword) || u.Email.Contains(keyword)));
            if(Status == "active")
            {
                list = list.Where(u => u.Status == 1);
            }
            if(Status == "inactive")
            {
                list = list.Where(u => u.Status != 1);
            }
            return await list.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();
        }
    }
}
