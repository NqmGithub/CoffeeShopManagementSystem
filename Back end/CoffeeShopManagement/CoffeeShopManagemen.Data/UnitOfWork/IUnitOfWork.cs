﻿using CoffeeShopManagement.Data.RepositoryContracts;
using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        CoffeeShopDbContext Context { get; }
        ICategoryRepository CategoryRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        IProductRepository ProductRepository { get; }
        IUserRepository UserRepository { get; }
        IContactRepository ContactRepository { get; }
        IProblemTypeRepository ProblemTypeRepository { get; }
        IOtpRepository OtpRepository { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
