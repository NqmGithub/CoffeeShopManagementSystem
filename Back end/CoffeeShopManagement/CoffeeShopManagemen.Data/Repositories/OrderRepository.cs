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
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly CoffeeShopDbContext _context;
        public OrderRepository(CoffeeShopDbContext db) : base(db)
        {
            _context = db;
        }
        public async Task<IEnumerable<Order>> GetOrders(
     string? search,
     int? status,
     string? sortColumn,
     bool isDescending,
     int pageNumber,
     int pageSize)
        {
            IQueryable<Order> query = _context.Orders.Include(o => o.User).Include(o => o.OrderDetails);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(o => o.User.UserName.Contains(search));
            }

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            switch (sortColumn?.ToLower())
            {
                case "totalprice":
                    query = isDescending ? query.OrderByDescending(o => o.OrderDetails.Sum(od => od.OrderPrice * od.Quantity))
                                          : query.OrderBy(o => o.OrderDetails.Sum(od => od.OrderPrice * od.Quantity));
                    break;
                case "totalquantity":
                    query = isDescending ? query.OrderByDescending(o => o.OrderDetails.Sum(od => od.Quantity))
                                          : query.OrderBy(o => o.OrderDetails.Sum(od => od.Quantity));
                    break;
                case "status":
                    query = isDescending ? query.OrderByDescending(o => o.Status) : query.OrderBy(o => o.Status);
                    break;
                default:
                    query = isDescending ? query.OrderByDescending(o => o.OrderDate) : query.OrderBy(o => o.OrderDate);
                    break;
            }

            return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<int> GetTotalRecords(string? search, int? status)
        {
            var query = _context.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(o => o.User.UserName.Contains(search));
            }

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            return await query.CountAsync();
        }

        public async Task<Order?> GetOrderById(Guid id)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
