using Microsoft.AspNetCore.Mvc;
using WhatTheForkMVC.Classes;
using WhatTheForkMVC.Database;
using WhatTheForkMVC.Classes.Login;
using System.Data;
using WhatTheForkMVC.Classes.Dashboard;

namespace WhatTheForkMVC.Controllers
{
    public class DashboardController : Controller
    {
        ReviewDB objReviewDB = new ReviewDB();
        ReservationDB objReservDB = new ReservationDB();
        RestaurantDB objRestaurant = new RestaurantDB();
         
         public IActionResult Index()
         {
             if (!SessionUser.IsLoggedIn(HttpContext.Session))
             {
                 return RedirectToAction("Login", "Account");
             }

             UserAccounts objUser = SessionUser.GetUser(HttpContext.Session);
             List<DashboardCard> lstCards = new List<DashboardCard>();

             if (objUser.IsReviewer)
             {
                AddReviewerCards(objUser, lstCards);
             }
             if (objUser.IsRepresentative)
             {
                AddRepresentativeCards(objUser, lstCards);
             }
             return View(lstCards);
         }
        

        private void AddReviewerCards(UserAccounts user, List<DashboardCard> lstCards)
        {
            ViewData["Eyebrow"] = "Reviewer Dashboard";
            ViewData["Welcome"] = "Welcome Back, " + user.FirstName;
            ViewData["Instructions"] = "Select a card to see reviews or add a new review.";
            String reviewStats = "";

            try
            {
                int reviewerID = objReviewDB.GetReviewerID(user.UserID);
                int reviewCount = 0;



                if (reviewerID == objReviewDB.GetReviewerID(user.UserID))
                {
                    DataSet dsReviews = objReviewDB.GetReviewsByReviewer(reviewerID);
                    reviewCount = dsReviews.Tables[0].Rows.Count;
                }

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
            catch (Exception ex)
            {
                ViewData["Error"] = "Error retrieving restaurant information";
                return;

            }

            if (restaurantID == 0)
            {
                lstCards.Add(new DashboardCard("Get started", "Add Your Restaurant", "No Restaurant",
                    "Add your restaurant's details, hours, social media links and photos.",
                    "~/Restaurant/Manage", "Add my restaurant"));

                lstCards.Add(new DashboardCard("Bookings", "Reservations", "No Restaurant",
                    "Once a restaurant is listed, diners can book a table and you can confirm reservations here.",
                    "~/Restaurant/Manage", "Add my restaurant"));

                lstCards.Add(new DashboardCard("Feedback", "Reviews to Respond To", "No Restaurant",
                    "Once your restaurant is listed, you can reply to  reviews.",
                    "~/Restaurant/Manage", "Add my restaurant"));
                return;
            }

            lstCards.Add(new DashboardCard("Your listing", "My Restaurant", restaurantName,
               "Update your restaurant's details, hours, social links and photos.",
               "~/Restaurant/Manage", "Edit my restaurant"));

            lstCards.Add(new DashboardCard("Bookings", "Reservations", pendingCount + "Confirmation Waiting",
                upcomingCount + " total upcoming. Customer gets email when you confirm, change, or cancel a reservation.",
                "~/Reservation/Manage", "Manage reservations"));
            String reviewStat = "";
            if (unanswerCount == 1)
            {
                reviewStat= "1 review waiting for a response";
            }
            else
            {
                reviewStat = unanswerCount + "reviews waiting for a response";
            }


            lstCards.Add(new DashboardCard("Feedback", "Reviews to Respond To", reviewStat,
                "Respond to reviewers. Your response displays under their review on your profile.",
                "~/Review/RestaurantReviews", "Respond to reviews"));
        }
    }
}
