using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using WhatTheForkMVC.Classes;
using WhatTheForkMVC.Classes.Login;
using WhatTheForkMVC.Classes.Security;
using WhatTheForkMVC.Models;
namespace WhatTheForkMVC.Controllers
{
    public class AccountController : Controller
    {
        AccountDB objAccountDB = new AccountDB();
        PasswordHasher objHasher = new PasswordHasher();
        LoginCookie objCookie = new LoginCookie();
        RegisterValidator objValidator = new RegisterValidator();
        SecurityQuestionChecks objQuestionCheck = new SecurityQuestionChecks();
        public IActionResult Login()
        {
            UserAccounts objUser = SessionUser.GetUser(HttpContext.Session);

            if (objUser != null)
            {
                ViewData["Message"] = "You are already logged in as " + objUser.Username + ".";
            }

            String savedUsername = objCookie.ReadLoginID(Request);
            if (savedUsername != "")
            {
                ViewData["Username"] = savedUsername;
                ViewData["RememberMe"] = true;
            }

            return View();
        }

        //TODO: if I have time create error checking class 
        [HttpPost]
        public IActionResult Login(String username, String password, String remeberMe)
        {
            if (username == null)
            {
                username = "";
            }
            if (password == null)
            {
                password = "";
            }

            username = username.Trim();
            String errors = "";

            if (username == "")
            {
                errors += "Username is required. ";
            }
            if (password == "")
            {
                errors += "Password is required. ";
            }

            if (errors != "")
            {
                ViewData["Error"] = errors;
                ViewData["Username"] = username;
                return View();
            }

            UserAccounts objUser;

            try
            {
                objUser = objAccountDB.GetUserLogin(username);

            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error occured while trying to log in";
                ViewData["Username"] = username;
                return View();

            }
            if (objUser == null || !objHasher.VerifyPassword(password, objUser.Password))
            {
                ViewData["Error"] = "Invalid username or password";
                ViewData["Username"] = username;
                return View();
            }

            if (!objUser.IsVerified)
            {
                ViewData["Error"] = "Please verify your account before logging in. Check your email for the verification link.";
                ViewData["Username"] = username;
                return View();
            }

            if (remeberMe != null)
            {
                objCookie.SaveLoginID(Response, username);

            }
            else
            {
                objCookie.DeleteLoginID(Response);
            }

            objUser.Password = "";
            SessionUser.SaveUser(HttpContext.Session, objUser);

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult DeleteCookie()
        {
            objCookie.DeleteLoginID(Response);
            ViewData["Message"] = "Saved username cleared.";
            return View("Login");
        }

        public IActionResult Logout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Logout(String forgotMe)
        {

            HttpContext.Session.Clear();

            if (forgotMe != null)
            {
                objCookie.DeleteLoginID(Response);

            }

            ViewData["Message"] = "You have logged out successfully.";
            return View("Login");
        }

        public IActionResult Register()
        {
            ViewData["Questions"] = objAccountDB.GetSecurityQuestions();
            return View(new RegisterForm());
        }

        [HttpPost]
        public IActionResult Register(RegisterForm form)
        {
            String errors = objValidator.CheckRegistration(form.AccountType, form.FirstName.Trim(), form.LastName.Trim(),
                form.Email.Trim(), form.HomeCity.Trim(), form.HomeState.Trim(),
                form.Username.Trim(), form.Password, form.ConfirmPassword);

            if (form.Username.Trim() != "" && objAccountDB.IsUernameTaken(form.Username.Trim()))
            {
                errors += "Username taken. Please choose another.<br />";
            }

            if (form.Question1 == 0 || form.Question2 == 0 || form.Question3 == 0)
            {
                errors += "Please select three security questions.<br />";
            }
            else if (form.Question1 == form.Question2 || form.Question1 == form.Question3 || form.Question2 == form.Question3)
            {
                errors += "You must select three unique security questions.<br />";
            }

            if (form.Answer1.Trim() == "" || form.Answer2.Trim() == "" || form.Answer3.Trim() == "")
            {
                errors += "Please answer all security questions.<br />";
            }

            if (errors != "")
            {
                ViewData["Error"] = errors;
                ViewData["Questions"] = objAccountDB.GetSecurityQuestions();
                return View(form);
            }

            UserAccounts objUser = new UserAccounts();
            objUser.Username = form.Username.Trim();
            objUser.Password = objHasher.HashPassword(form.Password);
            objUser.UserType = form.AccountType;
            objUser.FirstName = form.FirstName.Trim();
            objUser.LastName = form.LastName.Trim();
            objUser.Email = form.Email.Trim();
            objUser.PhoneNumber = form.Phone.Trim();

            try
            {
                objUser.UserID = objAccountDB.AddUserAccount(objUser);

                if (objUser.UserID == 0)
                {
                    ViewData["Error"] = "Account could not be created. Try again.";
                    ViewData["Questions"] = objAccountDB.GetSecurityQuestions();
                    return View(form);
                }

                if (objUser.IsReviewer)
                {
                    objAccountDB.AddReviewer(objUser.UserID, objUser.FirstName, objUser.LastName,
                        form.HomeCity.Trim(), form.HomeState.Trim().ToUpper());
                }

                objAccountDB.AddSecurityAnswer(objUser.UserID, form.Question1, objHasher.HashPassword(objQuestionCheck.CleanResponse(form.Answer1)));
                objAccountDB.AddSecurityAnswer(objUser.UserID, form.Question2, objHasher.HashPassword(objQuestionCheck.CleanResponse(form.Answer2)));
                objAccountDB.AddSecurityAnswer(objUser.UserID, form.Question3, objHasher.HashPassword(objQuestionCheck.CleanResponse(form.Answer3)));
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Your account could not be created. Try again later.";
                ViewData["Questions"] = objAccountDB.GetSecurityQuestions();
                return View(form);
            }

            if (form.SaveLoginID)
            {
                objCookie.SaveLoginID(Response, objUser.Username);
            }

            SessionUser.SaveUser(HttpContext.Session, objUser);
            return RedirectToAction("Index", "Dashboard");

        }
    }
}

