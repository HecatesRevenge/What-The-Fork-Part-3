using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace WhatTheForkMVC.Classes.Restaurant
{
    public class RestaurantValidator
    {
        public String CheckRestaurantInfo(String name, int cuisineID, int priceRange, String description,
            String streetAddress, String city, String state, String zipCode, String phone, String email, String websiteURL, String ownerName)
        {
            String errors = "";

            if (name == "")
            {
                errors = errors + "Restaurant name is required.<br />";
            }
            else if (name.Length > 100)
            {
                errors = errors + "Restaurant name must be 100 characters or less.<br />";
            }

            if (cuisineID == 0)
            {
                errors = errors + "Cuisine is required.<br />";
            }

            if (priceRange == 0)
            {
                errors = errors + "Price range is required.<br />";
            }

            if (description == "")
            {
                errors = errors + "Description is required.<br />";
            }
            else if (description.Length > 2000)
            {
                errors = errors + "Description must be 2000 characters or less.<br />";
            }

            if (streetAddress == "")
            {
                errors = errors + "Street address is required.<br />";
            }
            else if (streetAddress.Length > 150)
            {
                errors = errors + "Street address must be 150 characters or less.<br />";
            }

            if (city == "")
            {
                errors = errors + "City is required.<br />";
            }
            else if (city.Length > 60)
            {
                errors = errors + "City must be 60 characters or less.<br />";
            }

            if (state == "")
            {
                errors = errors + "State is required.<br />";
            }

            if (!IsValidZipCode(zipCode))
            {
                errors = errors + "Zip code must be 5 digits ZIP code such as 19150.<br />";
            }

            if (phone == "")
            {
                errors = errors + "Phone number is required.<br />";


            }
            else if (!IsValidPhone(phone))
            {
                errors = errors + "Phone number must be 10 digits, like 267-867-5309.<br />";

            }

            if (email != "" && !IsValidEmail(email))
            {
                errors = errors + "Email address must be a real email address, like chris@temple.edu.<br />";
            }
            else if (email.Length > 100)
            {
                errors = errors + "Email address must be 100 characters or less.<br />";
            }

            errors = errors + CheckLink("website", websiteURL);

            return errors;
        }

        public String CheckHours(String day, bool closed, String openTime, String closeTime)
        {
            if (closed)
                return "";

            if (openTime == "" || closeTime == "")
                return "Please enter the opening and closing times for " + day + ", or check Closed.<br />";

            TimeSpan varOpenTime;
            TimeSpan varCloseTime;
            if (!TimeSpan.TryParse(openTime, out varOpenTime) || !TimeSpan.TryParse(closeTime, out varCloseTime))
                return "Please enter real times for " + day + ".<br />";

            if (openTime == closeTime)
                return "The opening and closing times for " + day + " can't be the same.<br />";

            return "";
        }
        public bool IsValidZipCode(String zipCode)
        {
            if (zipCode.Length != 5 && zipCode.Length != 10)
                return false;

            for (int i = 0; i < zipCode.Length; i++)
            {
                if (i == 5)
                {
                    if (zipCode[i] != '-')
                        return false;
                }
                else if (!Char.IsDigit(zipCode[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsValidPhone(String phone)
        {
            int digitCount = 0;
            for (int i = 0; i < phone.Length; i++)
            {
                char c = phone[i];
                if (Char.IsDigit(c))
                    digitCount = digitCount + 1;
                else if (c != ' ' && c != '-' && c != '.' && c != '(' && c != ')')
                    return false;
            }
            return digitCount == 10;
        }

        public String CheckCaption(String photoType, String caption)
        {
            if (photoType == "Gallery" && caption == "")
                return "Please add a caption for your gallery photo.<br />";

            if (caption.Length > 200)
                return "Please keep the caption less than 200 characters.<br />";

            return "";
        }
        public String CheckLink(String name, String url)
        {
            if (url == "")
                return "";

            if (!url.StartsWith("https://"))
                return "Your " + name + " link must start with https://.<br />";

            if (url.Length > 200)
                return "Please keep your " + name + " link to 200 characters or fewer.<br />";

            return "";
        }


        public bool IsValidEmail(String theEmail)
        {
            int atPosition = theEmail.IndexOf("@");
            return atPosition > 0 && theEmail.LastIndexOf(".") > atPosition;
        }

    }
}