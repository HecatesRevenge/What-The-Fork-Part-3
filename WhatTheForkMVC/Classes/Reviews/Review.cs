using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatTheForkMVC.Classes.Reviews
{
    public class Review
    {
        private int rewviewID = 0;
        private int restaurantID = 0;
        private int reviewerID = 0;
        private DateTime vistDate = DateTime.Now;
        private String reviewTitle = "";
        private String reviewText = "";
        private int foodRating = 0;
        private int serviceRating = 0;
        private int atmosphereRating = 0;
        private int priceRating = 0;
        private byte[] reviewPhoto = null;
        private String reviewPhotoType = "";

        public int ReviewID
        {
            get { return rewviewID; }
            set { rewviewID = value; }
        }

        public int RestaurantID
        {
            get { return restaurantID; }
            set { restaurantID = value; }
        }

        public int ReviewerID
        {
            get { return reviewerID; }
            set { reviewerID = value; }
        }

        public DateTime VisitDate
        {
            get { return vistDate; }
            set { vistDate = value; }
        }

        public String ReviewTitle
        {
            get { return reviewTitle; }
            set { reviewTitle = value; }
        }

        public String ReviewText
        {
            get { return reviewText; }
            set { reviewText = value; }
        }

        public int FoodRating
        {
            get { return foodRating; }
            set { foodRating = value; }
        }

        public int ServiceRating
        {
            get { return serviceRating; }
            set { serviceRating = value; }
        }

        public int AtmosphereRating
        {
            get { return atmosphereRating; }
            set { atmosphereRating = value; }
        }

        public int PriceRating
        {
            get { return priceRating; }
            set { priceRating = value; }
        }

        public byte[] ReviewPhoto
        {
            get { return reviewPhoto; }
            set { reviewPhoto = value; }
        }

        public String ReviewPhotoType
        {
            get { return reviewPhotoType; }
            set { reviewPhotoType = value; }
        }

        public bool HasPhoto
        {
            get { return reviewPhoto != null && reviewPhoto.Length > 0; }
        }
    }
}