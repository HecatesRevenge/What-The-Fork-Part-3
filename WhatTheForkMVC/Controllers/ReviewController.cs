using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using WhatTheForkMVC.Classes;
using WhatTheForkMVC.Classes.Login;
using WhatTheForkMVC.Classes.Reviews;
using WhatTheForkMVC.Database;

namespace WhatTheForkMVC.Controllers
{
    public class ReviewController : Controller
    {
        ReviewDB objReviewDB = new ReviewDB();

        public IActionResult MyReviews()
        {
            if (!SessionUser.IsLoggedIn(HttpContext.Session))
            {
                return RedirectToAction("Login", "Account");
            }

            UserAccounts objUser = SessionUser.GetUser(HttpContext.Session);
            if (!objUser.IsReviewer)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            if (HttpContext.Session.GetString("ReviewMessage") != null)
            {
                ViewData["Message"] = HttpContext.Session.GetString("ReviewMessage");
                HttpContext.Session.Remove("ReviewMessage");
            }

            return ShowReviews(objUser);
        }

        public IActionResult ConfirmDelete(int reviewID, String restaurantName)
        {
            if (!SessionUser.IsLoggedIn(HttpContext.Session))
            {
                return RedirectToAction("Login", "Account");
            }

            UserAccounts objUser = SessionUser.GetUser(HttpContext.Session);
            if (!objUser.IsReviewer)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ViewData["DeleteReviewID"] = reviewID;
            ViewData["ConfirmDelete"] = "Delete your review of " + restaurantName + "? This can't be undone.";

            return ShowReviews(objUser);

        }

        [HttpPost]
        public IActionResult DeleteReview(int reviewID)
        {
            if (!SessionUser.IsLoggedIn(HttpContext.Session))
            {
                return RedirectToAction("Login", "Account");
            }
            UserAccounts objUser = SessionUser.GetUser(HttpContext.Session);
            if (!objUser.IsReviewer)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            try
            {
                int reviewerID = objReviewDB.GetReviewerID(objUser.UserID);
                ReviewsApiController objApi = new ReviewsApiController();
                if (objApi.DeleteReview(reviewID, reviewerID))
                {
                    ViewData["Message"] = "Your review was deleted.";
                }
                else
                {
                    ViewData["Error"] = "That review couldn't be deleted. It may already be deleted.";
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "We couldn't delete your review right now. Please try again later.";
            }
            return ShowReviews(objUser);
        }



        private IActionResult ShowReviews(UserAccounts objUser)
        {
            List<Review> reviews = new List<Review>();

            try
            {
                ReviewsApiController objApi = new ReviewsApiController();
                reviews = objApi.GetReviewsByUser(objUser.UserID);

                int count = reviews.Count;

                if (count == 1)
                {
                    ViewData["ReviewCount"] = "You have 1 review.";
                }
                else
                {
                    ViewData["ReviewCount"] = "You have " + count + " reviews.";
                }

            }
            catch (Exception ex)
            {
                ViewData["Error"] = "We couldn't load your reviews right now. Please try again later.";
            }

            return View("MyReviews", reviews);

        }

    }
}
