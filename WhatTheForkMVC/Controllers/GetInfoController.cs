using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.Data;
using WhatTheForkMVC.Classes;
using WhatTheForkMVC.Classes.Login;

namespace WhatTheForkMVC.Controllers
{
    public class GetInfoController : Controller
    {
       
        AccountDB objAccountDB = new AccountDB();
       // UserAccounts objUser;
        SecurityQuestionChecks objQuestionCheck = new SecurityQuestionChecks();
        public IActionResult getEmail(string email) {


            return View();
        }
        public IActionResult Submit(string answer, string email)
        {
            UserAccounts objUser;
           
                 objUser = objAccountDB.GetUserByEmail(email); 
            
          //  UserAccounts objUser = SessionUser.GetUser(HttpContext.Session);
            DataRow dr = objQuestionCheck.SelectRandomnSecurityQuestion(objUser.UserID);
            int selectedQ = Convert.ToInt32(dr["QuestionID"]);

            Boolean answerCheck= objQuestionCheck.CheckAnswer(objUser.UserID,selectedQ,answer);

            if (answerCheck!=false) {
                ViewData["Message"] = "Username: " + objUser.Username;
            }

            return View();
        }
        public IActionResult NewQuestion(string email){
            UserAccounts objUser = objAccountDB.GetUserByEmail(email);
            ViewData["Message"] = objQuestionCheck.SelectRandomnSecurityQuestion(objUser.UserID);
            return View();
        }


    }
}
