using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Business.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace CoffeeShopManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly CartCookieService _cartService;

        public OrderDetailController(IOrderDetailService orderDetailService, CartCookieService cartService)
        {
            _orderDetailService = orderDetailService;
            _cartService = cartService;
        }

        // Lấy chi tiết đơn hàng
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderDetails(Guid orderId)
        {
            var details = await _orderDetailService.GetOrderDetails(orderId);
            return Ok(details);
        }

        // Lưu thay đổi và xóa cookie
        [HttpPost("save/{orderId}")]
        public async Task<IActionResult> SaveOrderDetails(Guid orderId)
        {
            var cartItems = _cartService.GetCartItems();

            var orderDetails = cartItems.Select(ci => new OrderDetailDTO
            {
                ProductId = ci.ProductId,
                ProductName = ci.ProductName,
                OrderPrice = ci.Price,
                Quantity = ci.Quantity
            }).ToList();

            var success = await _orderDetailService.SaveOrderDetails(orderId, orderDetails);

            if (success)
            {
                _cartService.ClearCart(); // Xóa cookie sau khi lưu
                return Ok("Order details updated successfully");
            }

            return StatusCode(500, "Failed to update order details");
        }
        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetListContactByUserId(Guid id)
        {
            var list = await _orderDetailService.GetListOrderDetailsByOrderId(id);
            return Ok(list);
        }
    }
}
