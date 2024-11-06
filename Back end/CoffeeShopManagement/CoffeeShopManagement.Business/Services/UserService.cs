using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Data.UnitOfWork;
using CoffeeShopManagement.Models.Models;

namespace CoffeeShopManagement.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(User user)
        {
            user.Id = Guid.NewGuid();
            await _unitOfWork.UserRepository.Add(user);
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByEmail(string email)
        {
            var user =  _unitOfWork.UserRepository.GetByEmail(email);
            if(user == null)
            {
                throw new ArgumentException("The email is invalid");
            }
            return user;
        }

        public Task<IEnumerable<User>> GetPagination(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
