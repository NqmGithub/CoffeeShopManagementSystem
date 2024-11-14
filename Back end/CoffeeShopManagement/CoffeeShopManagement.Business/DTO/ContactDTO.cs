using CoffeeShopManagement.Business.Helpers;
using CoffeeShopManagement.Data.UnitOfWork;
using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.DTO
{
    public class ContactDTO
    {
        public Guid Id { get; set; }

        public string CustomerName { get; set; }

        public string AdminName { get; set; }

        public DateTime SendDate { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public string ProblemName { get; set; }

        public string Status { get; set; }
    }

    public static class ContactExtension
    {
        public static ContactDTO ToContactDTO(this Contact contact, IUnitOfWork unitOfWork)
        {
            var customer =  unitOfWork.UserRepository.GetQuery().FirstOrDefault(x => x.Id == contact.CustomerId);
            if (customer == null) 
            {
                throw new ArgumentException(nameof(customer));
            }
            User? admin = null;
            if(contact.AdminId != Guid.Empty)
            {
                 admin = unitOfWork.UserRepository.GetQuery().FirstOrDefault(x => x.Id == contact.AdminId);
            }            
            if (admin == null)
            {
                throw new ArgumentException(nameof(admin));
            }

            var problem = unitOfWork.ProblemTypeRepository.GetById(contact.ProblemId);
            if (problem == null) 
            {
                throw new ArgumentException(nameof(problem));
            }

            return new ContactDTO
            {
                Id = contact.Id,
                CustomerName = customer.UserName,
                AdminName = admin.UserName,
                SendDate = contact.SendDate,
                Description = contact.Description,
                ProblemName = problem.ProblemName,
                Status = ContactHelper.ConvertToStatusString(contact.Status),
            };
        }
    }
}
