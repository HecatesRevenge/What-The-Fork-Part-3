using System;
using System.Collections;
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
    public partial class Dashboard : System.Web.UI.Page
    {

        ReviewDB odjReviewDB = new ReviewDB();
        ReservationDB odjReservationDB = new ReservationDB();
        RestaurantDB odjRestaurantDB = new RestaurantDB();

        ArrayList arrayCards = new ArrayList();
        protected void Page_Load(object sender, EventArgs e)
        {
            UserAccounts odjUser = (UserAccounts)Session["User"];

            litHeader.Text = UiHeader.GetHeader(odjUser);

            litFooter.Text = UiFooter.GetFooter();

            if (!IsPostBack)
            {
                if (odjUser == null)
                    AddVisitorCards();
                else if (odjUser.IsReviewer)
                    AddReviewerCards(odjUser);
                else if (odjUser.IsRepresentative)
                    AddRepresentativeCards(odjUser);

                ShowCards();

            }

        }


        private void AddVisitorCards()
        {
            lblEyebrow.Text = "Start Here";
            lblWelcome.Text = "Welcome to What the Fork!";
            lblInstructions.Text = "Search restaurants and read reviews without an account. Log in or register to write reviews or manage a restaurant";

            arrayCards.Add(new DashboardCard("Explore", "Find Restaurants", "Search by cuisine and location",
                "Choose one or more cuisines and a city, then compare ratings for food, service and atmosphere.",
                "Search.aspx", "Search restaurants"));

            arrayCards.Add(new DashboardCard("Members", "Log In", "Already have an account?",
                "Reviewers can write and manage their reviews. Representatives can manage their restaurant and reservations.",
                "Login.aspx", "Log in"));

            arrayCards.Add(new DashboardCard("New here", "Register", "It's free",
                "Create a reviewer account to share your visits, or a representative account to list your restaurant.",
                "Register.aspx", "Create an account"));

        }

        private void AddReviewerCards(UserAccounts user)
        {
            lblEyebrow.Text = "Reviewer Dashboard";
            lblWelcome.Text = "Welcome Back, " + Server.HtmlEncode(user.FirstName) + "!";
            lblInstructions.Text = "Pick a card below to see the reviews you've written or to review a restaurant you visited.";

            String reviewStats = "";

            try
            {
                int reviewerID = odjReviewDB.GetReviewerID(user.UserID);
                int reviewCount = 0;

                if (reviewerID > 0)
                {
                    DataSet dsReviews = odjReviewDB.GetReviewsByReviewer(reviewerID);
                    reviewCount = dsReviews.Tables[0].Rows.Count;
                }

                if (reviewCount == 1)
                {
                    reviewStats = "You have written 1 review.";

                }
                else
                {
                    reviewStats = "You have written " + reviewCount + " reviews.";
                }
            }
            catch (Exception ex)
            {
                ShowError("We are having trouble loading your review counts right now. Please try again later");
            }

            arrayCards.Add(new DashboardCard("Your reviews", "My Reviews", reviewStats,
               "See every review you've written, and edit or delete any of them.",
               "MyReviews.aspx", "View my reviews"));

            arrayCards.Add(new DashboardCard("Share a visit", "Write a Review", "Food, service, atmosphere and price",
                "Search for the restaurant you visited, open its profile, and click Write a Review.",
                "Search.aspx", "Find a restaurant to review"));
        }

        private void AddRepresentativeCards(UserAccounts user)
        {
            lblEyebrow.Text = "Representative dashboard";
            lblWelcome.Text = "Welcome back, " + Server.HtmlEncode(user.FirstName);
            lblInstructions.Text = "Pick a card below to update your restaurant, manage reservations, or respond to reviews.";

            int restaurantID = 0;
            String restaurantName = "";
            int pendingCount = 0;
            int upcomingCount = 0;
            int unansweredCount = 0;

            try
            {
                DataSet dsRestaurant = odjRestaurantDB.GetResturantByRep(user.UserID);

                if (dsRestaurant.Tables[0].Rows.Count > 0)
                {
                    DataRow restaurantRow = dsRestaurant.Tables[0].Rows[0];
                    restaurantID = Convert.ToInt32(restaurantRow["RestaurantID"]);
                    restaurantName = restaurantRow["RestaurantName"].ToString();

                    DataSet dsReservations = odjReservationDB.GetReservations(restaurantID);
                    foreach (DataRow row in dsReservations.Tables[0].Rows)
                    {
                        if (Convert.ToDateTime(row["ReservationDate"]) >= DateTime.Now)
                        {
                            upcomingCount = upcomingCount + 1;
                            if (row["Status"].ToString() == "Pending")
                                pendingCount = pendingCount + 1;
                        }
                    }

                    // Count the reviews that don't have a response yet
                    DataSet dsReviews = odjReviewDB.GetReviews(restaurantID);
                    foreach (DataRow row in dsReviews.Tables[0].Rows)
                    {
                        if (row["ResponseText"] == DBNull.Value || row["ResponseText"].ToString().Trim() == "")
                            unansweredCount = unansweredCount + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("We couldn't load your restaurant's details right now. Please try again later.");
                return;
            }

            if (restaurantID == 0)
            {
                arrayCards.Add(new DashboardCard("Get started", "Add Your Restaurant", "No restaurant yet",
                    "Add your restaurant's details, hours, social links and photos so diners can find it.",
                    "ManageRestaurant.aspx", "Add my restaurant"));

                arrayCards.Add(new DashboardCard("Bookings", "Reservations", "Add your restaurant first",
                    "Once your restaurant is listed, diners can book a table and you can confirm reservations here.",
                    "ManageRestaurant.aspx", "Add my restaurant"));

                arrayCards.Add(new DashboardCard("Feedback", "Reviews to Respond To", "Add your restaurant first",
                    "Once your restaurant is listed, you can reply to what reviewers say about it.",
                    "ManageRestaurant.aspx", "Add my restaurant"));
                return;
            }

            arrayCards.Add(new DashboardCard("Your listing", "My Restaurant", Server.HtmlEncode(restaurantName),
                "Update your restaurant's details, hours, social links and photos.",
                "ManageRestaurant.aspx", "Edit my restaurant"));

            arrayCards.Add(new DashboardCard("Bookings", "Reservations", pendingCount + " waiting to be confirmed",
                upcomingCount + " upcoming in total. Confirm, change or cancel them, and the customer gets an email.",
                "ManageReservations.aspx", "Manage reservations"));

            String reviewStat = "";
            if (unansweredCount == 1)
                reviewStat = "1 review without a response";
            else
                reviewStat = unansweredCount + " reviews without a response";

            arrayCards.Add(new DashboardCard("Feedback", "Reviews to Respond To", reviewStat,
                "Thank reviewers or answer their concerns. Your response shows under their review on your profile.",
                "RestaurantReviews.aspx", "Respond to reviews"));
        }
        private void ShowError(String theMessage)
        {
            lblValidation.Text = theMessage;
            lblValidation.Visible = true;
        }
        private void ShowCards()
        {
            rptCards.DataSource = arrayCards;
            rptCards.DataBind();
        }

    }
}