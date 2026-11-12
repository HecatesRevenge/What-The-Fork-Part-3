using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using WhatTheForkMVC.Classes;

namespace WhatTheForkMVC.Controllers
{
    public class AccountController : Controller
    {
        AccountDB objAccountDB = new AccountDB();
        PasswordHasher objHasher = new PasswordHasher();
        public IActionResult Login(String loggedout)
        {
            UserAccounts objUser = SessionUser.GetUser(HttpContext.Session);

            if (loggedout == "yes")
            {
                ViewData["Message"] = "Logged out successfully.";
            }
            else if (objUser != null)
            {
                ViewData["Message"] = "You are already logged in as " + objUser.Username + ".";


            }
            return View();

        }


        //TODO: if I have time create error checking class 
        [HttpPost]
        public IActionResult Login(String username, String password)
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

            objUser.Password = "";
            SessionUser.SaveUser(HttpContext.Session, objUser);

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", new { loggedout = "yes" });
        }
    }
}

