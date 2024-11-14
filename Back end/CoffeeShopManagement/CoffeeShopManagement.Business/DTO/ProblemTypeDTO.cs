using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.DTO
{
    public class ProblemTypeDTO
    {
        [Required(ErrorMessage ="Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Problem is required")]
        public string ProblemName { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public int Status { get; set; }
    }

    public static class ProblemTypeExtensions
    {
        public static ProblemTypeDTO ToProblemTypeDTO(this ProblemType problemType)
        {
            return new ProblemTypeDTO
            {
                Id = problemType.Id,
                ProblemName = problemType.ProblemName,
                Status = problemType.Status,
            };
        }
    }
}
