using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Business.Services;
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

        [HttpGet("all")]
        public async Task<IActionResult> GetContacts()
        {
            var contacts = await _contactService.GetListContact();
            return Ok(contacts);
        }
        [HttpGet("customer/{id}")]
        public async Task<IActionResult> GetListContactByUserId(Guid id)
        {
            var contact = await _contactService.GetListContactsByUserId(id);
            return Ok(contact);
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

        [HttpPut("{id}/update-status")]
        public async Task<IActionResult> UpdateStatus(Guid id, string status)
        {
            var result = await _contactService.ChangeStatus(id, status);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContactResponse(Guid id,ContactResponseDTO contactResponseDTO)
        {
            var result = await _contactService.UpdateContactResponseAsync(id,contactResponseDTO);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetListContactss(string search = "",
            string filterStatus = "",
             int page = 0, int pageSize = 6,
           string sortColumn = "SendDate",
            string sortDirection = "desc")
        {
            //result include: list products and totalProducts after filter,sort, pagination
            var contactQueryRequest = new ContactQueryRequest()
            {
                Search = search,
                FilterStatus = filterStatus,
                Page = page,
                PageSize = pageSize,
                SortColumn = sortColumn,
                SortDirection = sortDirection
            };
            var result = await _contactService.GetContactWithCondition(contactQueryRequest);
            return Ok(result);
        }
    }
}
