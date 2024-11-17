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
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        private readonly CoffeeShopDbContext _context;
        public OrderDetailRepository(CoffeeShopDbContext db) : base(db)
        {
            _context = db;
        }
        public async Task<List<OrderDetail>> GetOrderDetailsByOrderId(Guid orderId)
        {
            return await _context.OrderDetails
                .Include(d => d.Product) // Load thông tin Product
                .Where(d => d.OrderId == orderId)
                .ToListAsync();
        }


        public async Task<bool> UpdateOrderDetails(Guid orderId, List<OrderDetail> orderDetails)
        {
            var existingOrder = await _context.Orders.Include(o => o.OrderDetails)
                                                     .FirstOrDefaultAsync(o => o.Id == orderId);

            if (existingOrder == null) return false;

            _context.OrderDetails.RemoveRange(existingOrder.OrderDetails);
            await _context.OrderDetails.AddRangeAsync(orderDetails);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
