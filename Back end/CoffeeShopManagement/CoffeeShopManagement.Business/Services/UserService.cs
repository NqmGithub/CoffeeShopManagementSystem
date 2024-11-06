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

        public Task<User> GetByEmail(string email)
        {
            var user =  _unitOfWork.UserRepository.GetByEmail(email);
            if(user == null)
            {
                throw new ArgumentException("The email is invalid");
            }
            return user;
        }
        public Task<User> GetById(Guid id)
        {
            var user = _unitOfWork.UserRepository.GetById(id);
            if (user == null) { 
                throw new ArgumentException("User not found"); 
            }
            return user;
        }
    }
}
