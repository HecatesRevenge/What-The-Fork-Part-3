using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace What_The_Fork.Styling.Reviews
{
    public class ReviewDivs
    {

        public static String GetStarsHtml(double theRating)
        {
            String percent = (theRating * 20).ToString("0");
            return "<span class=\"stars\" title=\"" + theRating.ToString("0.0") + " out of 5\">&#9733;&#9733;&#9733;&#9733;&#9733;" +
                   "<span class=\"stars-fill\" style=\"width: " + percent + "%\">&#9733;&#9733;&#9733;&#9733;&#9733;</span></span>";
        }

        public static String GetRatingBarsHtml(double theFood, double theService, double theAtmosphere, double thePrice)
        {
            return GetRatingRowHtml("Food", theFood) +
                   GetRatingRowHtml("Service", theService) +
                   GetRatingRowHtml("Atmosphere", theAtmosphere) +
                   GetRatingRowHtml("Price", thePrice);
        }
        private static String GetRatingRowHtml(String theLabel, double theRating)
        {
            String percent = (theRating * 20).ToString("0");
            return "<div class=\"rating-row\">" +
                   "<span class=\"rating-label\">" + theLabel + "</span>" +
                   "<div class=\"rating-bar\"><div class=\"rating-fill\" style=\"width: " + percent + "%\"></div></div>" +
                   "<span class=\"rating-value\">" + theRating.ToString("0.0") + "</span>" +
                   "</div>";
        }
    }
}