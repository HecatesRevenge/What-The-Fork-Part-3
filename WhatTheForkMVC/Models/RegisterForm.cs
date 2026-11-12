using System;
using System.Runtime.InteropServices;
namespace WhatTheForkMVC.Models
{
    public class RegisterForm
    {
        private String accountType = "";
        private String firstName = "";
        private String lastName = "";
        private String email = "";
        private String phone = "";
        private String homeCity = "";
        private String homeState = "";
        private String username = "";
        private String password = "";
        private String confirmPassword = "";
        private int question1 = 0;
        private int question2 = 0;
        private int question3 = 0;
        private String answer1 = "";
        private String answer2 = "";
        private String answer3 = "";
        private bool saveLoginID = false;

        public String AccountType
        {
            get { return accountType; }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                accountType = value;

            }
        }

        public String FirstName
        {
            get { return firstName; }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                firstName = value;

            }
        }

        public String LastName
        {
            get { return lastName; }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                lastName = value;
            }
        }

        public String Email
        {
            get { return email; }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                email = value;
            }
        }

        public String Phone
        {
            get { return phone; }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                phone = value;
            }
        }

        public String HomeCity
        {
            get { return homeCity; }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                homeCity = value;
            }
        }

        public String Username
        {
            get { return username; }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                username = value;
            }
        }

        public String Password
        {
            get { return password; }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                password = value;
            }
        }

        public String ConfirmPassword
        {
            get { return confirmPassword; }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                confirmPassword = value;
            }
        }

        public int Question1
        {
            get { return question1; }
            set { question1 = value; }
        }
        public int Question2
        {
            get { return question2; }
            set { question2 = value; }
        }

        public int Question3
        {
            get { return question3; }
            set { question3 = value; }
        }

        public String Answer1
        {
            get { return answer1; }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                answer1 = value;
            }
        }

        public String Answer2
        {
            get { return answer2; }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                answer2 = value;
            }
        }

        public String Answer3
        {
            get { return answer3; }
            set
            {
                if (value == null)
                {
                    value = "";
                }
                answer3 = value;
            }
        }

        public bool SaveLoginID
        {
            get { return saveLoginID; }
            set { saveLoginID = value; }
        }
    }
}
