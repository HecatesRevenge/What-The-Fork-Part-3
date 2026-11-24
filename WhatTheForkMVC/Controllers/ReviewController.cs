using Microsoft.AspNetCore.Mvc;
using System.Data;
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
        RestaurantDB objRestaurantDB = new RestaurantDB();
        ReviewValidator objValidator = new ReviewValidator();

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
                ViewData["Error"] = "Error deleting review. Please try again later.";
            }
            return ShowReviews(objUser);
        }

        public IActionResult WriteReview(int id, int reviewID)
        {
            if (!SessionUser.IsLoggedIn(HttpContext.Session))
            {
                return RedirectToAction("Login", "Account");
            }

            UserAccounts objUser = SessionUser.GetUser(HttpContext.Session);
            if (!objUser.IsReviewer)
            {
                return ShowReviewMessage("Reviewers only",
                    "Only reviewer accounts can write reviews. Restaurant representatives can respond to reviews from their dashboard.",
                    "~/Dashboard/Index", "Back to my dashboard");
            }

            try
            {
                int reviewerID = objReviewDB.GetReviewerID(objUser.UserID);
                if (reviewerID == 0)
                {
                    return ShowReviewMessage("Reviewer profile not found",
                        "Your account is not linked to a reviewer profile, so it can't write reviews.",
                        "~/Dashboard/Index", "Back to my dashboard");
                }

                if (reviewID > 0)
                {
                    return LoadReview(reviewID, reviewerID);
                }
                return LoadRestaurant(id);
            }
            catch (Exception ex)
            {
                return ShowReviewMessage("Something went wrong",
                    "We couldn't load this page right now. Please try again later.",
                    "~/Dashboard/Index", "Back to my dashboard");
            }
        }

        [HttpPost]
        public IActionResult WriteReview(int reviewID, int restaurantID, String restaurantName,
            int foodRating, int serviceRating, int atmosphereRating, int priceRating,
            String visitDate, String visitTime, String title, String text, String removePhoto, IFormFile photo)
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

            if (visitDate == null)
            {
                visitDate = "";
            }
            if (visitTime == null)
            {
                visitTime = "";
            }
            if (title == null)
            {
                title = "";
            }
            if (text == null)
            {
                text = "";
            }

            visitDate = visitDate.Trim();
            visitTime = visitTime.Trim();
            title = title.Trim();
            text = text.Trim();

            Review objReview = new Review();
            objReview.ReviewID = reviewID;
            objReview.RestaurantID = restaurantID;
            objReview.RestaurantName = restaurantName;
            objReview.ReviewTitle = title;
            objReview.ReviewText = text;
            objReview.FoodRating = foodRating;
            objReview.ServiceRating = serviceRating;
            objReview.AtmosphereRating = atmosphereRating;
            objReview.PriceRating = priceRating;

            String errors = objValidator.CheckReview(foodRating, serviceRating, atmosphereRating, priceRating,
                visitDate, visitTime, title, text);

            if (photo != null)
            {
                errors = errors + objValidator.CheckPhoto(photo.FileName, (int)photo.Length);
            }

            if (errors != "")
            {
                if (photo != null)
                {
                    errors = errors + "Please choose your photo again.";
                }

                ViewData["Error"] = errors;
                return ShowReviewForm(objReview, visitDate, visitTime);
            }

            objReview.VisitDate = DateTime.Parse(visitDate + " " + visitTime);

            byte[] photoData = null;
            String photoType = "";
            if (photo != null)
            {
                MemoryStream memStream = new MemoryStream();
                photo.CopyTo(memStream);
                photoData = memStream.ToArray();
                photoType = photo.ContentType;
            }

            try
            {
                ReviewApiClient objReviewApi = new ReviewApiClient(Request);
                objReview.ReviewerID = objReviewDB.GetReviewerID(objUser.UserID);

                if (reviewID == 0)
                {
                    objReview.ReviewPhoto = photoData;
                    objReview.ReviewPhotoType = photoType;

                    if (!objReviewApi.AddReview(objReview))
                    {
                        ViewData["Error"] = "Your review couldn't be posted. Please try again later.";
                        return ShowReviewForm(objReview, visitDate, visitTime);
                    }
                    HttpContext.Session.SetString("ReviewMessage", "Thanks! Your review of " + restaurantName + " was posted.");
                }
                else
                {
                    if (!objReviewApi.UpdateReview(objReview))
                    {
                        ViewData["Error"] = "Your changes couldn't be saved. The review may have been deleted.";
                        return ShowReviewForm(objReview, visitDate, visitTime);
                    }

                    if (photoData != null)
                    {
                        objReviewDB.UpdateReviewPhoto(reviewID, objReview.ReviewerID, photoData, photoType);
                    }
                    else if (removePhoto != null)
                    {
                        objReviewDB.UpdateReviewPhoto(reviewID, objReview.ReviewerID, null, "");
                    }

                    HttpContext.Session.SetString("ReviewMessage", "Your review of " + restaurantName + " was updated.");
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Your review couldn't be saved right now. Please try again later.";
                return ShowReviewForm(objReview, visitDate, visitTime);
            }

            return RedirectToAction("MyReviews");
        }

        private IActionResult LoadRestaurant(int restaurantID)
        {
            DataSet dsRestaurant = objRestaurantDB.GetRestaurant(restaurantID);
            if (dsRestaurant.Tables[0].Rows.Count == 0)
            {
                return ShowReviewMessage("Restaurant not found",
                    "We couldn't find that restaurant. Select it from the search results and click Write a Review.",
                    "~/Restaurant/Search", "Find a restaurant");
            }

            Review objReview = new Review();
            objReview.RestaurantID = restaurantID;
            objReview.RestaurantName = dsRestaurant.Tables[0].Rows[0]["RestaurantName"].ToString();

            return ShowReviewForm(objReview, "", "");
        }

        private IActionResult LoadReview(int reviewID, int reviewerID)
        {
            ReviewApiClient objReviewApi = new ReviewApiClient(Request);
            Review objReview = objReviewApi.GetReview(reviewID, reviewerID);
            if (objReview == null)
            {
                return ShowReviewMessage("Review not found",
                    "We couldn't find that review. It may have been deleted.",
                    "~/Review/MyReviews", "Back to my reviews");
            }

            DataSet dsRestaurant = objRestaurantDB.GetRestaurant(objReview.RestaurantID);
            if (dsRestaurant.Tables[0].Rows.Count > 0)
            {
                objReview.RestaurantName = dsRestaurant.Tables[0].Rows[0]["RestaurantName"].ToString();
            }

            return ShowReviewForm(objReview, objReview.VisitDate.ToString("yyyy-MM-dd"), objReview.VisitDate.ToString("HH:mm"));
        }

        private IActionResult ShowReviewForm(Review review, String visitDate, String visitTime)
        {
            if (review.ReviewID > 0)
            {
                ViewData["Eyebrow"] = "Edit your review";
                ViewData["FormTitle"] = "Change your review";
                ViewData["ButtonText"] = "Save changes";
                ViewData["CancelUrl"] = "~/Review/MyReviews";
            }
            else
            {
                ViewData["Eyebrow"] = "Write a review";
                ViewData["FormTitle"] = "Your review";
                ViewData["ButtonText"] = "Post review";
                ViewData["CancelUrl"] = "~/Restaurant/Profile/" + review.RestaurantID;
            }

            ViewData["VisitDate"] = visitDate;
            ViewData["VisitTime"] = visitTime;

            return View("WriteReview", review);
        }

        private IActionResult ShowReviewMessage(String title, String text, String linkUrl, String linkText)
        {
            ViewData["MessageTitle"] = title;
            ViewData["MessageText"] = text;
            ViewData["MessageLink"] = linkUrl;
            ViewData["MessageLinkText"] = linkText;

            return View("WriteReview", new Review());
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
