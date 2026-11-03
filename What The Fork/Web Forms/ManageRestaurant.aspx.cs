using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using What_The_Fork.Classes;
using What_The_Fork.Database;
using What_The_Fork.Navigation;

namespace What_The_Fork.Web_Forms
{
    public partial class ManageRestaurant : System.Web.UI.Page
    {
        RestaurantDB odjRestaurantDB = new RestaurantDB();
        RestaurantFormHelper odjHelper = new RestaurantFormHelper();
        RestaurantValidator odjValidator = new RestaurantValidator();
        ReviewValidator odjPhotoValidator = new ReviewValidator();

        protected void Page_Load(object sender, EventArgs e)
        {
            UserAccounts odjUser = (UserAccounts)Session["User"];

            litHeader.Text = UiHeader.GetHeader(odjUser);
            litFooter.Text = UiFooter.GetFooter();

            if (odjUser == null)
            {
                ShowMessage("Log in to manage your restaurant",
                    "You need a representative account to add or edit a restaurant. Log in, or register for free.",
                    "Login.aspx", "Log in");
                return;
            }

            if (!odjUser.IsRepresentative)
            {
                ShowMessage("Representatives only",
                    "Only restaurant representative accounts can add or edit a restaurant. Reviewers can write reviews from their dashboard.",
                    "Dashboard.aspx", "Back to my dashboard");
                return;
            }

            try
            {
                int restaurantID = GetMyRestaurantID();

                if (!IsPostBack)
                {
                    odjHelper.FillCuisines(ddlCuisine);
                    odjHelper.FillStates(ddlState);

                    if (restaurantID == 0)
                        ShowNewRestaurant();
                    else
                        ShowRestaurant();

                    gvHours.DataSource = odjHelper.GetHoursTable(restaurantID);
                    gvHours.DataBind();

                    gvSocialLinks.DataSource = odjHelper.GetSocialLinksTable(restaurantID);
                    gvSocialLinks.DataBind();

                    if (Session["RestaurantMessage"] != null)
                    {
                        lblMessage.Text = Server.HtmlEncode(Session["RestaurantMessage"].ToString());
                        lblMessage.Visible = true;
                        Session.Remove("RestaurantMessage");
                    }
                }

               
                if (restaurantID > 0)
                {
                    divPhotos.Visible = true;
                    divPhotosLater.Visible = false;
                    ShowPhotos(restaurantID);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Something went wrong",
                    "We couldn't load your restaurant right now. Please try again later.",
                    "Dashboard.aspx", "Back to my dashboard");
            }
        }

        private int GetMyRestaurantID()
        {
            UserAccounts odjUser = (UserAccounts)Session["User"];
            return odjHelper.GetRestaurantID(odjUser.UserID);
        }

        
        private void ShowNewRestaurant()
        {
            lblEyebrow.Text = "Get started";
            lblPageTitle.Text = "Add Your Restaurant";
            btnSave.Text = "Add my restaurant";
            Page.Title = "Add Your Restaurant | What the Fork";
        }

        private void ShowRestaurant()
        {
            UserAccounts odjUser = (UserAccounts)Session["User"];
            DataRow row = odjRestaurantDB.GetResturantByRep(odjUser.UserID).Tables[0].Rows[0];

            String name = row["RestaurantName"].ToString();
            int restaurantID = Convert.ToInt32(row["RestaurantID"]);

            lblEyebrow.Text = "Your listing";
            lblPageTitle.Text = Server.HtmlEncode(name);
            lblInstructions.Text = "Edit the information below and click Save changes.";
            btnSave.Text = "Save changes";
            Page.Title = "My Restaurant | What the Fork";

            txtName.Text = name;
            odjHelper.SelectValue(ddlCuisine, row["CuisineID"].ToString());
            odjHelper.SelectValue(ddlPrice, row["PriceRange"].ToString());
            txtDescription.Text = odjHelper.GetText(row["Description"]);
            txtOwner.Text = odjHelper.GetText(row["OwnerName"]);

            txtStreet.Text = odjHelper.GetText(row["StreetAddress"]);
            txtCity.Text = odjHelper.GetText(row["City"]);
            odjHelper.SelectValue(ddlState, odjHelper.GetText(row["State"]));
            txtZip.Text = odjHelper.GetText(row["ZipCode"]);
            txtPhone.Text = odjHelper.GetText(row["Phone"]);
            txtEmail.Text = odjHelper.GetText(row["Email"]);
            txtWebsite.Text = odjHelper.GetText(row["WebsiteUrl"]);

            lnkViewProfile.NavigateUrl = "RestaurantProfile.aspx?id=" + restaurantID;
            pViewProfile.Visible = true;
        }

