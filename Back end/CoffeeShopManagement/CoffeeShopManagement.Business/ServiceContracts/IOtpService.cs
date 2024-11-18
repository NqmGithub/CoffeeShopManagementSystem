using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.ServiceContracts
{
    public interface IOtpService
    {
        Task<bool> SendOtp(string email);
        Task<bool> ResetPassword(string email);
        Task<bool> ValidateOtp(string email, string otp);
    }
}
