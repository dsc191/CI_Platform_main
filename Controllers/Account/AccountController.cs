using CI_Platform.entities.Models;
using CI_Platform.Entities.DataViewModel;
using CI_Platform.repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CI_Platform.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _registerInterface;

        public AccountController(ILogger<HomeController> logger, IAccountRepository accountInterface)
        {
            _registerInterface = accountInterface;
        }

        public IActionResult login()
        {
            return View();
        }

        public IActionResult forgotPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult registration()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult registration(RegistrationViewModel model)
        {
            try
            {
                User register = _registerInterface.RegistrationViewModel(model);
                if (register == null)
                    return StatusCode(HttpStatusCode.BadRequest.GetHashCode(), "Bad request");

                return StatusCode(HttpStatusCode.OK.GetHashCode(), register);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }
        public IActionResult resetPassword()
        {
            return View();
        }
    }
}