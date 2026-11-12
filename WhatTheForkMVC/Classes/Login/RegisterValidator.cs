using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatTheForkMVC.Classes
{
    public class RegisterValidator
    {
      

       //Registration Validator class to make sure user enters correct info. 
       //Move this from Register.cs codebehind because it made it look cluttered. 
        public String CheckRegistration(String accountType, String firstName, String lastName, String email,
            String homeCity, String homeState, String username, String password, String confirmPassword)
        {
            String errors = "";

            if (accountType == "")
                errors = errors + "Please choose an account type.<br />";

            if (firstName == "")
                errors = errors + "Please enter your first name.<br />";
            if (lastName == "")
                errors = errors + "Please enter your last name.<br />";

            if (email == "")
                errors = errors + "Please enter your email address.<br />";
            else if (!IsValidEmail(email))
                errors = errors + "Please enter a valid email address, like name@example.com.<br />";

            if (accountType == "Reviewer")
            {
                if (homeCity == "")
                    errors = errors + "Please enter your home city.<br />";
                if (!IsValidState(homeState))
                    errors = errors + "Please enter your state as 2 letters, like PA.<br />";
            }

            if (username == "")
                errors = errors + "Please choose a username.<br />";
            else if (!IsLettersAndNumbers(username))
                errors = errors + "Your username can only use letters and numbers, with no spaces.<br />";

            if (password.Length < 6)
                errors = errors + "Please choose a password with at least 6 characters.<br />";
            else if (password != confirmPassword)
                errors = errors + "The two passwords don't match.<br />";

            return errors;
        }

        public bool IsValidEmail(String email)
        {
            int atPosition = email.IndexOf("@");
            return atPosition > 0 && email.LastIndexOf(".") > atPosition;
        }

        public bool IsValidState(String state)
        {
            return state.Length == 2 && Char.IsLetter(state[0]) && Char.IsLetter(state[1]);
        }

        public bool IsLettersAndNumbers(String text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (!Char.IsLetterOrDigit(text[i]))
                    return false;
            }
            return true;
        }
    }
}