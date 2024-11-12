using CoffeeShopManagement.Business.Helpers;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoffeeShopManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = (await _userService.GetByEmail(loginRequest.Email));
            if (user == null || user.Status != 1 || !PasswordHelper.VerifyPassword(loginRequest.Password, user.Password))
            {
                return Unauthorized("Invalid email or password");
            }
            var token = GenerateJwtToken(user.Id, user.Role);
            return Ok(new { token });
        }

        public class SignupRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Address { get; set; }
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest signupRequest)
        {
            if (string.IsNullOrWhiteSpace(signupRequest.Username) ||
                string.IsNullOrWhiteSpace(signupRequest.Password) ||
                string.IsNullOrWhiteSpace(signupRequest.Email) ||
                string.IsNullOrWhiteSpace(signupRequest.PhoneNumber) ||
                string.IsNullOrWhiteSpace(signupRequest.Address))
            {
                return BadRequest("All fields are required.");
            }

            var existingUser = await _userService.GetByEmail(signupRequest.Email);
            if (existingUser != null)
            {
                return Conflict("A user with this email already exists.");
            }

            var newUser = new User
            {
                Id = Guid.NewGuid(), 
                UserName = signupRequest.Username,
                Password = signupRequest.Password, 
                Email = signupRequest.Email,
                Role = 1,
                PhoneNumber = signupRequest.PhoneNumber,
                Address = signupRequest.Address,
                Avatar = @"..\..\avatar.jpg",
                Status = 1
            };

            await _userService.Add(newUser);

            var token = GenerateJwtToken(newUser.Id, newUser.Role);

            return Ok(new { token });
        }

        private string GenerateJwtToken(Guid userId, int role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("role", (role == 1 ? "customer" : "admin"))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my16characterkeymy16characterkey"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "http://localhost:44344",
                audience: "http://localhost:4200",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
