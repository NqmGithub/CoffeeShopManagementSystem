using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Data.RepositoryContracts
{
    public interface IOtpRepository: IGenericRepository<Otp>
    {
        Task<Otp> GetByEmail(string email);
        Task Update(Otp otp);
        Task Add(Otp otp);
    }
}
