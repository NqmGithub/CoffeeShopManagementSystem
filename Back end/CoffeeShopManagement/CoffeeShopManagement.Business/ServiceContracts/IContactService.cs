using CoffeeShopManagement.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.ServiceContracts
{
    public interface IContactService
    {
        Task<ContactDTO> GetContactById(Guid id);
        Task<ICollection<ContactDTO>> GetListContact();
        Task<bool> CreateContactAsync(CreateContactDTO createContactDTO);

        Task<bool> UpdateContactResponseAsync(Guid id, ContactResponseDTO contactResponseDTO);

        Task<bool> ChangeStatus(Guid id, string status);
        Task<ContactListResponse> GetContactWithCondition
            (ContactQueryRequest contactQueryRequest);

        Task<ICollection<ContactDTO>> GetListContactsByUserId(Guid id);
    }
}
