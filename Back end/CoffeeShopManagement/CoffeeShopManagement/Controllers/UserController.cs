using CoffeeShopManagement.Business.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopManagement.WebAPI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
    }
}
