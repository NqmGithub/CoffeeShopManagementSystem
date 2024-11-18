using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopManagement.Business.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ICollection<OrderDetailDTO>> GetListOrderDetailsByOrderId(Guid id)
        {
            var list = _unitOfWork.OrderDetailRepository.GetQuery().Where(x => x.OrderId == id).ToList();

            var result = new List<OrderDetailDTO>();
            foreach (var item in list) 
            {
                var product = _unitOfWork.ProductRepository.GetQuery().First(x => x.Id == item.ProductId);
                result.Add(new OrderDetailDTO()
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    ProductName = product.ProductName,
                    Img = product.Thumbnail,
                    OrderPrice = item.OrderPrice,
                    Quantity = item.Quantity,
                });
            }
            return result;
        }
    }
}
