using CoffeeShopManagement.Business.DTO;
using CoffeeShopManagement.Business.Helpers;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Data.UnitOfWork;
using CoffeeShopManagement.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.Services
{
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public ContactService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> CreateContactAsync(CreateContactDTO createContactDTO)
        {
            var customerId = (await _unitOfWork.UserRepository.GetQuery().FirstOrDefaultAsync(x => x.Email.Equals(createContactDTO.Email))).Id;
            var problemId = await _unitOfWork.ProblemTypeRepository.GetQuery().Where(x => x.ProblemName.Equals(createContactDTO.ProblemName)).Select(x => x.Id).FirstOrDefaultAsync();
            if (problemId == Guid.Empty)
            {
                throw new ArgumentException(nameof(problemId));
            }
            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                SendDate = createContactDTO.SendDate,
                Subject = createContactDTO.Subject,
                ProblemId = problemId,
                Description = await formatContent(createContactDTO.Content),
            };
            _unitOfWork.ContactRepository.Add(contact);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        private async Task<string> formatContent(string content)
        {
            var regex = new Regex(@"data:image/(?<type>[a-zA-Z]+);base64,(?<data>[a-zA-Z0-9+/=]+)");

            var matches = regex.Matches(content);
            foreach (Match match in matches)
            {
                try
                {
                    var base64Data = match.Groups["data"].Value;
                    var imageType = match.Groups["type"].Value;
                    var imageData = Convert.FromBase64String(base64Data);

                    var fileName = $"{Guid.NewGuid()}.{imageType}";
                    var folderName = Path.Combine("Resources", "Contacts");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var fullPath = Path.Combine(pathToSave, fileName);

                    await System.IO.File.WriteAllBytesAsync(fullPath, imageData);

                    var scheme = _httpContextAccessor.HttpContext.Request.Scheme;
                    var host =  _httpContextAccessor.HttpContext.Request.Host;

                    var imageUrl = $"{scheme}://{host}/Resources/Contacts/{fileName}";

                    content = content.Replace(match.Value, imageUrl);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


            return content;
        }

        public async Task<ICollection<ContactDTO>> GetListContact()
        {
            return await _unitOfWork.ContactRepository.GetQuery().Select(x => x.ToContactDTO(_unitOfWork)).ToListAsync();
        }

        public async Task<ContactDTO> GetContactById(Guid id)
        {
            return (await _unitOfWork.ContactRepository.GetByIdAsync(id)).ToContactDTO(_unitOfWork);
        }

        public async Task<bool> UpdateContactResponseAsync(Guid id, ContactResponseDTO contactResponseDTO)
        {
            if(id != contactResponseDTO.ContactId)
            {
                throw new ArgumentException("Id diff");
            }
            var contact = _unitOfWork.ContactRepository.GetById(contactResponseDTO.ContactId);
            if (contact == null) 
            {
                throw new ArgumentException(nameof(contact));
            }
            contact.AdminId = contactResponseDTO.AdminId;
            contact.Response = contactResponseDTO.Response;
            contact.Status  = ContactHelper.ConvertToStatusInt(contactResponseDTO.Status);

            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }
    }
}
