using Microsoft.AspNetCore.Mvc;
using WhatTheForkMVC.Classes;
namespace WhatTheForkMVC.Controllers
{
    [Route("api/[controller]")]

    public class Verification : Controller
    {
        AccountDB objAccountDB = new AccountDB();



        [HttpGet("{id}/{code}")]

        public IActionResult Verify(int id, String code)
        {
            if (objAccountDB.VerifyAccount(id, code))
            {
                ViewData["Message"] = ("Account verified successfully.");
            }
            else
            {
                ViewData["Error"] = ("Invalid verification link or account already verified.");
            }

            return View();


        }

    }
}
