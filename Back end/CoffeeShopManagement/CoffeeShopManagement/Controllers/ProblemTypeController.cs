using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemTypeController : ControllerBase
    {
        private readonly IProblemTypeService _problemTypeService;
        public ProblemTypeController(IProblemTypeService problemTypeService)
        {
            _problemTypeService = problemTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProblemTypes()
        {
            var problems = await _problemTypeService.GetListProblemTypes();
            return Ok(problems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProblemTypeById(Guid id)
        {
            var problem = await _problemTypeService.GetProblemTypeById(id);
            return Ok(problem);
        }

        [HttpPost]
        public async Task<IActionResult> AddProblemType(ProblemTypeDTO problemType)
        {
            var result = await _problemTypeService.CreateProblemTypeAsync(problemType);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProblemType(Guid id, ProblemTypeDTO problemType)
        {
            var result = await _problemTypeService.UpdateProblemTypeAsync(id, problemType);
            return Ok(result);
        }
    }
}
