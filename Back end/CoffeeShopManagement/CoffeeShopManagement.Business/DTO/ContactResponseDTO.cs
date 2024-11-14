using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.DTO
{
    public class ContactResponseDTO
    {
        [Required(ErrorMessage = "ContactId is required")]
        public Guid ContactId { get; set; }
        public Guid AdminId { get; set; }

        [Required(ErrorMessage ="Response is required")]
        public string Response {  get; set; }
        public string Status { get; set; }
    }
}
