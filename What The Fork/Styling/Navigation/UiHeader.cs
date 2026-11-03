using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using What_The_Fork.Classes;

namespace What_The_Fork.Navigation
{
    public class UiHeader
    {
        public static String GetHeader(UserAccounts user)
        {
            String html =
                "<nav class=\"navbar navbar-expand-md site-navbar\" aria-label=\"Main navigation\">" +
                "  <div class=\"container\">" +
                "    <a class=\"navbar-brand brand\" href=\"Search.aspx\">" +
                "      <span class=\"brand-mark\"></span>" +
                "      <span class=\"brand-name\">What the <em>Fork</em></span>" +
                "    </a>" +
                "    <ul class=\"navbar-nav ms-auto\">" +
                "      <li class=\"nav-item\"><a class=\"nav-link\" href=\"Search.aspx\">Find Restaurants</a></li>" +
                "      <li class=\"nav-item\"><a class=\"nav-link\" href=\"Dashboard.aspx\">Dashboard</a></li>";



            if (user == null)
            {
                html = html +
                    "      <li class=\"nav-item\"><a class=\"nav-link\" href=\"Login.aspx\">Log in</a></li>" +
                    "      <li class=\"nav-item\"><a class=\"btn btn-wtf btn-sm ms-md-2\" href=\"Register.aspx\">Register</a></li>";
            }
            else
            {
                html = html +
                   "      <li class=\"nav-item\"><span class=\"nav-user\">Hi, " + HttpUtility.HtmlEncode(user.FirstName) +
                   " <span class=\"nav-user-type\">" + user.UserType + "</span></span></li>" +
                   "      <li class=\"nav-item\"><a class=\"nav-link\" href=\"Logout.aspx\">Log out</a></li>";

            }

            html = html +
            "    </ul>" +
            "  </div>" +
            "</nav>";
            return html;
        }
    }
}