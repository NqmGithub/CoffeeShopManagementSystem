using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CoffeeShopManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

<<<<<<< HEAD
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserProfile(Guid id)
        {
            var user = await _userService.GetById(id); // Giả sử bạn có phương thức GetById trong UserService
=======
        [HttpGet("users")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("byId/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.Get(id);
>>>>>>> main
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
<<<<<<< HEAD
=======

        [HttpGet("byEmail/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetByEmail(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetPatientPgaination(int pageNumber, int pageSize)
        {
            var users = await _userService.GetPagination(pageNumber, pageSize);
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            await _userService.Add(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user); ;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            await _userService.Update(user);

            return NoContent();
        }
>>>>>>> main
    }
}
