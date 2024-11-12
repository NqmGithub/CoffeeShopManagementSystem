using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.DTO
{
    public class ProductUpdateDTO
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "ProductName is required")]
        public string ProductName { get; set; } = null!;

        [Required(ErrorMessage = "Category is required")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.1, int.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Thumbnail is required")]
        public string? Thumbnail { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }
    }
}
