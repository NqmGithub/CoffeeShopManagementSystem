﻿using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Data.RepositoryContracts
{
    public interface IOrderDetailRepository: IGenericRepository<OrderDetail>
    {
        Task AddAsync(OrderDetail orderDetail);  
        Task AddAsync(List<OrderDetail> orderDetails);
        Task<List<OrderDetail>> GetOrderDetailsByOrderId(Guid orderId);
        Task<bool> UpdateOrderDetails(Guid orderId, List<OrderDetail> orderDetails);
    }
}
