using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WhatTheForkMVC.Classes
{
    public class ReviewValidator
    {
        public const int MaxPhotoBytes = 1048576;

        public String CheckReview(int foodRating, int serviceRating, int atmosphereRating, int priceRating,
            String visitDate, String visitTime, String title, String text)
        {
            String errors = "";

            if (foodRating == 0)
                errors = errors + "Please rate the food.<br />";
            if (serviceRating == 0)
                errors = errors + "Please rate the service.<br />";
            if (atmosphereRating == 0)
                errors = errors + "Please rate the atmosphere.<br />";
            if (priceRating == 0)
                errors = errors + "Please choose a price level.<br />";

            if (visitDate == "" || visitTime == "")
            {
                errors = errors + "Please enter the date and time of your visit.<br />";
            }
            else
            {
                DateTime dtVisitDate;
                if (!DateTime.TryParse(visitDate + " " + visitTime, out dtVisitDate))
                    errors = errors + "Please enter a real date and time for your visit.<br />";
                else if (dtVisitDate > DateTime.Now)
                    errors = errors + "Your visit can't be in the future.<br />";
            }

            if (title == "")
            {
                errors = errors + "Please give your review a title.<br />";

            }
            else if(title.Length > 100) { 
                errors = errors + "Please keep the title to 100 characters or fewer.<br />";
            }

            if (text == "")
            {
                errors = errors + "Please write your review.<br />";

            }
            else if (text.Length > 4000)
            {
                errors = errors + "Please keep your review to 4000 characters or fewer.<br />";
            }

            return errors;
        }

       
        public String CheckPhoto(String fileName, int length)
        {
            String extension = Path.GetExtension(fileName).ToLower();

            if (extension != ".jpg" && extension != ".jpeg" && extension != ".gif" && extension != ".png")
            {
                return "Your photo must be a .jpg, .gif or .png file.<br />";

            }

            if (length == 0)
            {
                return "Your photo file is empty. Please choose another one.<br />";

            }

            if (length > MaxPhotoBytes)
            {
                return "Your photo is larger than 1 MB. Please choose a smaller one.<br />";

            }

            return "";
        }
    }
}