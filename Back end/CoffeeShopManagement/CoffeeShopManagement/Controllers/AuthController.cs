using CoffeeShopManagement.Business.Helpers;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace CoffeeShopManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOtpService _otpService;

        public AuthController(IUserService userService, IOtpService otpService)
        {
            _userService = userService;
            _otpService = otpService;
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

        [HttpPost("checkEmail")]
        public async Task<IActionResult> CheckEmail([FromBody] OTPRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest("Email is required.");
            }

            var existingUser = await _userService.GetByEmail(request.Email);
            if (existingUser != null)
            {
                return Conflict("A user with this email already exists.");
            }

            return Ok();
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

            var avatarDirectory = Path.Combine("wwwroot", "Avatars");
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                UserName = signupRequest.Username,
                Password = signupRequest.Password,
                Email = signupRequest.Email,
                Role = 1,
                PhoneNumber = signupRequest.PhoneNumber,
                Address = signupRequest.Address,
                Avatar = "avatar.jpg",
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


        public class OTPRequest
        {
            [Required] public string Email { get; set; }
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOTP([FromBody] OTPRequest request)
        {
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            Regex regex = new Regex(pattern);
            if (request.Email == null || !regex.IsMatch(request.Email))
            {
                throw new ArgumentNullException("Invalid email format or email empty");
            }

            try
            {
                bool result = await _otpService.SendOtp(request.Email);

                if (!result)
                {
                    return StatusCode(500, new { success = false, message = "Failed to send OTP. Try again later." });
                }

                return Ok(new { success = true, message = "OTP sent successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        public class ValidateOTPRequest
        {
            [Required] public string email { get; set; }
            [Required] public string otpCode { get; set; }
        }
        [HttpPost("validate-otp")]
        public async Task<IActionResult> ValidateOtp([FromBody] ValidateOTPRequest request)
        {
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            Regex regex = new Regex(pattern);
            if (request.email == null || !regex.IsMatch(request.email))
            {
                throw new ArgumentNullException("Invalid email format or email empty");
            }

            try
            {
                var isValid = await _otpService.ValidateOtp(request.email, request.otpCode);

                if (!isValid)
                {
                    return BadRequest(new { success = false, message = "Invalid or expired OTP." });
                }

                return Ok(new { success = true, message = "OTP validated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] OTPRequest request)
        {
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            Regex regex = new Regex(pattern);
            if (request.Email == null || !regex.IsMatch(request.Email))
            {
                throw new ArgumentNullException("Invalid email format or email empty");
            }

            bool result = await _otpService.ResetPassword(request.Email);

            return Ok(result);
        }
    }
}
