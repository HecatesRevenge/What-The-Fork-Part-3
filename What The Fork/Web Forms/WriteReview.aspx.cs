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
    public partial class WriteReview : System.Web.UI.Page
    {
        ReviewDB odjReviewDB = new ReviewDB();
        RestaurantDB odjRestaurantDB = new RestaurantDB();
        ReviewValidator odjValidator = new ReviewValidator();

        protected void Page_Load(object sender, EventArgs e)
        {
            UserAccounts odjUser = (UserAccounts)Session["User"];

            litHeader.Text = UiHeader.GetHeader(odjUser);
            litFooter.Text = UiFooter.GetFooter();

            if (odjUser == null)
            {
                ShowMessage("Log in to write a review",
                    "You need a reviewer account to write reviews. Log in, or register for free.",
                    "Login.aspx", "Log in");
                return;
            }

            if (!odjUser.IsReviewer)
            {
                ShowMessage("Reviewers only",
                    "Only reviewer accounts can write reviews. Restaurant representatives can respond to reviews from their dashboard.",
                    "Dashboard.aspx", "Back to my dashboard");
                return;
            }

            if (!IsPostBack)
            {
                try
                {
                    int reviewerID = odjReviewDB.GetReviewerID(odjUser.UserID);
                    if (reviewerID == 0)
                    {
                        ShowMessage("Reviewer profile not found",
                            "Your account doesn't have a reviewer profile, so it can't write reviews. Please contact us.",
                            "Dashboard.aspx", "Back to my dashboard");
                        return;
                    }

                    int reviewID;
                    int restaurantID;
                    int.TryParse(Request.QueryString["reviewid"], out reviewID);
                    int.TryParse(Request.QueryString["id"], out restaurantID);

                    if (Request.QueryString["reviewid"] != null)
                        LoadReview(reviewID, reviewerID);
                    else
                        LoadRestaurant(restaurantID);
                }
                catch (Exception ex)
                {
                    ShowMessage("Something went wrong",
                        "We couldn't load this page right now. Please try again later.",
                        "Dashboard.aspx", "Back to my dashboard");
                }
            }
        }

        private void LoadRestaurant(int restaurantID)
        {
            DataSet dsRestaurant = odjRestaurantDB.GetRestaurant(restaurantID);
            if (dsRestaurant.Tables[0].Rows.Count == 0)
            {
                ShowMessage("Restaurant not found",
                    "We couldn't find that restaurant. Search for it, open its page, and click Write a Review.",
                    "Search.aspx", "Find a restaurant");
                return;
            }

            String name = dsRestaurant.Tables[0].Rows[0]["RestaurantName"].ToString();
            lblRestaurantName.Text = Server.HtmlEncode(name);
            Page.Title = "Review " + name + " | What the Fork";

            ViewState["RestaurantID"] = restaurantID;
            ViewState["ReviewID"] = 0;
            ViewState["RestaurantName"] = name;

            lnkCancel.NavigateUrl = "RestaurantProfile.aspx?id=" + restaurantID;
        }

        private void LoadReview(int reviewID, int reviewerID)
        {
            Review odjReview = odjReviewDB.GetReviewByID(reviewID, reviewerID);
            if (odjReview == null)
            {
                ShowMessage("Review not found",
                    "We couldn't find that review. It may have been deleted, or it isn't one of yours.",
                    "MyReviews.aspx", "Back to my reviews");
                return;
            }

            DataSet dsRestaurant = odjRestaurantDB.GetRestaurant(odjReview.RestaurantID);
            String name = "";
            if (dsRestaurant.Tables[0].Rows.Count > 0)
            {
                name = dsRestaurant.Tables[0].Rows[0]["RestaurantName"].ToString();

            }

            lblEyebrow.Text = "Edit your review";
            lblRestaurantName.Text = Server.HtmlEncode(name);
            lblFormTitle.Text = "Change your review";
            btnSave.Text = "Save changes";
            Page.Title = "Edit Review | What the Fork";

            SelectValue(rblFood, odjReview.FoodRating);
            SelectValue(rblService, odjReview.ServiceRating);
            SelectValue(rblAtmosphere, odjReview.AtmosphereRating);
            SelectValue(ddlPrice, odjReview.PriceRating);

            txtVisitDate.Text = odjReview.VisitDate.ToString("yyyy-MM-dd");
            txtVisitTime.Text = odjReview.VisitDate.ToString("HH:mm");

            txtTitle.Text = odjReview.ReviewTitle;
            txtText.Text = odjReview.ReviewText;

            ShowCurrentPhoto(odjReview);

            ViewState["RestaurantID"] = odjReview.RestaurantID;
            ViewState["ReviewID"] = odjReview.ReviewID;
            ViewState["RestaurantName"] = name;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            UserAccounts odjUser = (UserAccounts)Session["User"];
            if (odjUser == null || !odjUser.IsReviewer || ViewState["ReviewID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            int foodRating = GetSelectedNumber(rblFood);
            int serviceRating = GetSelectedNumber(rblService);
            int atmosphereRating = GetSelectedNumber(rblAtmosphere);
            int priceRating = GetSelectedNumber(ddlPrice);

            String visitDate = txtVisitDate.Text.Trim();
            String visitTime = txtVisitTime.Text.Trim();
            String title = txtTitle.Text.Trim();
            String text = txtText.Text.Trim();

            String errors = odjValidator.CheckReview(foodRating, serviceRating, atmosphereRating, priceRating,
                visitDate, visitTime, title, text);

            if (fuPhoto.HasFile)
            {
                errors = errors + odjValidator.CheckPhoto(fuPhoto.PostedFile.FileName, fuPhoto.PostedFile.ContentLength);

            }

            if (errors != "")
            {
                if (fuPhoto.HasFile)
                {
                    errors = errors + "Please choose your photo again.";
                }

                ShowError(errors);
                return;
            }

            int reviewID = (int)ViewState["ReviewID"];
            String restaurantName = ViewState["RestaurantName"].ToString();

            Review odjReview = new Review();
            odjReview.ReviewID = reviewID;
            odjReview.RestaurantID = (int)ViewState["RestaurantID"];
            odjReview.VisitDate = DateTime.Parse(visitDate + " " + visitTime);
            odjReview.ReviewTitle = title;
            odjReview.ReviewText = text;
            odjReview.FoodRating = foodRating;
            odjReview.ServiceRating = serviceRating;
            odjReview.AtmosphereRating = atmosphereRating;
            odjReview.PriceRating = priceRating;

            byte[] photoData = null;
            String photoType = "";
            if (fuPhoto.HasFile)
            {
                int photoSize = fuPhoto.PostedFile.ContentLength;
                photoData = new byte[photoSize];
                fuPhoto.PostedFile.InputStream.Read(photoData, 0, photoSize);
                photoType = fuPhoto.PostedFile.ContentType;
            }

            try
            {

                odjReview.ReviewerID = odjReviewDB.GetReviewerID(odjUser.UserID);

                if (reviewID == 0)
                {
                    odjReview.ReviewPhoto = photoData;
                    odjReview.ReviewPhotoType = photoType;

                    if (odjReviewDB.AddReview(odjReview) < 1)
                    {
                        ShowError("Your review couldn't be posted. Please try again later.");
                        return;
                    }
                    Session["ReviewMessage"] = "Thanks! Your review of " + restaurantName + " was posted.";
                }
                else
                {
                    if (odjReviewDB.UpdateReview(odjReview) < 1)
                    {
                        ShowError("Your changes couldn't be saved. The review may have been deleted.");
                        return;
                    }
                    else
                    {
                        if (photoData != null)
                        {
                            odjReviewDB.UpdateReviewPhoto(reviewID, odjReview.ReviewerID, photoData, photoType);

                        }
                        else if (chkRemovePhoto.Checked)
                        {
                            odjReviewDB.UpdateReviewPhoto(reviewID, odjReview.ReviewerID, null, "");

                        }


                        Session["ReviewMessage"] = "Your review of " + restaurantName + " was updated.";
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Your review couldn't be saved right now. Please try again later.");
                return;
            }

            Response.Redirect("MyReviews.aspx");
        }

        private void ShowCurrentPhoto(Review review)
        {
            if (review.HasPhoto)
            {
                imgCurrentPhoto.ImageUrl = "data:" + review.ReviewPhotoType + ";base64," +
                                           Convert.ToBase64String(review.ReviewPhoto);
                divCurrentPhoto.Visible = true;
            }
        }

        private int GetSelectedNumber(ListControl list)
        {
            int number;
            if (list.SelectedIndex == -1 || !int.TryParse(list.SelectedValue, out number))
            {

                return 0;
            }
            return number;
        }

        private void SelectValue(ListControl list, int number)
        {
            ListItem item = list.Items.FindByValue(number.ToString());
            if (item != null)
            {
                list.SelectedValue = number.ToString();


            }
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
    }
}