        private void ShowPhotos(int restaurantID)
        {
            gvPhotos.DataSource = odjHelper.GetPhotos(restaurantID);
            gvPhotos.DataBind();
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            UserAccounts objUser = (UserAccounts)Session["User"];
            if (objUser == null || !objUser.IsRepresentative)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            Restaurant odjRestaurant = new Restaurant();
            odjRestaurant.RepUserID = objUser.UserID;
            odjRestaurant.RestaurantName = txtName.Text.Trim();
            odjRestaurant.CuisineID = int.Parse(ddlCuisine.SelectedValue);
            odjRestaurant.PriceRange = int.Parse(ddlPrice.SelectedValue);
            odjRestaurant.Description = txtDescription.Text.Trim();
            odjRestaurant.OwnerName = txtOwner.Text.Trim();
            odjRestaurant.StreetAddress = txtStreet.Text.Trim();
            odjRestaurant.City = txtCity.Text.Trim();
            odjRestaurant.State = ddlState.SelectedValue;
            odjRestaurant.ZipCode = txtZip.Text.Trim();
            odjRestaurant.Phone = txtPhone.Text.Trim();
            odjRestaurant.Email = txtEmail.Text.Trim();
            odjRestaurant.WebsiteUrl = txtWebsite.Text.Trim();

            String errors = odjValidator.CheckRestaurantInfo(odjRestaurant.RestaurantName, odjRestaurant.CuisineID,
                odjRestaurant.PriceRange, odjRestaurant.Description, odjRestaurant.StreetAddress, odjRestaurant.City,
                odjRestaurant.State, odjRestaurant.ZipCode, odjRestaurant.Phone, odjRestaurant.Email,
                odjRestaurant.WebsiteUrl, odjRestaurant.OwnerName);

            foreach (GridViewRow row in gvHours.Rows)
            {
                CheckBox chkClosed = (CheckBox)row.FindControl("chkClosed");
                TextBox txtOpen = (TextBox)row.FindControl("txtOpen");
                TextBox txtClose = (TextBox)row.FindControl("txtClose");

                errors = errors + odjValidator.CheckHours(row.Cells[0].Text, chkClosed.Checked,
                    txtOpen.Text.Trim(), txtClose.Text.Trim());
            }

            foreach (GridViewRow row in gvSocialLinks.Rows)
            {
                TextBox txtLink = (TextBox)row.FindControl("txtLink");
                String platformName = gvSocialLinks.DataKeys[row.RowIndex].Values["PlatformName"].ToString();

                errors = errors + odjValidator.CheckLink(platformName, txtLink.Text.Trim());
            }

            if (errors != "")
            {
                ShowError(errors);
                return;
            }

            String message = "";
            try
            {
                int restaurantID = GetMyRestaurantID();

                if (restaurantID == 0)
                {
                    restaurantID = odjRestaurantDB.AddRestaurant(odjRestaurant);
                    if (restaurantID < 1)
                    {
                        ShowError("Your restaurant couldn't be added. Please try again later.");
                        return;
                    }
                    message = odjRestaurant.RestaurantName + " was added.";
                }
                else
                {
                    odjRestaurant.RestaurantID = restaurantID;
                    if (odjRestaurantDB.UpdateRestaurant(odjRestaurant) < 1)
                    {
                        ShowError("Your changes couldn't be saved. Please try again later.");
                        return;
                    }
                    message = "Your changes to " + odjRestaurant.RestaurantName + " were saved.";
                }

                odjHelper.SaveHours(restaurantID, gvHours);
                odjHelper.SaveSocialLinks(restaurantID, gvSocialLinks);
            }
            catch (Exception ex)
            {
                ShowError("Your restaurant couldn't be saved right now. Please try again later.");
                return;
            }

            Session["RestaurantMessage"] = message;
            Response.Redirect("ManageRestaurant.aspx");
        }


