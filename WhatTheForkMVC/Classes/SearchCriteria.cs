using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace WhatTheForkMVC.Classes
{
    public class SearchCriteria
    {
        private String cuisineList = "";
        private String city = "";
        private String state = "";

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

        public bool IsAllLocations
        {
            get { return city == "" || state == ""; }
        }

    }
}