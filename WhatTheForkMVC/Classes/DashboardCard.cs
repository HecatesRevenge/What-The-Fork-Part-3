using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatTheForkMVC.Classes
{
    public class DashboardCard
    {
        private String category="";
        private String title = "";
        private String stat = "";
        private String description = "";
        private String linkURL = "";
        private String linkText = "";

        public DashboardCard()
        {

        }

        public DashboardCard(String category, String title, String stat, String description, String linkURL, String linkText)
        {
            this.category = category;
            this.title = title;
            this.stat = stat;
            this.description = description;
            this.linkURL = linkURL;
            this.linkText = linkText;
        }

        public String Category
        {
            get { return category; }
            set { category = value; }
        }

        public String Title
        {
            get { return title; }
            set { title = value; }
        }

        public String Stat
        {
            get { return stat; }
            set { stat = value; }
        }

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public String LinkURL
        {
            get { return linkURL; }
            set { linkURL = value; }
        }

        public String LinkText
        {
            get { return linkText; }
            set { linkText = value; }
        }
    }
}