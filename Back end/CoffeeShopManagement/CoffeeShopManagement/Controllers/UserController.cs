using Azure.Core;
using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.Helpers;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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

            if (!emailRegex.IsMatch(email))
            {
                return BadRequest("Invalid email");
            }

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
        [HttpPut("{id}/ChangePassword")]
        public async Task<IActionResult> ChangePassword(Guid id, ChangePasswordDTO changePassword)
        {
            var user = await _userService.Get(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }
            if (user.Password != changePassword.CurrentPassword)
            {
                return BadRequest(new { message = "Current password is incorrect." });
            }
            if (user.Password == changePassword.NewPassword)
            {
                return BadRequest(new { message = "The old password must be different from the new password." });
            }
            if (changePassword.NewPassword != changePassword.ConfirmNewPassword)
            {
                return BadRequest(new { message = "Confirm password does not match." });
            }

            user.Password = changePassword.NewPassword;
            await _userService.Update(user);

            return NoContent();
        }
        [HttpPut("{id}/UpdateProfile")]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateProfileDTO updateProfile)
        {
                var user = await _userService.Get(id);
                if (user == null)
                {
                    return NotFound();
                }

                user.UserName = updateProfile.UserName;
                user.PhoneNumber = updateProfile.PhoneNumber;
                user.Address = updateProfile.Address;

                if (updateProfile.Avatar != null)
                {
                    
                    var avatarDirectory = Path.Combine("wwwroot", "images");
                    if (!Directory.Exists(avatarDirectory))
                    {
                        Directory.CreateDirectory(avatarDirectory);
                    }

                    var avatarFileName = $"{Guid.NewGuid()}_{updateProfile.Avatar.FileName}";
                    var avatarPath = Path.Combine(avatarDirectory, avatarFileName);
                    using (var stream = new FileStream(avatarPath, FileMode.Create))
                    {
                        await updateProfile.Avatar.CopyToAsync(stream);
                    }
                    user.Avatar = avatarFileName;
                }

                await _userService.Update(user);

                return NoContent();
        }

    }

}