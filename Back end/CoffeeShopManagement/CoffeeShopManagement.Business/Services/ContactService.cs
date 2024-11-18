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
                    var folderName = Path.Combine("wwwroot", "Contacts");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var fullPath = Path.Combine(pathToSave, fileName);

                    await System.IO.File.WriteAllBytesAsync(fullPath, imageData);

                    var scheme = _httpContextAccessor.HttpContext.Request.Scheme;
                    var host = _httpContextAccessor.HttpContext.Request.Host;

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
            return await _unitOfWork.ContactRepository.GetQuery()
       .Include(x => x.Customer)
       .Include(x => x.Admin)
       .Include(x => x.Problem)
       .Select(x => new ContactDTO
       {
           Id = x.Id,
           Customer = new UserContactDTO
           {
               UserName = x.Customer.UserName,
               Avatar = x.Customer.Avatar
           },
           AdminName = x.Admin != null ? x.Admin.UserName : null,
           SendDate = x.SendDate,
           Subject = x.Subject,
           Description = x.Description,
           ProblemName = x.Problem.ProblemName,
           Response = x.Response,
           Status = ContactHelper.ConvertToStatusString(x.Status)
       }).OrderByDescending(x => x.SendDate)
       .ToListAsync();
        }

        public async Task<ContactDTO> GetContactById(Guid id)
        {
            return await _unitOfWork.ContactRepository.GetQuery()
                .Include(x => x.Customer)
                .Include(x => x.Admin)
                .Include(x => x.Problem)
                .Where(x => x.Id == id)
                .Select(x => new ContactDTO
                {
                    Id = x.Id,
                    Customer = new UserContactDTO
                    {
                        UserName = x.Customer.UserName,
                        Avatar = x.Customer.Avatar
                    },
                    AdminName = x.Admin != null ? x.Admin.UserName : null,
                    SendDate = x.SendDate,
                    Subject = x.Subject,
                    Description = x.Description,
                    ProblemName = x.Problem.ProblemName,
                    Response = x.Response,
                    Status = ContactHelper.ConvertToStatusString(x.Status)
                })
                .FirstOrDefaultAsync(); 
        }


        public async Task<bool> UpdateContactResponseAsync(Guid id, ContactResponseDTO contactResponseDTO)
    {
        if (id != contactResponseDTO.ContactId)
        {
            throw new ArgumentException("Id diff");
        }
        var contact = _unitOfWork.ContactRepository.GetById(contactResponseDTO.ContactId);
        if (contact == null)
        {
            throw new ArgumentException(nameof(contact));
        }
        contact.AdminId = contactResponseDTO.AdminId;
        contact.Response = await formatContent(contactResponseDTO.Response);
        contact.Status = ContactHelper.ConvertToStatusInt(contactResponseDTO.Status);

        var result = await _unitOfWork.SaveChangesAsync();
        return result > 0;
    }

    public async Task<ContactListResponse> GetContactWithCondition(ContactQueryRequest contactQueryRequest)
    {
        var query = _unitOfWork.ContactRepository.GetQuery().Include(x => x.Problem).Include(x => x.Customer).Where(x => x.Problem.Status == 1);

        // Apply search
        if (!string.IsNullOrEmpty(contactQueryRequest.Search))
        {
            query = query.Where(p => p.Subject.Contains(contactQueryRequest.Search) || p.Description.Contains(contactQueryRequest.Search)
            || p.Problem.ProblemName.Contains(contactQueryRequest.Search));
        }

        // Apply filter

        if (!string.IsNullOrEmpty(contactQueryRequest.FilterStatus))
        {
            query = query.Where(p => p.Status == ContactHelper.ConvertToStatusInt(contactQueryRequest.FilterStatus));
        }
        // Apply sorting
        if (contactQueryRequest.SortColumn.Equals("ProblemName"))
        {
            // Sort by CategoryName using the navigation property
            query = contactQueryRequest.SortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase)
                ? query.OrderBy(p => p.Problem.ProblemName)
                : query.OrderByDescending(p => p.Problem.ProblemName);
        }
        else if (contactQueryRequest.SortColumn.Equals("CustomerName"))
        {
            query = contactQueryRequest.SortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase)
                ? query.OrderBy(p => p.Customer.UserName)
                : query.OrderByDescending(p => p.Customer.UserName);
        }
        else
        {
            // Sort by the property in Product
            query = contactQueryRequest.SortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase)
                ? query.OrderBy(p => EF.Property<object>(p, contactQueryRequest.SortColumn))
                : query.OrderByDescending(p => EF.Property<object>(p, contactQueryRequest.SortColumn));
        }

        // Apply pagination
        var totalContacts = query.Count();
        var contacts = query.Skip(contactQueryRequest.Page * contactQueryRequest.PageSize).Take(contactQueryRequest.PageSize)
            .Select(p => new ContactDTO()
            {
                Id = p.Id,
                AdminName = p.AdminId != null ? _unitOfWork.UserRepository.GetQuery().First(x => x.Id == p.AdminId).UserName : null,
                Customer = new UserContactDTO
                {
                    UserName = _unitOfWork.UserRepository.GetQuery().First(x => x.Id == p.CustomerId).UserName,
                    Avatar = _unitOfWork.UserRepository.GetQuery().First(x => x.Id == p.CustomerId).Avatar
                },
                ProblemName = _unitOfWork.ProblemTypeRepository.GetQuery().First(x => x.Id == p.ProblemId).ProblemName,
                Description = p.Description,
                SendDate = p.SendDate,
                Subject = p.Subject,
                Status = ContactHelper.ConvertToStatusString(p.Status),
            }).ToListAsync();

        return new ContactListResponse()
        {
            List = await contacts,
            Total = totalContacts,
        };
    }

        public async Task<bool> ChangeStatus(Guid id, string status)
        {
            var contact = _unitOfWork.ContactRepository.GetById(id);
            contact.Status = ContactHelper.ConvertToStatusInt(status);

            var result = await _unitOfWork.SaveChangesAsync();
            return result>0;
        }

        public async Task<ICollection<ContactDTO>> GetListContactsByUserId(Guid id)
        {
            return await _unitOfWork.ContactRepository.GetQuery().Where(x => x.CustomerId == id)
       .Include(x => x.Customer)
       .Include(x => x.Admin)
       .Include(x => x.Problem)
       .Select(x => new ContactDTO
       {
           Id = x.Id,
           Customer = new UserContactDTO
           {
               UserName = x.Customer.UserName,
               Avatar = x.Customer.Avatar
           },
           AdminName = x.Admin != null ? x.Admin.UserName : null,
           SendDate = x.SendDate,
           Subject = x.Subject,
           Description = x.Description,
           ProblemName = x.Problem.ProblemName,
           Response = x.Response,
           Status = ContactHelper.ConvertToStatusString(x.Status)
       }).OrderByDescending(x => x.SendDate)
       .ToListAsync();
        }
    }
}
