using CoffeeShopManagement.Business.Helpers;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

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
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("byEmail/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Invalid email");

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);

            if (!emailRegex.IsMatch(email)){
                return BadRequest("Invalid email");
            }

            var user = await _userService.GetByEmail(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

/*        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetPatientPgaination(int pageNumber, int pageSize)
        {
            var users = await _userService.GetPagination(pageNumber, pageSize);
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }*/

        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var users = await _userService.GetUserCount();
            return Ok(users);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUser([FromQuery] string keyword="", [FromQuery] string status = "all", int pageNumber = 1, int pageSize = 5)
        {
            var users = await _userService.SearchUser(keyword, status, pageNumber, pageSize);
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _userService.Add(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user); ;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            user.Password = PasswordHelper.HashPassword(user.Password);
            await _userService.Update(user);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.Delete(id);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchDelete(Guid id)
        {
            var user = await _userService.Get(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            user.Status = 2;
            await _userService.Update(user);

            return NoContent();
        }
    }
}
