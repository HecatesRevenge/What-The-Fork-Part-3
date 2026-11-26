using System;

namespace WhatTheForkMVC.Classes
{
    public class ToolDescriptions
    {
        public const String GetMyReviews = "Gets the reviews the current user has written, with ratings from 1 to 5.";
        public const String GetMySearchHistory = "Gets the current user's 10 most recent restaurant searches.";
        public const String GetSearchOptions = "Lists every cuisine ID and name, and every city and state that has restaurants. Call this before SearchRestaurants.";
        public const String SearchRestaurants = "Searches restaurants. Returns up to 10 results, best rated first.";

        public const String CuisineIDs = "Comma-separated cuisine IDs from GetSearchOptions, like 1,4. Empty means all cuisines.";
        public const String City = "City exactly as listed in GetSearchOptions. Empty means all locations.";
        public const String State = "Two-letter state, like PA. Empty means all locations.";
        public const String MaxPrice = "Highest price level from 1 ($) to 4 ($$$$). Use 0 for any price.";
    }
}