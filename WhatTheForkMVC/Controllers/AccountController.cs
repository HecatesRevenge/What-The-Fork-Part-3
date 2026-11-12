using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using WhatTheForkMVC.Classes;

namespace WhatTheForkMVC.Controllers
{
    public class AccountController : Controller
    {
        AccountDB objAccountDB = new AccountDB();
        PasswordHasher objHasher = new PasswordHasher();
        LoginCookie objCookie = new LoginCookie();
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
    }
}

