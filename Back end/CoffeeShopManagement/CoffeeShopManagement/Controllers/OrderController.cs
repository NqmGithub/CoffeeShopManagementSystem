using CoffeeShopManagement.Business.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController: ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders(
            [FromQuery] string? search = "",
            [FromQuery] int? status = null,
            [FromQuery] string? sortColumn = "OrderDate",
            [FromQuery] bool isDescending = false,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var (orders, totalRecords) = await _orderService.GetOrders(search, status, sortColumn, isDescending, pageNumber, pageSize);
            return Ok(new
            {
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
                Data = orders
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
    }
}
