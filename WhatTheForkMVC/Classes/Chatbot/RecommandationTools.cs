using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;
using WhatTheForkMVC.Classes;


using WhatTheForkMVC.Classes.Restaurant;
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
        [Description(ToolDescriptions.GetMyReviews)]
        public String GetUserReviews()
        {
            ReviewDB objReview = new ReviewDB();

            int reviewerID = objReview.GetReviewerID(userID);
            if (reviewerID == 0)
            {
                return "User has not written any reviews yet.";
            }

            List<Review> reviews = objReview.GetReviewListByReviewer(reviewerID);
            if (reviews.Count == 0)
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


        [Description(ToolDescriptions.GetMySearchHistory)]
        public String GetUserSearchHistory()
        {
            SearchLogDB objSearchLog = new SearchLogDB();
            DataSet ds = objSearchLog.GetRecentSearches(userID);

            if (ds.Tables[0].Rows.Count == 0)
            {
                return "User has not searched for any restaurants yet.";
            }

            Dictionary<String, String> cuisineName = GetCuisineNames();
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                List<String> names = new List<String>();
                foreach (String id in row["CuisineIDs"].ToString().Split(','))
                {
                    if (cuisineName.ContainsKey(id))
                    {
                        names.Add(cuisineName[id]);
                    }
                }

                String locations = "all locations";
                if (row["City"].ToString() != "")
                {
                    locations = row["City"].ToString() + ", " + row["State"];
                }

                int maxPrice = Convert.ToInt32(row["MaxPrice"]);
                String price = "any price";
                if (maxPrice > 0)
                {
                    price = new String('$', maxPrice) + " or less";
                }

                sb.AppendLine(Convert.ToDateTime(row["SearchDate"]).ToShortDateString() + ": " +
                    String.Join(", ", names) + " in " + locations + ", " + price);
            }

            return sb.ToString();

        }

        [Description(ToolDescriptions.GetSearchOptions)]
        public String GetSearchOptions()
        {
            StringBuilder sb = new StringBuilder("Cuisines (ID: name):\n");

            foreach (KeyValuePair<String, String> cuisine in
            GetCuisineNames())
            {
                sb.AppendLine(cuisine.Key + ": " + cuisine.Value);
            }
            sb.AppendLine("Locations:");

            RestaurantDB objRestaurantDB = new RestaurantDB();
            foreach (DataRow row in objRestaurantDB.GetLocations().Tables[0].Rows)
            {
                sb.AppendLine(row["City"] + ", " + row["State"]);
            }
            return sb.ToString();
        }

        [Description("Searches restaurants. Returns up to 10 results, best rated first.")]
       

        private Dictionary<String, String> GetCuisineNames()
        {
            Dictionary<String, String> names = new Dictionary<String, String>();

            RestaurantDB objRestaurant = new RestaurantDB();
            foreach (DataRow row in objRestaurant.GetCuisines().Tables[0].Rows)
            {
                names.Add(row["CuisineID"].ToString(), row["CuisineName"].ToString());
            }
            return names;
        }
    }
}
