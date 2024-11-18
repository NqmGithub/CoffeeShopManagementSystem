using CoffeeShopManagement.Business.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("customer/{id}")]
        public async Task<IActionResult> GetListContactByUserId(Guid id)
        {
            var orders = await _orderService.GetOrdersByUserId(id);
            return Ok(orders);
        }
    }
}
