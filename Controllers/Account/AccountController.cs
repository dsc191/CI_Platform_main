using CI_Platform.entities.Models;
using CI_Platform.entities.ViewDataModel;
using CI_Platform.Entities.DataViewModel;
using CI_Platform.Helpers;
using CI_Platform.repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CI_Platform.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _registerInterface;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration configuration;


        public AccountController(ILogger<HomeController> logger, IAccountRepository accountInterface, IHttpContextAccessor httpContextAccessor,
                              IConfiguration _configuration)
        {
            _registerInterface = accountInterface;
            _httpContextAccessor = httpContextAccessor;
            configuration = _configuration;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            try
            {
                User user = _registerInterface.LoginViewModel(model);
                if (user == null)
                    return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "User not found or invalid password ");

                return StatusCode(HttpStatusCode.OK.GetHashCode(), user);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
        }


        [HttpGet]
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

        public IActionResult Registration(RegistrationViewModel model)
        {
            try
            {
                User registertion = _registerInterface.RegistrationViewModel(model);
                if (registertion == null)
                    return StatusCode(HttpStatusCode.BadRequest.GetHashCode(), "Bad request");

                return StatusCode(HttpStatusCode.OK.GetHashCode(), registertion);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }

        }




        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (_registerInterface.IsValidUserEmail(model))
            {
                try
                {
                    int UserID = _registerInterface.GetUserID(model.Email);
                    string welcomeMessage = "Welcome to CI Platform, <br/> You can Reset your password using below link. </br>";
                    string path = "<a href=\""+"https://"+_httpContextAccessor.HttpContext.Request.Host.Value+"/Account/resetPassword/"+UserID.ToString()+"\"style=\"font-weight:500;color:blue;\">Reset Password</a>";
                    MailHelper mailHelper = new MailHelper(configuration);
                    ViewBag.sendMail = mailHelper.Send(model.Email, welcomeMessage + path);
                    ModelState.Clear();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("", _registerInterface.Message());
                ViewBag.isForgetPasswordOpen = true;
            }
            return View("login");
        }



        public IActionResult resetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(User model, string code)
        {

            var resetUser = _registerInterface.PasswordResets.OrderByDescending(x => x.CreatedAt).FirstOrDefault(x => x.Token.Equals(code));
            var user = new User();
            if (resetUser != null)
            {
                user = _registerInterface.Users.FirstOrDefault(x => x.Email.Equals(resetUser.Email));

                user.Password = model.Password;
                _registerInterface.Users.Update(user);
                _registerInterface.SaveChanges();

                return RedirectToAction("login", "Account");
            }

            return RedirectToAction("Registration", "Account");

        }

    }
}