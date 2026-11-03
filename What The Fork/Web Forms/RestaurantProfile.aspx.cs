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
using What_The_Fork.Styling.Reviews;


namespace What_The_Fork.Web_Forms
{
    public partial class RestaurantProfile : System.Web.UI.Page
    {
        RestaurantDB objRestaurantDB = new RestaurantDB();
        ReviewDB objReviewDB = new ReviewDB();

        RestaurantFormHelper odjHelper = new RestaurantFormHelper();


        protected void Page_Load(object sender, EventArgs e)
        {
            litHeader.Text = UiHeader.GetHeader((UserAccounts)Session["User"]);
            litFooter.Text = UiFooter.GetFooter();


            int restaurantID;
            int.TryParse(Request.QueryString["id"], out restaurantID);

            try
            {
                DataSet dsRestaurant = objRestaurantDB.GetRestaurant(restaurantID);
                if (dsRestaurant.Tables[0].Rows.Count == 0)
                {
                    ShowMessage("Restaurant not found", "We couldn't find that restaurant. Try searching again.");
                    return;
                }

                ShowRestaurant(dsRestaurant.Tables[0].Rows[0]);
                ShowPhotos(restaurantID);

                gvHours.DataSource = objRestaurantDB.GetHours(restaurantID);
                gvHours.DataBind();

                gvSocialLinks.DataSource = objRestaurantDB.GetSocialLinks(restaurantID);
                gvSocialLinks.DataBind();

                LoadReviews(restaurantID);
                ShowWriteReviewLink(restaurantID);
                ShowReserveLink(restaurantID);
            }
            catch (Exception ex)
            {
                ShowMessage("Something went wrong","Error loading restaurant. Please try again later.");
            }
        }

        private void ShowRestaurant(DataRow theRestaurant)
        {
            String name = theRestaurant["RestaurantName"].ToString();
            Page.Title = name + " | What the Fork";

            imgLogo.ImageUrl = GetImageUrl(theRestaurant["LogoImagePath"],
                "https://raw.githubusercontent.com/HecatesRevenge/What-The-Fork-Images/main/Placeholders/logo.svg");
            imgLogo.AlternateText = name + " logo";

            if (theRestaurant["CoverImagePath"] == DBNull.Value)
            {
                imgCover.Visible = false;


            }
            else
            {
                imgCover.ImageUrl = GetImageUrl(theRestaurant["CoverImagePath"], "");

            }

            lblRestaurantName.Text = Server.HtmlEncode(name);
            lblCuisine.Text = Server.HtmlEncode(theRestaurant["CuisineName"].ToString());
            lblPriceLevel.Text = new String('$', Convert.ToInt32(theRestaurant["PriceRange"]));
            lblLocation.Text = Server.HtmlEncode(theRestaurant["City"] + ", " + theRestaurant["State"]);
            lblDescription.Text = Server.HtmlEncode(theRestaurant["Description"].ToString());

            int reviewCount = Convert.ToInt32(theRestaurant["ReviewCount"]);
            if (reviewCount == 0)
            {
                lblOverallRating.Text = "New";
                lblReviewCount.Text = "No reviews yet";
                divRatings.InnerHtml = "<p class=\"text-muted-light mb-0\">Ratings will appear after the first review.</p>";
            }
            else
            {
                double overall = GetRating(theRestaurant["OverallRating"]);
                lblOverallRating.Text = overall.ToString("0.0");
                divOverallStars.InnerHtml = ReviewDivs.GetStarsHtml(overall);

                if (reviewCount == 1)
                    lblReviewCount.Text = "1 review";
                else
                    lblReviewCount.Text = reviewCount + " reviews";

                divRatings.InnerHtml = ReviewDivs.GetRatingBarsHtml(
                    GetRating(theRestaurant["FoodRating"]),
                    GetRating(theRestaurant["ServiceRating"]),
                    GetRating(theRestaurant["AtmosphereRating"]),
                    GetRating(theRestaurant["PriceRating"]));
            }

            lblAddress.Text = Server.HtmlEncode(theRestaurant["StreetAddress"].ToString()) + "<br />" +
                              Server.HtmlEncode(theRestaurant["City"] + ", " + theRestaurant["State"] + " " + theRestaurant["ZipCode"]);
            lblOwner.Text = Server.HtmlEncode(GetText(theRestaurant["OwnerName"]));

            String phone = GetText(theRestaurant["Phone"]);
            lnkPhone.Text = Server.HtmlEncode(phone);
            if (phone != "Not listed")
            {
                lnkPhone.NavigateUrl = "tel:" + phone;

            }

            String email = GetText(theRestaurant["Email"]);
            lnkEmail.Text = Server.HtmlEncode(email);
            if (email != "Not listed")
            {
                lnkEmail.NavigateUrl = "mailto:" + email;
            }

         
            String website = GetText(theRestaurant["WebsiteUrl"]);
            lnkWebsite.Text = Server.HtmlEncode(website);

            if (website != "Not listed")
            {
                lnkWebsite.NavigateUrl = website;

            }
        }

