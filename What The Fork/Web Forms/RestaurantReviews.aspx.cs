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
    public partial class RestaurantReviews : System.Web.UI.Page
    {
        ReviewDB odjReviewDB = new ReviewDB();
        RestaurantDB odjRestaurantDB = new RestaurantDB();

        const int MaxResponseLength = 2000;

        int restaurantID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            UserAccounts odjUser = (UserAccounts)Session["User"];

            litHeader.Text = UiHeader.GetHeader(odjUser);
            litFooter.Text = UiFooter.GetFooter();

            if (odjUser == null)
            {
                ShowMessage("Log in to respond to reviews",
                    "You need a representative account to respond to your restaurant's reviews. Log in, or register for free.",
                    "Login.aspx", "Log in");
                return;
            }

            if (!odjUser.IsRepresentative)
            {
                ShowMessage("Representatives only",
                    "Only restaurant representative accounts can respond to reviews. Reviewers can manage their own reviews from their dashboard.",
                    "Dashboard.aspx", "Back to my dashboard");
                return;
            }

            try
            {
                DataSet dsRestaurant = odjRestaurantDB.GetResturantByRep(odjUser.UserID);

                if (dsRestaurant.Tables[0].Rows.Count == 0)
                {
                    ShowMessage("Add your restaurant first",
                        "Once your restaurant is listed, its reviews show up here so you can respond to them.",
                        "ManageRestaurant.aspx", "Add my restaurant");
                    return;
                }

                DataRow restaurantRow = dsRestaurant.Tables[0].Rows[0];
                restaurantID = Convert.ToInt32(restaurantRow["RestaurantID"]);

                lblRestaurantName.Text = Server.HtmlEncode(restaurantRow["RestaurantName"].ToString());

                ShowReviews();
            }
            catch (Exception ex)
            {
                ShowMessage("Something went wrong",
                    "We couldn't load your restaurant's reviews right now. Please try again later.",
                    "Dashboard.aspx", "Back to my dashboard");
            }
        }

        private void ShowReviews()
        {
            DataSet dsReviews = odjReviewDB.GetReviews(restaurantID);
            DataTable odjTable = dsReviews.Tables[0];

            odjTable.Columns.Add("VisitText", typeof(String));
            odjTable.Columns.Add("AtmosphereText", typeof(String));
            odjTable.Columns.Add("PhotoUrl", typeof(String));
            odjTable.Columns.Add("HasPhoto", typeof(bool));
            odjTable.Columns.Add("ResponseDateText", typeof(String));

            int unansweredCount = 0;

            for (int i = odjTable.Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = odjTable.Rows[i];

                bool hasResponse = row["ResponseText"] != DBNull.Value && row["ResponseText"].ToString().Trim() != "";
                if (!hasResponse)
                    unansweredCount = unansweredCount + 1;

                if (hasResponse && ddlFilter.SelectedValue == "Unanswered")
                {
                    odjTable.Rows.Remove(row);
                    continue;
                }

                if (row["VisitDate"] == DBNull.Value)
                    row["VisitText"] = "Posted " + Convert.ToDateTime(row["ReviewDate"]).ToString("MMMM d, yyyy");
                else
                    row["VisitText"] = "Visited " + Convert.ToDateTime(row["VisitDate"]).ToString("MMMM d, yyyy");

                if (row["AtmosphereRating"] == DBNull.Value)
                    row["AtmosphereText"] = "-";
                else
                    row["AtmosphereText"] = row["AtmosphereRating"].ToString();

                if (row["ReviewPhoto"] == DBNull.Value)
                {
                    row["PhotoUrl"] = "";
                    row["HasPhoto"] = false;
                }
                else
                {
                    row["PhotoUrl"] = "data:" + row["ReviewPhotoType"].ToString() + ";base64," + Convert.ToBase64String((byte[])row["ReviewPhoto"]);
                    row["HasPhoto"] = true;
                }

                if (hasResponse && row["ResponseDate"] != DBNull.Value)
                    row["ResponseDateText"] = "Responded " + Convert.ToDateTime(row["ResponseDate"]).ToString("MMMM d, yyyy");
                else
                    row["ResponseDateText"] = "";
            }

            if (unansweredCount == 1)
                lblReviewCount.Text = "1 review needs a response";
            else
                lblReviewCount.Text = unansweredCount + " reviews need a response";

            if (odjTable.Rows.Count == 0)
            {
                if (ddlFilter.SelectedValue == "Unanswered")
                    lblNoReviews.Text = "Every review has a response. Choose All reviews to see them.";
                else
                    lblNoReviews.Text = "Your restaurant doesn't have any reviews yet.";
                lblNoReviews.Visible = true;
            }
            else
            {
                lblNoReviews.Visible = false;
            }

            rptReviews.DataSource = odjTable;
            rptReviews.DataBind();
        }

        protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (restaurantID == 0)
                return;

            try
            {
                ShowReviews();
            }
            catch (Exception ex)
            {
                ShowError("We couldn't load your restaurant's reviews right now. Please try again later.");
            }
        }

        protected void rptReviews_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "SaveResponse" || restaurantID == 0)
                return;

            int reviewID = int.Parse(e.CommandArgument.ToString());
            TextBox txtResponse = (TextBox)e.Item.FindControl("txtResponse");
            String responseText = txtResponse.Text.Trim();

            if (responseText == "")
            {
                ShowError("Type a response before clicking Save response.");
                return;
            }

            if (responseText.Length > MaxResponseLength)
            {
                ShowError("Your response is " + responseText.Length + " characters long. Please keep it to " + MaxResponseLength + " characters or fewer.");
                return;
            }

            try
            {
                if (odjReviewDB.AddReviewResponse(reviewID, restaurantID, responseText) == 1)
                {
                    lblMessage.Text = "Your response was saved. It now shows under the review on your restaurant's page.";
                    lblMessage.Visible = true;
                    ShowReviews();
                }
                else
                {
                    ShowError("That response couldn't be saved. The review may have been removed.");
                }
            }
            catch (Exception ex)
            {
                ShowError("Your response couldn't be saved right now. Please try again later.");
            }
        }

        private void ShowMessage(String title, String text, String linkUrl, String linkText)
        {
            lblMessageTitle.Text = title;
            lblMessageText.Text = text;
            lnkMessage.NavigateUrl = linkUrl;
            lnkMessage.Text = linkText;
            divMessage.Visible = true;
            divReviews.Visible = false;
        }

        private void ShowError(String message)
        {
            lblValidation.Text = message;
            lblValidation.Visible = true;
        }
    }
}
