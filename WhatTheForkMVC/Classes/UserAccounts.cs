using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatTheForkMVC.Classes
{


    //Keeps session so user dosent need to keep loging in
    [Serializable]

    public class UserAccounts
    {
        private int userID = 0;

        private String username = "";
        private String password = "";
        private String userType = "";
        private String firstName = "";
        private String lastName = "";
        private String email = "";
        private String phoneNumber = "";

        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public String Username
        {
            get { return username; }
            set { username = value; }
        }

        public String Password
        {
            get { return password; }
            set { password = value; }
        }

        public String UserType
        {
            get { return userType; }
            set { userType = value; }
        }

        public String FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public String LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public String Email
        {
            get { return email; }
            set { email = value; }
        }

        public String PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }


        public bool IsReviewer
        {
            get { return userType == "Reviewer"; }
        }

        public bool IsRepresentative
        {
            get { return userType == "Representative"; }
        }
    }
}