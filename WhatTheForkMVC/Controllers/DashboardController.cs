using Microsoft.AspNetCore.Mvc;
using System.Data;
using WhatTheForkMVC.Classes;
using WhatTheForkMVC.Classes.Dashboard;
using WhatTheForkMVC.Classes.Login;
using WhatTheForkMVC.Classes.Restaurant;
using WhatTheForkMVC.Classes.Reviews;
using WhatTheForkMVC.Database;

namespace WhatTheForkMVC.Controllers
{
    public class DashboardController : Controller
    {
        ReviewDB objReviewDB = new ReviewDB();
        ReservationDB objReservDB = new ReservationDB();
        RestaurantDB objRestaurant = new RestaurantDB();
        SearchSerializer objSerializer = new SearchSerializer();

        public IActionResult Index()
        {
            UserAccounts objUser = SessionUser.GetUser(HttpContext.Session);
            List<DashboardCard> lstCards = new List<DashboardCard>();

            if (objUser == null)
            {
                AddVisitorCards(lstCards);
            }
            else if (objUser.IsReviewer)
            {
                AddReviewerCards(objUser, lstCards);
            }
            else if (objUser.IsRepresentative)
            {
                AddRepresentativeCards(objUser, lstCards);
            }
            return View(lstCards);
        }

        private void AddVisitorCards(List<DashboardCard> lstCards)
        {
            ViewData["Eyebrow"] = "Start Here";
            ViewData["Welcome"] = "Welcome to What the Fork!";
            ViewData["Instructions"] = "Search restaurants and read reviews without an account. Log in or register to write reviews or manage a restaurant";

            lstCards.Add(new DashboardCard("Explore", "Find Restaurants", "Search by cuisine and location",
                "Choose one or more cuisines and a city, then compare ratings for food, service and atmosphere.",
                "~/Restaurant/Search", "Search restaurants"));

            lstCards.Add(new DashboardCard("Members", "Log In", "Already have an account?",
                "Reviewers can write and manage their reviews. Representatives can manage their restaurant and reservations.",
                "~/Account/Login", "Log in"));

            lstCards.Add(new DashboardCard("New here", "Register", "It's free",
                "Create a reviewer account to share your visits, or a representative account to list your restaurant.",
                "~/Account/Register", "Create an account"));
        }

