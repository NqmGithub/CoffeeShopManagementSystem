using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;
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

        [HttpPut("rating")]
        public async Task<IActionResult> Rating(RatingProductDTO[] list)
        {
            var result = await _orderDetailService.RatingProducts(list);
            return Ok(result);
        }
    }
}
