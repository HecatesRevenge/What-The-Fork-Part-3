using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatTheForkMVC.Classes.Restaurant
{
    public class Restaurant
    {

        private int restaurantID = 0;
        private int repUserID = 0;
        private String restaurantName = "";
        private int cuisineID = 0;
        private String description = "";
        private String streetAddress = "";
        private String city = "";
        private String state = "";
        private String zipCode = "";
        private String phone = "";
        private String email = "";
        private String websiteUrl = "";
        private String ownerName = "";
        private int priceRange = 0;

        public int RestaurantID
        {
            get { return restaurantID; }
            set { restaurantID = value; }
        }

        public int RepUserID
        {
            get { return repUserID; }
            set { repUserID = value; }
        }

        public String RestaurantName
        {
            get { return restaurantName; }
            set { restaurantName = value; }
        }

        public int CuisineID
        {
            get { return cuisineID; }
            set { cuisineID = value; }
        }

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public String StreetAddress
        {
            get { return streetAddress; }
            set { streetAddress = value; }
        }

        public String City
        {
            get { return city; }
            set { city = value; }
        }

        public String State
        {
            get { return state; }
            set { state = value; }
        }

        public String ZipCode
        {
            get { return zipCode; }
            set { zipCode = value; }
        }

        public String Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public String Email
        {
            get { return email; }
            set { email = value; }
        }

        public String WebsiteUrl
        {
            get { return websiteUrl; }
            set { websiteUrl = value; }
        }

        public String OwnerName
        {
            get { return ownerName; }
            set { ownerName = value; }
        }

        public int PriceRange
        {
            get { return priceRange; }
            set { priceRange = value; }
        }
    }
}