        private void AddReviewerCards(UserAccounts user, List<DashboardCard> lstCards)
        {
            ViewData["Eyebrow"] = "Reviewer Dashboard";
            ViewData["Welcome"] = "Welcome Back, " + user.FirstName;
            ViewData["Instructions"] = "Select a card to see reviews or add a new review.";
            String reviewStats = "";

            try
            {
                ReviewApiClient objReviewApi = new ReviewApiClient(Request);
                int reviewCount = objReviewApi.GetReviewsForUser(user.UserID).Count;

                if (reviewCount == 1)
                {
                    reviewStats = "1 Review";
                }

                else
                {
                    reviewStats = reviewCount + " Reviews";
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error number of retrieving reviews";
            }

            lstCards.Add(new DashboardCard("Your Reviews", "My Reviews", reviewStats,
                "Show all reviews, edit past reviews, or delete reviews",
                "~/Review/MyReviews", "View Reviews"));

            lstCards.Add(new DashboardCard("Share Your Visit", "Write Review", "Food, Service, Atmostpher, Price",
            "Search for restaurants, click on profile, select Write a Review",
            "~/Restaurant/Search", "Search Restaurants to Write a Review"));

            AddLastSearchCard(user, lstCards);
        }

        private void AddLastSearchCard(UserAccounts user, List<DashboardCard> lstCards)
        {
            try
            {
                Byte[] searchData = objRestaurant.GetSavedSearch(user.UserID);
                if (searchData == null)
                {
                    return;
                }

                SearchCriteria objCriteria = objSerializer.Deserialize(searchData);

                lstCards.Add(new DashboardCard("Pick up where you left off", "Your Last Search", objCriteria.Description,
                    "Saved " + objCriteria.SavedDate.ToString("MMM d, yyyy 'at' h:mm tt") + ". Run it again to see what's new.",
                    "~/Restaurant/LastSearch", "Run it again"));
            }
            catch (Exception ex)
            {
            }
        }

        private void AddRepresentativeCards(UserAccounts user, List<DashboardCard> lstCards)
        {
            ViewData["Eyebrow"] = "Representative Dashboard";
            ViewData["Welcome"] = "Welcome Back, " + user.FirstName;
            ViewData["Instructions"] = "Select a card to update your restaurant profile, manage reservations, or view and respond to reviews.";

            int restaurantID = 0;
            String restaurantName = "";
            int pendingCount = 0;
            int upcomingCount = 0;
            int unanswerCount = 0;

            try
            {
                DataSet dsRestaurant = objRestaurant.GetResturantByRep(user.UserID);

                if (dsRestaurant.Tables[0].Rows.Count > 0)
                {
                    DataRow restaurantRow = dsRestaurant.Tables[0].Rows[0];
                    restaurantID = Convert.ToInt32(restaurantRow["RestaurantID"]);
                    restaurantName = restaurantRow["RestaurantName"].ToString();

                    DataSet dsReversations = objReservDB.GetReservations(restaurantID);

                    foreach (DataRow row in dsReversations.Tables[0].Rows)
                    {
                        if (Convert.ToDateTime(row["ReservationDate"]) >= DateTime.Now)
                        {
                            upcomingCount = upcomingCount + 1;

                            if (row["Status"].ToString() == "Pending")
                            {
                                pendingCount = pendingCount + 1;
                            }
                        }
                    }

                    DataSet dsReviews = objReviewDB.GetReviews(restaurantID);
                    foreach (DataRow row in dsReviews.Tables[0].Rows)
                    {
                        if (row["ResponseText"] == DBNull.Value || row["ResponseText"].ToString().Trim() == "")
                        {
                            unanswerCount = unanswerCount + 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Error retrieving restaurant information";
                return;

            }

            if (restaurantID == 0)
            {
                lstCards.Add(new DashboardCard("Get started", "Add Your Restaurant", "No Restaurant",
                    "Add your restaurant's details, hours, social media links and photos.",
                    "~/Restaurant/ManageReservation", "Add my restaurant"));

                lstCards.Add(new DashboardCard("Bookings", "Reservations", "No Restaurant",
                    "Once a restaurant is listed, diners can book a table and you can confirm reservations here.",
                    "~/Restaurant/ManageReservation", "Add my restaurant"));

                lstCards.Add(new DashboardCard("Feedback", "Reviews to Respond To", "No Restaurant",
                    "Once your restaurant is listed, you can reply to  reviews.",
                    "~/Restaurant/ManageReservation", "Add my restaurant"));
                return;
            }

            lstCards.Add(new DashboardCard("Your listing", "My Restaurant", restaurantName,
               "Update your restaurant's details, hours, social links and photos.",
               "~/Restaurant/ManageReservation", "Edit my restaurant"));

            lstCards.Add(new DashboardCard("Bookings", "Reservations", pendingCount + " Confirmation Waiting",
                upcomingCount + " total upcoming. Customer gets email when you confirm, change, or cancel a reservation.",
                "~/Reservation/ManageReservation", "Manage reservations"));
            String reviewStat = "";
            if (unanswerCount == 1)
            {
                reviewStat = "1 review waiting for a response";
            }
            else
            {
                reviewStat = unanswerCount + " reviews waiting for a response";
            }


            lstCards.Add(new DashboardCard("Feedback", "Reviews to Respond To", reviewStat,
                "Respond to reviewers. Your response displays under their review on your profile.",
                "~/Review/RestaurantReviews", "Respond to reviews"));
        }
    }
}
