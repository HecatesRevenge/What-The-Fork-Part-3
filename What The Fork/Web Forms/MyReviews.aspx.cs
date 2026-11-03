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
    public partial class MyReviews : System.Web.UI.Page
    {
        ReviewDB odjReviewDB = new ReviewDB();

        protected void Page_Load(object sender, EventArgs e)
        {

            UserAccounts odjUser = (UserAccounts)Session["User"];
            litHeader.Text = UiHeader.GetHeader(odjUser);
            litFooter.Text = UiFooter.GetFooter();

            if (odjUser == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!odjUser.IsReviewer)
            {
                Response.Redirect("Dashboard.aspx");
                return;
            }

            if (!IsPostBack)
            {
                if (Session["ReviewMessage"] != null)
                {
                    ShowMessage(Session["ReviewMessage"].ToString());
                    Session.Remove("ReviewMessage");
                }
                ShowReviews();
            }



        }

        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            divConfirmDelete.Visible = false;

            if (ViewState["DeleteReviewID"] == null)
                return;

            int reviewID = int.Parse(ViewState["DeleteReviewID"].ToString());
            ViewState.Remove("DeleteReviewID");
            UserAccounts odjUser = (UserAccounts)Session["User"];

            try
            {
                // DeleteReview only deletes the review if this reviewer wrote it
                int reviewerID = odjReviewDB.GetReviewerID(odjUser.UserID);
                if (odjReviewDB.DeleteReview(reviewID, reviewerID) == 1)
                    ShowMessage("Your review was deleted.");
                else
                    ShowError("That review couldn't be deleted. It may already be gone.");
            }
            catch (Exception ex)
            {
                ShowError("Your review couldn't be deleted right now. Please try again later.");
            }

            ShowReviews();
        }

        protected void btnCancelDelete_Click(object sender, EventArgs e)
        {
            ViewState.Remove("DeleteReviewID");
            divConfirmDelete.Visible = false;

        }

        protected void gvMyReviews_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMyReviews.PageIndex = e.NewPageIndex;
            ShowReviews();
        }
        protected void gvMyReviews_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Page")
                return;

            int rowIndex = int.Parse(e.CommandArgument.ToString());
            String reviewID = gvMyReviews.DataKeys[rowIndex].Values["ReviewID"].ToString();
            String restaurantID = gvMyReviews.DataKeys[rowIndex].Values["RestaurantID"].ToString();
            String restaurantName = gvMyReviews.DataKeys[rowIndex].Values["RestaurantName"].ToString();

            if (e.CommandName == "ViewRestaurant")
            {
                Response.Redirect("RestaurantProfile.aspx?id=" + restaurantID);
            }
            else if (e.CommandName == "EditReview")
            {
                Response.Redirect("WriteReview.aspx?reviewid=" + reviewID);
            }
            else if (e.CommandName == "DeleteReview")
            {
                ViewState["DeleteReviewID"] = reviewID;
                lblConfirmDelete.Text = "Delete your review of " + Server.HtmlEncode(restaurantName) + "? This can't be undone.";
                divConfirmDelete.Visible = true;
                lblMessage.Visible = false;
            }
        }



        private void ShowReviews()
        {
            UserAccounts odjUser = (UserAccounts)Session["User"];

            try
            {
                int reviewerId = odjReviewDB.GetReviewerID(odjUser.UserID);
                DataSet dsReviews = odjReviewDB.GetReviewsByReviewer(reviewerId);

                DataTable odjTable = dsReviews.Tables[0];
                odjTable.Columns.Add("PriceLevel", typeof(String));
                foreach (DataRow row in odjTable.Rows)
                {
                    row["PriceLevel"] = RestaurantDB.GetDollarSigns(Convert.ToDouble(row["PriceRating"]));
                }

                int count = odjTable.Rows.Count;

                if (count == 1)
                {
                    lblReviewCount.Text = "1 Review";
                }
                else
                {
                    lblReviewCount.Text = count + "reviews";
                }

                gvMyReviews.DataSource = dsReviews;
                gvMyReviews.DataBind();

            }
            catch (Exception ex)
            {
                ShowError("We couldn't load your reviews right now. Please try again later.");
            }
        }

        private void ShowMessage(String theMessage)
        {
            lblMessage.Text = Server.HtmlEncode(theMessage);
            lblMessage.Visible = true;
        }

        private void ShowError(String theMessage)
        {
            lblValidation.Text = theMessage;
            lblValidation.Visible = true;
        }
    }
}