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
                {
                    return StatusCode(HttpStatusCode.NotFound.GetHashCode(), "User not found or invalid password ");
                }
                else
                {
                    return RedirectToAction("missionLandingPlateform", "Mission");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
            }
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
                

                if (_registerInterface.IsValidUserEmail(model))
                {
                    User registertion = _registerInterface.RegistrationViewModel(model);
                    return RedirectToAction("login", "Account");
                  
                }
                else
                {
                    return StatusCode(HttpStatusCode.BadRequest.GetHashCode(), "This Mail Account Already Register !! Please Check your mail or Login your Account...");
                }
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
                    return StatusCode(HttpStatusCode.InternalServerError.GetHashCode(), ex.Message);
                }
            }
            else
            {
                
                return StatusCode(HttpStatusCode.BadRequest.GetHashCode(), "This Mail Account not Register !! Please Check your mail or Do Registration...");
            }

            return View("login");
        }



       
        [HttpGet]
        public IActionResult Resetpassword(long id)
        {
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            model.UserId = id;
            return View(model);
        }

        [HttpPost]
        public IActionResult Resetpassword(ResetPasswordViewModel model, long id)
        {
            if (ModelState.IsValid)
            {
               
                if (_registerInterface.ChangePassword(id, model))
                {
                    ModelState.Clear();
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Enter Same Password");
                }
            }

            return View();
        }

    }
}