using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Data.UnitOfWork;
using CoffeeShopManagement.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.Services
{
    public class ProblemTypeService : IProblemTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProblemTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateProblemTypeAsync(ProblemTypeDTO problemType)
        {
            var p = new ProblemType
            {
                Id = problemType.Id,
                ProblemName = problemType.ProblemName,
                Status = problemType.Status,
            };
            _unitOfWork.ProblemTypeRepository.Add(p);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<ProblemTypeDTO?> GetProblemTypeById(Guid id)
        {
           var p = await _unitOfWork.ProblemTypeRepository.GetByIdAsync(id);
            return p.ToProblemTypeDTO();
        }

        public async Task<ICollection<ProblemTypeDTO>> GetListProblemTypes()
        {
            return await _unitOfWork.ProblemTypeRepository.GetQuery().Select(x => x.ToProblemTypeDTO()).ToListAsync();
        }

        public async Task<bool> UpdateProblemTypeAsync(Guid id, ProblemTypeDTO problemType)
        {
            if(id != problemType.Id)
            {
                throw new ArgumentException("Id diff");
            }
            var problem = _unitOfWork.ProblemTypeRepository.GetById(problemType.Id);
            problem.ProblemName = problemType.ProblemName;
            problem.Status = problemType.Status;    

            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }
    }
}
