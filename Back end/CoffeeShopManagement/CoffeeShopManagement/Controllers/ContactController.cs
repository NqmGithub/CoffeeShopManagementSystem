using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopManagement.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            var contacts = await _contactService.GetListContact();
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactById(Guid id)
        {
            var contact = await _contactService.GetContactById(id);
            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(CreateContactDTO createContactDTO)
        {
            var result = await _contactService.CreateContactAsync(createContactDTO);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContactResponse(Guid id,ContactResponseDTO contactResponseDTO)
        {
            var result = await _contactService.UpdateContactResponseAsync(id,contactResponseDTO);
            return Ok(result);
        }
    }
}
