using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace WhatTheForkMVC.Classes.Restaurant
{
    public class SearchCriteria
    {
        private String cuisineList = "";
        private String city = "";
        private String state = "";
        private int maxPrice = 0;
        private String description = "";
        private DateTime savedDate;

        public String CuisineList
        {
            get { return cuisineList; }
            set { cuisineList = value; }
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

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public DateTime SavedDate
        {
            get { return savedDate; }
            set { savedDate = value; }
        }

        public bool IsAllLocations
        {
            get { return city == "" || state == ""; }
        }

        public int MaxPrice
        {
            get { return maxPrice; }
            set { maxPrice = value; }
        }

    }
}