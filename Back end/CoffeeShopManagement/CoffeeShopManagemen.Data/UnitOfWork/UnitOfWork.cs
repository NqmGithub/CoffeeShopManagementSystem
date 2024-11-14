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
        private readonly IContactRepository _contactRepository;
        private readonly IProblemTypeRepository _problemTypeRepository;

        private readonly CoffeeShopDbContext _db;
        public UnitOfWork(CoffeeShopDbContext db, ICategoryRepository categoryRepository, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IProductRepository productRepository, IUserRepository userRepository, IContactRepository contactRepository, IProblemTypeRepository problemTypeRepository)
        {
            _db = db;
            _categoryRepository = categoryRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _contactRepository = contactRepository;
            _problemTypeRepository = problemTypeRepository;
        }
        public CoffeeShopDbContext Context => _db;

        public ICategoryRepository CategoryRepository => _categoryRepository;

        public IOrderRepository OrderRepository => _orderRepository;

        public IOrderDetailRepository OrderDetailRepository => _orderDetailRepository;

        public IProductRepository ProductRepository => _productRepository;

        public IUserRepository UserRepository => _userRepository;

        public IContactRepository ContactRepository => _contactRepository;

        public IProblemTypeRepository ProblemTypeRepository => _problemTypeRepository;

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
