using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using What_The_Fork.Classes;
using What_The_Fork.Navigation;

namespace What_The_Fork.Web_Forms
{
    public partial class Login : System.Web.UI.Page
    {
        AccountDB odjAccountDB = new AccountDB();
        PasswordHasher odjHasher=new PasswordHasher();
        protected void Page_Load(object sender, EventArgs e)
        {
            litHeader.Text = UiHeader.GetHeader((UserAccounts)Session["User"]);
            litFooter.Text = UiFooter.GetFooter();

            if (!IsPostBack)
            {
                if (Request.QueryString["loggedout"] == "yes")
                {
                    lblMessage.Text = "Successfully logged out";
                    lblMessage.Visible = true;

                }
                else if (Session["User"] != null)
                {
                    UserAccounts odjUser=(UserAccounts)Session["User"];
                    lblMessage.Text = "You are already logged in as " + Server.HtmlEncode(odjUser.Username) + ".";
                    lblMessage.Visible = true;
                }
            }

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            String username=txtUsername.Text.Trim();
            String password= txtPassword.Text;

            String errors = "";

            if(username == "")
            {
                errors += "Please enter your username.<br />";
            }
            if(password == "")
            {
                errors += "Please enter your password.<br />";
            }
            if(errors != "")
            {
                ShowError(errors);
                return;
            }

            UserAccounts odjUser;
            try
            {
                odjUser = odjAccountDB.GetUserLogin(username);

            }
            catch (Exception ex)
            {
                ShowError("We couldn't log you in right now. Please try again later.");
                return;
            }

            if (odjUser == null || !odjHasher.VerifyPassword(password, odjUser.Password))
            {
                ShowError("The username or password is incorrect. Please try again.");
                return;
            }


            //Making sure the password isnt kept in seassion
            odjUser.Password = "";
            Session["User"] = odjUser;

            Response.Redirect("Dashboard.aspx");
        }


        private void ShowError(String theMessage)
        {
            lblValidation.Text = theMessage;
            lblValidation.Visible = true;
        }
    }
}