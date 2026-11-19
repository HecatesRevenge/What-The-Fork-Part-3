using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WhatTheForkMVC.Classes.Reviews;
using WhatTheForkMVC.Database;

namespace WhatTheForkMVC.Controllers
{
    [Route("applications/json")]
    [Route("api/Reviews")]

    public class ReviewsApiController : ControllerBase
    {
        ReviewDB objReviewDB = new ReviewDB();
        ReviewValidator objValidator = new ReviewValidator();


        [HttpGet("Restaurant/{restaurantID}")]
        public List<Review> GetReviewsByRestaurant(int restaurantID)
        {
            return objReviewDB.GetReviewList(restaurantID);
        }

        [HttpGet("User/{userID}")]
        public List<Review> GetReviewsByUser(int userID)
        {
            int reviewerID = objReviewDB.GetReviewerID(userID);

            if (reviewerID == 0)
            {
                return new List<Review>();
            }
            return objReviewDB.GetReviewListByReviewer(reviewerID);
        }

        [HttpGet("{id}/{reviewerID}")]
        public Review GetReview(int id, int reviewerID)
        {
            return objReviewDB.GetReviewByID(id, reviewerID);
        }




        [HttpPost]
        public Boolean AddReview([FromBody] Review review)
        {
            return SaveNewReview(review);
        }

        [HttpPost("Form")]
        public Boolean AddReviewForm([FromForm] Review review)
        {
            return SaveNewReview(review);
        }


        [HttpPut("{id}")]
        public Boolean UpdateReview(int id, [FromBody] Review review)
        {
            return SaveChangedReview(id, review);
        }

        [HttpPut("Form/{id}")]
        public Boolean UpdateReviewForm(int id, [FromForm] Review review)
        {
            return SaveChangedReview(id, review);
        }

        [HttpDelete("{id}/{reviewerID}")]
        public Boolean DeleteReview(int id, int reviewerID)
        {
            if (objReviewDB.DeleteReview(id, reviewerID) > 0)
            {
                return true;
            }
            return false;
        }


        private Boolean SaveNewReview(Review review)
        {
            if (!IsValid(review))
            {
                return false;
            }

            if (objReviewDB.AddReview(review) > 0)
            {
                return true;
            }
            return false;

        }

        private Boolean SaveChangedReview(int id, Review review)
        {
            if (!IsValid(review))
            {
                return false;
            }
            if (objReviewDB.AddReview(review) > 0)
            {
                return true;
            }
            return false;
        }



        private Boolean IsValid(Review review)
        {
            if (review == null || review.RestaurantID == 0 || review.ReviewerID == 0)
            {
                return false;
            }

            if (review.ReviewTitle == null)
            {
                review.ReviewTitle = "";
            }

            if (review.ReviewText == null)
            {
                review.ReviewText = "";
            }

            String errors = objValidator.CheckReview(review.FoodRating, review.ServiceRating, review.AtmosphereRating, review.PriceRating,
                 review.VisitDate.ToString("yyyy-MM-dd"), review.VisitDate.ToString("HH:mm"),
                review.ReviewTitle.Trim(), review.ReviewText.Trim());

            if (errors == "")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
