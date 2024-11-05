using CoffeeShopManagement.Data.Repositories;
using CoffeeShopManagement.Data.RepositoryContracts;
using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly CoffeeShopDbContext _db;

        public UnitOfWork(CoffeeShopDbContext db, ICategoryRepository categoryRepository, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _db = db;
            _categoryRepository = categoryRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }
        public CoffeeShopDbContext Context => _db;

        public IGenericRepository<Category> CategoryRepository => _categoryRepository;

        public IGenericRepository<Order> OrderRepository => _orderRepository;

        public IGenericRepository<OrderDetail> OrderDetailRepository => _orderDetailRepository;

        public IGenericRepository<Product> ProductRepository => _productRepository;

        public IGenericRepository<User> UserRepository => _userRepository;

        public int SaveChanges()
        {
            return _db.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
