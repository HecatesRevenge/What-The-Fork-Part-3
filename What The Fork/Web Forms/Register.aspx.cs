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
    public partial class Register : System.Web.UI.Page
    {
        AccountDB objAccountDB = new AccountDB();
        PasswordHasher odjHasher = new PasswordHasher();

        RegisterValidator odjRegisterValidator = new RegisterValidator();


        protected void Page_Load(object sender, EventArgs e)
        {
            litHeader.Text = UiHeader.GetHeader((UserAccounts)Session["User"]);
            litFooter.Text = UiFooter.GetFooter();

        }

        protected void rblAccountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            divReviewerInfo.Visible = (rblAccountType.SelectedValue == "Reviewer");

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            UserAccounts objUser = new UserAccounts();
            objUser.UserType = rblAccountType.SelectedValue;
            objUser.FirstName = txtFirstName.Text.Trim();
            objUser.LastName = txtLastName.Text.Trim();
            objUser.Email = txtEmail.Text.Trim();
            objUser.PhoneNumber = txtPhone.Text.Trim();
            objUser.Username = txtUsername.Text.Trim();


            String homeCity = txtHomeCity.Text.Trim();
            String homeState = txtHomeState.Text.Trim().ToUpper();
            String password = txtPassword.Text;

            divReviewerInfo.Visible = objUser.IsReviewer;

            String errors = odjRegisterValidator.CheckRegistration(objUser.UserType, objUser.FirstName, objUser.LastName, objUser.Email,
     homeCity, homeState, objUser.Username, password, txtConfirmPassword.Text);

            if (errors != "")
            {
                ShowError(errors + "Please enter your password again in both boxes.");
                return;
            }

            try
            {
                if (objAccountDB.IsUernameTaken(objUser.Username))
                {
                    ShowError("The username \"" + Server.HtmlEncode(objUser.Username) + "\" is already taken. Please choose another one.<br />" +
                        "Please enter your password again in both boxes.");
                    return;
                }


                objUser.Password = odjHasher.HashPassword(password);

                int newUserID = objAccountDB.AddUserAccount(objUser);
                if (newUserID == 0)
                {
                    ShowError("Your account couldn't be created. Please try again later.");
                    return;
                }
                objUser.UserID = newUserID;

                if (objUser.IsReviewer)
                {
                    if (objAccountDB.AddReviewer(newUserID, objUser.FirstName, objUser.LastName, homeCity, homeState) < 1)
                    {
                        ShowError("Your account was created, but your reviewer profile couldn't be saved. Please contact us.");
                        return;
                    }
                }

                objUser.Password = "";
                Session["User"] = objUser;
            }
            catch (Exception ex)
            {
                ShowError("Your account couldn't be created right now. Please try again later.");
                return;
            }

            Response.Redirect("Dashboard.aspx");

        }
        private void ShowError(String theMessage)
        {
            lblValidation.Text = theMessage;
            lblValidation.Visible = true;
        }
    }
}