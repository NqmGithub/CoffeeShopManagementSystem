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
        public async Task<(List<Order>, int)> GetOrders(
            string search,
            int? status,
            string sortColumn,
            bool isDescending,
            int pageNumber,
            int pageSize)
        {
            var query = _context.Orders
                .Include(o => o.OrderDetails) // Nạp các OrderDetails
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product) // Nạp thông tin sản phẩm
                .Include(o => o.User) // Nạp thông tin User
                .AsQueryable();

            // Lọc theo search
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(o => o.User.UserName.Contains(search));
            }

            // Lọc theo status
            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            // Thực hiện sắp xếp dựa trên cột được chọn
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

            // Tổng số lượng đơn hàng
            var totalCount = await query.CountAsync();

            // Phân trang và lấy dữ liệu
            var paginatedOrders = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (paginatedOrders, totalCount);
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
