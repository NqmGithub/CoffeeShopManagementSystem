using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeShopManagement.Models.Models;
using System;

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

        /// <summary>
        /// Get a list of orders with pagination and optional filters (search, status, etc.)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetOrders(
            [FromQuery] string search = "",
            [FromQuery] string? status = null,
            [FromQuery] string? sortColumn = "OrderDate",
            [FromQuery] bool isDescending = false,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                // Convert the status string to the corresponding enum value
                int? statusEnum = status switch
                {
                    "Pending" => 1,
                    "Completed" => 2,
                    "Cancelled" => 3,
                    _ => null // Default is null (all statuses)
                };

                // Fetch orders and total count from the service
                var (orders, totalOrders) = await _orderService.GetOrdersWithCount(search, statusEnum, sortColumn, isDescending, pageNumber, pageSize);

                if (orders == null || !orders.Any())
                {
                    return NotFound(new { Message = "No orders found matching the criteria." });
                }

                // Return the orders in a structured response
                return Ok(new
                {
                    list = orders,
                    total = totalOrders
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while processing your request.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Toggle the status of an order between Pending and Completed/Cancelled.
        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> ToggleOrderStatus(Guid id)
        {
            try
            {
                var result = await _orderService.ToggleOrderStatus(id);

                if (!result.IsSuccess)
                {
                    return BadRequest(new { Message = result.Message });
                }

                return Ok(new { Message = "Status updated successfully", NewStatus = result.NewStatus });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while updating the order status.", Error = ex.Message });
            }
        }


        /// <summary>
        /// Get a list of orders placed by a specific customer.
        /// </summary>
        [HttpGet("customer/{id}")]
        public async Task<IActionResult> GetOrdersByUserId(Guid id)
        {
            try
            {
                var orders = await _orderService.GetOrdersByUserId(id);

                if (orders == null || !orders.Any())
                {
                    return NotFound(new { Message = "No orders found for this user." });
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while retrieving the user's orders.", Error = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderCreateDTO orderCreateDTO)
        {
            try
            {
                // Validate input
                if (orderCreateDTO == null)
                {
                    return BadRequest(new { Message = "Order data is required." });
                }

                // Add order through service layer
                var orderDTO = await _orderService.AddOrderAsync(orderCreateDTO);

                if (orderDTO == null)
                {
                    return BadRequest(new { Message = "Order could not be created." });
                }

                // Return the newly created order with a 201 Created status
                return CreatedAtAction(nameof(GetOrders), new { id = orderDTO.Id }, orderDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while creating the order.", Error = ex.Message });
            }
        }

    }
}
