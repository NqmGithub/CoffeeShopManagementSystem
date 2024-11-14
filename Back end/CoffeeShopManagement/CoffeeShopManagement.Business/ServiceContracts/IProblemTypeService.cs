using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.ServiceContracts
{
    public interface IProblemTypeService
    {
        Task<ProblemTypeDTO?> GetProblemTypeById(Guid id);
        Task<ICollection<ProblemTypeDTO>> GetListProblemTypes();
        Task<bool> CreateProblemTypeAsync (ProblemTypeDTO problemType);

        Task<bool> UpdateProblemTypeAsync (Guid id, ProblemTypeDTO problemType);
    }
}