        private void ShowPhotos(int restaurantID)
        {
            DataSet dsPhotos = odjHelper.GetPhotos(restaurantID);

            DataTable odjGallery = dsPhotos.Tables[0].Clone();

            foreach (DataRow row in dsPhotos.Tables[0].Rows)
            {
                String photoType = row["PhotoType"].ToString();

                if (photoType == "Logo")
                {
                    imgLogo.ImageUrl = row["ImageUrl"].ToString();
                }
                else if (photoType == "Profile")
                {
                    imgCover.ImageUrl = row["ImageUrl"].ToString();
                    imgCover.Visible = true;
                }
                else if (photoType == "Gallery")
                {
                    odjGallery.ImportRow(row);
                }
            }

            lblNoPhotos.Visible = (odjGallery.Rows.Count == 0);
            rptGallery.Visible = (odjGallery.Rows.Count > 0);
            rptGallery.DataSource = odjGallery;
            rptGallery.DataBind();
        }



        private void LoadReviews(int restaurantID)
        {
            DataSet dsReviews = objReviewDB.GetReviews(restaurantID);
            int count = dsReviews.Tables[0].Rows.Count;

            lblReviewsHeading.Text = "Reviews (" + count + ")";
            lblNoReviews.Visible = (count == 0);

            foreach (DataRow row in dsReviews.Tables[0].Rows)
            {
                ReviewDisplay reviewControl = (ReviewDisplay)LoadControl("ReviewDisplay.ascx");

                reviewControl.ID = "ucReview" + row["ReviewID"];

                reviewControl.ReviewID = Convert.ToInt32(row["ReviewID"]);
                reviewControl.ReviewerName = row["ReviewerName"].ToString();
                reviewControl.ReviewerHome = row["ReviewerHome"].ToString();
                reviewControl.ReviewDate = Convert.ToDateTime(row["ReviewDate"]);
                reviewControl.ReviewTitle = row["ReviewTitle"].ToString();
                reviewControl.ReviewText = row["ReviewText"].ToString();
                reviewControl.FoodRating = Convert.ToInt32(row["FoodRating"]);
                reviewControl.ServiceRating = Convert.ToInt32(row["ServiceRating"]);
                reviewControl.PriceRating = Convert.ToInt32(row["PriceRating"]);
                reviewControl.HelpfulCount = Convert.ToInt32(row["HelpfulCount"]);
                reviewControl.NotHelpfulCount = Convert.ToInt32(row["NotHelpfulCount"]);

                if (row["AtmosphereRating"] == DBNull.Value)
                {
                    reviewControl.AtmosphereRating = 0;

                }
                else
                {
                    reviewControl.AtmosphereRating = Convert.ToInt32(row["AtmosphereRating"]);

                }

                if (row["VisitDate"] != DBNull.Value)
                {
                    reviewControl.VisitDate = Convert.ToDateTime(row["VisitDate"]);

                }

                if (row["ReviewPhoto"] != DBNull.Value)
                {
                    reviewControl.ReviewPhoto = (byte[])row["ReviewPhoto"];
                    reviewControl.ReviewPhotoType = row["ReviewPhotoType"].ToString();
                }

                if (row["ResponseText"] != DBNull.Value)
                {
                    reviewControl.ResponseText = row["ResponseText"].ToString();

                    if (row["ResponseDate"] != DBNull.Value)
                    {
                        reviewControl.ResponseDate = Convert.ToDateTime(row["ResponseDate"]);
                    }
                }

                reviewControl.DataBind();
                phReviews.Controls.Add(reviewControl);
            }
        }


        private void ShowWriteReviewLink(int restaurantID)
        {
            UserAccounts objUser = (UserAccounts)Session["User"];

            if (objUser == null)
            {
                lnkWriteReview.Text = "Log in to write a review";
                lnkWriteReview.NavigateUrl = "Login.aspx";
            }
            else if (objUser.IsReviewer)
            {
                lnkWriteReview.Text = "Write a Review";
                lnkWriteReview.NavigateUrl = "WriteReview.aspx?id=" + restaurantID;
            }
            else
            {
                lnkWriteReview.Visible = false;
            }
        }

        private void ShowReserveLink(int restaurantID)
        {
            UserAccounts objUser = (UserAccounts)Session["User"];

            if (objUser != null && objUser.IsRepresentative)
            {
                lnkReserve.Visible = false;

            }
            else
            {
                lnkReserve.NavigateUrl = "MakeReservation.aspx?id=" + restaurantID;
            }
        }

        private String GetImageUrl(Object thePath, String theDefaultUrl)
        {
            if (thePath == DBNull.Value || thePath.ToString() == "")
                return theDefaultUrl;
            return thePath.ToString();
        }

        private String GetText(Object theValue)
        {
            if (theValue == DBNull.Value || theValue.ToString() == "")
                return "Not listed";
            return theValue.ToString();
        }

        private double GetRating(Object theValue)
        {
            if (theValue == DBNull.Value)
                return 0;
            return Convert.ToDouble(theValue);
        }

        private void ShowMessage(String theTitle, String theText)
        {
            lblMessageTitle.Text = theTitle;
            lblMessageText.Text = theText;
            divMessage.Visible = true;
            divProfile.Visible = false;
        }

    }
}



