using CoffeeShopManagement.Data.RepositoryContracts;
using CoffeeShopManagement.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Data.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly CoffeeShopDbContext _context;

        public OrderRepository(CoffeeShopDbContext db) : base(db)
        {
            _context = db;
        }

        // Get Orders with filters, sorting, and pagination
        public async Task<(List<Order>, int)> GetOrders(
            string search,
            int? status,
            string sortColumn,
            bool isDescending,
            int pageNumber,
            int pageSize)
        {
            var query = _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product) // Include Product details in OrderDetails
                .Include(o => o.User) // Include User info in Orders
                .AsQueryable();

            // Filter by search term (UserName)
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(o => o.User.UserName.Contains(search));
            }

            // Filter by status if provided
            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            // Sorting logic based on the sortColumn and direction
            query = sortColumn switch
            {
                "OrderDate" => isDescending
                    ? query.OrderByDescending(o => o.OrderDate)
                    : query.OrderBy(o => o.OrderDate),
                "TotalPrice" => isDescending
                    ? query.OrderByDescending(o => o.OrderDetails.Sum(od => od.OrderPrice * od.Quantity))
                    : query.OrderBy(o => o.OrderDetails.Sum(od => od.OrderPrice * od.Quantity)),
                "TotalQuantity" => isDescending
                    ? query.OrderByDescending(o => o.OrderDetails.Sum(od => od.Quantity))
                    : query.OrderBy(o => o.OrderDetails.Sum(od => od.Quantity)),
                "Status" => isDescending
                    ? query.OrderByDescending(o => o.Status)
                    : query.OrderBy(o => o.Status),
                "OrderId" => isDescending
                    ? query.OrderByDescending(o => o.Id)
                    : query.OrderBy(o => o.Id),
                "UserName" => isDescending
                    ? query.OrderByDescending(o => o.User.UserName)
                    : query.OrderBy(o => o.User.UserName),
                _ => query.OrderByDescending(o => o.OrderDate) // Default to OrderDate if no match
            };

            // Get the total count of orders
            var totalCount = await query.CountAsync();

            // Paginate the query result
            var paginatedOrders = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (paginatedOrders, totalCount);
        }

        // Get Order by ID
        public async Task<Order?> GetOrderById(Guid id)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product) // Include Product details in OrderDetails
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        // Update an existing order
        public async Task UpdateOrder(Order order)
        {
            var existingOrder = await _context.Orders.FindAsync(order.Id);
            if (existingOrder != null)
            {
                existingOrder.Status = order.Status;
                _context.Orders.Update(existingOrder);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync(); // Commit the changes
        }

    }
}
