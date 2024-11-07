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
            User existId = await _unitOfWork.UserRepository.GetById(user.Id);
            if(existId != null)
            {
                throw new ArgumentException("Already exist user with this id with username: " + existId.UserName);
            }

            User existEmail = await _unitOfWork.UserRepository.GetByEmail(user.Email);
            if (existId != null)
            {
                throw new ArgumentException("Already exist user with this email with username: " + existId.UserName);
            }

            user.Id = Guid.NewGuid();
            await _unitOfWork.UserRepository.Add(user);
        }

        public async Task Delete(Guid id)
        {
            var exist = _unitOfWork.UserRepository.GetById(id); 
            if(exist == null)
            {
                throw new ArgumentException($"No user with this id: {id}");
            }
            await _unitOfWork.UserRepository.Delete(id);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _unitOfWork.UserRepository.GetAll();
        }

        public async Task<User> Get(Guid id)
        {

            return await _unitOfWork.UserRepository.GetById(id);
        }

        public Task<User> GetByEmail(string email)
        {
            var user =  _unitOfWork.UserRepository.GetByEmail(email);
            if(user == null)
            {
                throw new ArgumentException($"The email {email} is invalid");
            }
            return user;
        }
<<<<<<< HEAD
        public Task<User> GetById(Guid id)
        {
            var user = _unitOfWork.UserRepository.GetById(id);
            if (user == null) { 
                throw new ArgumentException("User not found"); 
            }
            return user;
=======

        public async Task<IEnumerable<User>> GetPagination(int pageNumber, int pageSize)
        {
            if(pageNumber < 1 || pageSize < 1)
            {
                throw new ArgumentException("Page number and size must be greater than 0");
            }
            if(pageSize > 30)
            {
                throw new ArgumentException("Page size must not be too big (< 30)");
            }
            int totalCount = await _unitOfWork.UserRepository.GetUserCount();
            int maxPageNumber = (int)Math.Ceiling((double)totalCount / pageSize);
            if (pageNumber > maxPageNumber)
            {
                throw new ArgumentException("Page number is out of range.");
            }

            return await _unitOfWork.UserRepository.GetPagination(pageNumber, pageSize);
        }

        public async Task Update(User user)
        {
            await _unitOfWork.UserRepository.Update(user);
        }

        public async Task<int> GetUserCount()
        {
            return await _unitOfWork.UserRepository.GetUserCount();
>>>>>>> main
        }
    }
}
