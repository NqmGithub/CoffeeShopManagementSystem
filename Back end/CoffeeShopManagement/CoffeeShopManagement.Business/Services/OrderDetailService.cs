using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Data.RepositoryContracts;
using CoffeeShopManagement.Models.Models;
using CoffeeShopManagement.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopManagement.Business.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderDetailRepository _repository;

        public OrderDetailService(IOrderDetailRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

       public async Task<List<OrderDetailDTO>> GetOrderDetails(Guid orderId)
        {
            var details = await _repository.GetOrderDetailsByOrderId(orderId);
            return details.Select(d => new OrderDetailDTO
            {
                ProductId = d.ProductId,
                ProductName = d.Product != null ? d.Product.ProductName : "Unknown Product",
                OrderPrice = d.OrderPrice,
                Quantity = d.Quantity
            }).ToList();
        }

        public async Task<bool> SaveOrderDetails(Guid orderId, List<OrderDetailDTO> orderDetails)
        {
            var updatedDetails = orderDetails.Select(d => new OrderDetail
            {
                OrderId = orderId,
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                OrderPrice = d.OrderPrice,
            }).ToList();

            return await _repository.UpdateOrderDetails(orderId, updatedDetails);
        }
              
        public async Task<ICollection<OrderDetailDTO>> GetListOrderDetailsByOrderId(Guid id)
        {
            var x = _unitOfWork.OrderDetailRepository.GetQuery().Where(x => x.OrderId == id);
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
                    Rating = item.Rating.Value
                });
            }
            return result;
        }

        public async Task<bool> RatingProducts(RatingProductDTO[] list)
        {
            foreach (RatingProductDTO ratingProduct in list) 
            {
                var orderDetail = _unitOfWork.OrderDetailRepository.GetById(ratingProduct.Id);
                if(orderDetail == null)
                {
                    throw new ArgumentNullException();
                }
                orderDetail.Rating = ratingProduct.Rating;
                
            }
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }
    }
}
