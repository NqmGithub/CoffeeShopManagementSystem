using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;


namespace CoffeeShopManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders(
     [FromQuery] string search = "",
     [FromQuery] string? status = null,
     [FromQuery] string? sortColumn = "OrderDate",
     [FromQuery] bool isDescending = false,
     [FromQuery] int pageNumber = 1,
     [FromQuery] int pageSize = 10)
        {
            int? statusEnum = null;
            if (!string.IsNullOrEmpty(status))
            {
                statusEnum = status switch
                {
                    "Pending" => 1,
                    "Completed" => 2,
                    "Cancelled" => 3,
                    _ => null
                };
            }

            // Lấy dữ liệu từ Service với tổng số lượng đơn hàng
            var (orders, totalOrders) = await _orderService.GetOrdersWithCount(search, statusEnum, sortColumn, isDescending, pageNumber, pageSize);

            // Trả về dữ liệu khớp với yêu cầu từ frontend
            return Ok(new
            {
                list = orders,
                total = totalOrders
            });
        }


        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> ToggleOrderStatus(Guid id)
        {
            var result = await _orderService.ToggleOrderStatus(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(new { Message = "Status updated successfully", NewStatus = result.NewStatus });
        }
       
        [HttpGet("customer/{id}")]
        public async Task<IActionResult> GetListContactByUserId(Guid id)
        {
            var orders = await _orderService.GetOrdersByUserId(id);
            return Ok(orders);
        }
    }
}
