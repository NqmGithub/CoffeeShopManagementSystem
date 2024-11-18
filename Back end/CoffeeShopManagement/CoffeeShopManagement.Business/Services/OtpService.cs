using CoffeeShopManagement.Business.Helpers;
using CoffeeShopManagement.Business.ServiceContracts;
using CoffeeShopManagement.Data.RepositoryContracts;
using CoffeeShopManagement.Data.UnitOfWork;
using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.Services
{
    public class OtpService : IOtpService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OtpService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
        public async Task<bool> SendOtp(string email)
        {
            var otp = GenerateOtp();
            var otpRecord = new Otp
            {
                Id = Guid.NewGuid(),
                Email = email,
                Otp1 = otp,
                ExpirationTime = DateTime.UtcNow.AddMinutes(5),
                Status = 0
            };
            
            await _unitOfWork.OtpRepository.Add(otpRecord);

            bool result = SendEmailHelper.SendEmailOTP(email, otp);

            return result;
        }

        public async Task<bool> ValidateOtp(string email, string otpcode)
        {
            var otp = await _unitOfWork.OtpRepository.GetByEmail(email);
            if(otp == null || otp.Status != 0 || otp.ExpirationTime < DateTime.UtcNow || otp.Otp1 != otpcode)
            {
                return false;
            }
            otp.Status = 1;
            await _unitOfWork.OtpRepository.Update(otp);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ResetPassword(string email)
        {
            var randomPassword = GenerateOtp() + GenerateOtp();
            var user = await _unitOfWork.UserRepository.GetByEmail(email);
            if(user == null)
            {
                return false;
            }
            user.Password = randomPassword;
            await _unitOfWork.UserRepository.Update(user);

            bool result = SendEmailHelper.SendEmailResetPassword(email, randomPassword);
            return result;
        }
    }
}
