using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using WhatTheForkMVC.Classes.Reviews;
using WhatTheForkMVC.Database;
using static System.Net.Mime.MediaTypeNames;

namespace WhatTheForkMVC.Views.Account
{
    public class RecommandationTools
    {
        private int userID;
        public RecommandationTools(int userID)
        {
            this.userID = userID;
        }

        // This attriubute is what claude reads to decide what method to call

        /*
         * This Method returns a string containing the reviews the current user has written, with ratings from 1 to 5.
         * This method can then be read by claude and used to answer questions about the user's reviews.
         * Main use case is giving claude more context about the user when they ask for restaurant recommendations.
         */
        [Description("Gets the reviews the current user has written, with ratings from 1 to 5.")]
        public String GetUserReviews()
        {
            ReviewDB objReview = new ReviewDB();

            int reviewerID=objReview.GetReviewerID(userID);
            if(reviewerID==0)
            {
                return "User has not written any reviews yet.";
            }

            List<Review> reviews = objReview.GetReviewListByReviewer(reviewerID);
            if(reviews.Count == 0)
            {
                return "User has not written any reviews yet.";
            }

            StringBuilder sb = new StringBuilder();
            foreach (Review review in reviews)
            {
                sb.AppendLine("RestaurantID " + review.RestaurantID + ": " + review.RestaurantName
                    + " | \"" + review.ReviewTitle + "\""
                    + " | Food " + review.FoodRating + ", Service " + review.ServiceRating
                    + ", Atmosphere " + review.AtmosphereRating + ", Price " + review.PriceRating
                    + " | Reviewed " + review.ReviewDate.ToShortDateString());
            }
            return sb.ToString();
        }
    }
}