        protected void btnUpload_Click(object sender, EventArgs e)
        {
            UserAccounts objUser = (UserAccounts)Session["User"];
            if (objUser == null || !objUser.IsRepresentative)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            String photoType = ddlPhotoType.SelectedValue;
            String caption = txtCaption.Text.Trim();

            String errors = "";
            if (!fuPhoto.HasFile)
                errors = "Please choose a photo to upload.<br />";
            else
                errors = odjPhotoValidator.CheckPhoto(fuPhoto.PostedFile.FileName, fuPhoto.PostedFile.ContentLength);

            errors = errors + odjValidator.CheckCaption(photoType, caption);

            if (errors != "")
            {
                if (fuPhoto.HasFile)
                    errors = errors + "Please choose your photo again.";
                ShowPhotoError(errors);
                return;
            }

            int photoSize = fuPhoto.PostedFile.ContentLength;
            byte[] photoData = new byte[photoSize];
            fuPhoto.PostedFile.InputStream.Read(photoData, 0, photoSize);
            String imageType = fuPhoto.PostedFile.ContentType;

            try
            {
                int restaurantID = GetMyRestaurantID();
                if (restaurantID == 0)
                {
                    ShowPhotoError("Please save the restaurant before adding photos.");
                    return;
                }

                if (odjHelper.SavePhoto(restaurantID, photoType, caption, photoData, imageType) < 1)
                {
                    ShowPhotoError("Error uploading photo. Please try again later.");
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowPhotoError("Your photo couldn't be uploaded right now. Please try again later.");
                return;
            }

            if (photoType == "Gallery")
            {
                Session["RestaurantMessage"] = "Your photo was added to your gallery.";

            }else if (photoType == "Logo")
            {
                Session["RestaurantMessage"] = "Your new logo was uploaded.";

            }
            else
            {
                Session["RestaurantMessage"] = "Your new profile photo was uploaded.";
            }
        
               
            Response.Redirect("ManageRestaurant.aspx");
        }

        protected void gvPhotos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "DeletePhoto")
                return;

            int rowIndex = int.Parse(e.CommandArgument.ToString());
            String photoID = gvPhotos.DataKeys[rowIndex].Values["PhotoID"].ToString();
            String photoType = gvPhotos.DataKeys[rowIndex].Values["PhotoType"].ToString();

            ViewState["DeletePhotoID"] = photoID;
            if (photoType == "Gallery")
            {
                lblConfirmDelete.Text = "Do you want to delete this gallery photo? This action can't be undone.";

            }
            else
            {
                lblConfirmDelete.Text = "Delete your " + photoType.ToLower() + " photo? This can't be undone.";

            }

            divConfirmDelete.Visible = true;
        }

        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            divConfirmDelete.Visible = false;

            if (ViewState["DeletePhotoID"] == null)
                return;

            int photoID = int.Parse(ViewState["DeletePhotoID"].ToString());
            ViewState.Remove("DeletePhotoID");

            try
            {
              
                int restaurantID = GetMyRestaurantID();
                if (odjRestaurantDB.DeletePhoto(photoID, restaurantID) == 1)
                {
                    lblPhotoMessage.Text = "Your photo was deleted.";
                    lblPhotoMessage.Visible = true;
                }
                else
                {
                    ShowPhotoError("This photo could not be deleted. ");
                }

                ShowPhotos(restaurantID);
            }
            catch (Exception ex)
            {
                ShowPhotoError("Photo couldn't be deleted. Please try again later.");
            }
        }

        protected void btnCancelDelete_Click(object sender, EventArgs e)
        {
            ViewState.Remove("DeletePhotoID");
            divConfirmDelete.Visible = false;
        }


        private void ShowMessage(String title, String text, String linkUrl, String linkText)
        {
            lblMessageTitle.Text = title;
            lblMessageText.Text = text;
            lnkMessage.NavigateUrl = linkUrl;
            lnkMessage.Text = linkText;
            divMessage.Visible = true;
            divForm.Visible = false;
        }

        private void ShowError(String message)
        {
            lblValidation.Text = message;
            lblValidation.Visible = true;
        }

        private void ShowPhotoError(String message)
        {
            lblPhotoValidation.Text = message;
            lblPhotoValidation.Visible = true;
        }
    }
}