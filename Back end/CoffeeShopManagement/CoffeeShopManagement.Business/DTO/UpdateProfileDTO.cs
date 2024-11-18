using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.DTO
{
    public class UpdateProfileDTO
    {
        [Required(ErrorMessage = "UserName is required")]
        [MinLength(3, ErrorMessage = "UserName must be at least 3 characters long.")]
        [MaxLength(20, ErrorMessage = "UserName must be less than 20 characters long.")]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Address is required")]
        [MinLength(2, ErrorMessage = "Address must be at least 2 characters long.")]
        [MaxLength(30, ErrorMessage = "Address must be less than 30 characters long.")]
        public string? Address { get; set; }

        public IFormFile Avatar { get; set; }


    }
}
