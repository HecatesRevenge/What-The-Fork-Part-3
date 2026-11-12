using System.Web;
using WhatTheForkMVC.Classes;
using WhatTheForkMVC.Classes.Emails;
namespace WhatTheForkMVC.Classes.Emails
{
    public class VerificationEmail
    {
        //This can be changed to a different email later.
        //Can also add a function that lets an admin change this email address if needed.
        const String FromEmail="tus45674@temple.edu";

        public bool SendVerificationEmail(String email, String firstname, String link)
        {
            String subject= "WhatTheFork Account Verification";
            String body =
                        "<p>Hello "+HttpUtility.HtmlEncode(firstname) + ",</p>" +
                        "<p> Thank you for registering with WhatTheFork! Please click the link below to verify your account:</p>"+
                        "<p><a href=\"" + HttpUtility.HtmlEncode(link) + "\">Verify Account</a></p>" +
                        "<p>If you did not register for an account, please ignore this email.</p>";
            try
            {
              Email objEmail = new Email();
                objEmail.SendMail(email, FromEmail, subject, body);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
    }
}
