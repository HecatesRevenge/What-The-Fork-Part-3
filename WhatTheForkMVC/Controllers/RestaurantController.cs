using Microsoft.AspNetCore.Mvc;
using System.Data;
using WhatTheForkMVC.Classes;
using WhatTheForkMVC.Classes.Login;
using WhatTheForkMVC.Classes.Restaurant;
using WhatTheForkMVC.Classes.Reviews;
using WhatTheForkMVC.Database;

namespace WhatTheForkMVC.Controllers
{
    public class RestaurantController : Controller
    {
        RestaurantDB objRestaurantDB = new RestaurantDB();
        RestaurantFormHelper objHelper = new RestaurantFormHelper();
        SearchSerializer objSerializer = new SearchSerializer();

        public IActionResult Search()
        {
            DataSet dsResults = null;

            try
            {
                LoadFilters();

                if (HttpContext.Session.GetString("SearchCuisines") != null)
                {
                    SearchCriteria objCriteria = new SearchCriteria();
                    objCriteria.CuisineList = HttpContext.Session.GetString("SearchCuisines");
                    objCriteria.City = HttpContext.Session.GetString("SearchCity");
                    objCriteria.State = HttpContext.Session.GetString("SearchState");

                    dsResults = ShowResults(objCriteria);
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "An error occurred while loading the search page. Please try again later.";
            }

            return View(dsResults);
        }

        [HttpPost]
        public IActionResult Search(int[] cuisines, String location, String command)
        {
            if (cuisines == null)
            {
                cuisines = new int[0];
            }
            if (location == null)
            {
                location = "";
            }

            SearchCriteria objCriteria = GetSearchCriteria(cuisines, location);
            DataSet dsResults = null;

            try
            {
                LoadFilters();
                ViewData["SelectedCuisines"] = "," + objCriteria.CuisineList + ",";
                ViewData["SelectedLocation"] = location;

                if (command == "All")
                {
                    ViewData["SelectedCuisines"] = "all";
                    return View(dsResults);
                }

                if (command == "Clear")
                {
                    ViewData["SelectedCuisines"] = "";
                    return View(dsResults);
                }

                if (objCriteria.CuisineList == "")
                {
                    ViewData["Validation"] = "Please select at least one cuisine.";
                    return View(dsResults);
                }

                HttpContext.Session.SetString("SearchCuisines", objCriteria.CuisineList);
                HttpContext.Session.SetString("SearchCity", objCriteria.City);
                HttpContext.Session.SetString("SearchState", objCriteria.State);

                dsResults = ShowResults(objCriteria);
                SaveLastSearch(objCriteria);
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "An error occurred while searching for restaurants. Please try again later.";
            }

            return View(dsResults);
        }

        public IActionResult LastSearch()
        {
            UserAccounts objUser = SessionUser.GetUser(HttpContext.Session);
            if (objUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            DataSet dsResults = null;

            try
            {
                LoadFilters();

                Byte[] searchData = objRestaurantDB.GetSavedSearch(objUser.UserID);
                if (searchData == null)
                {
                    ViewData["Validation"] = "You don't have a saved search yet. Pick your cuisines and a location below.";
                    return View("Search", dsResults);
                }

                SearchCriteria objCriteria = objSerializer.Deserialize(searchData);

                HttpContext.Session.SetString("SearchCuisines", objCriteria.CuisineList);
                HttpContext.Session.SetString("SearchCity", objCriteria.City);
                HttpContext.Session.SetString("SearchState", objCriteria.State);

                dsResults = ShowResults(objCriteria);
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "We couldn't load your last search right now. Please try again later.";
            }

            return View("Search", dsResults);
        }

        private void SaveLastSearch(SearchCriteria criteria)
        {
            UserAccounts objUser = SessionUser.GetUser(HttpContext.Session);
            if (objUser == null)
            {
                return;
            }

            try
            {
                criteria.Description = ViewData["Criteria"].ToString();
                criteria.SavedDate = DateTime.Now;
                objRestaurantDB.SaveSearch(objUser.UserID, objSerializer.Serialize(criteria));
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadFilters()
        {
            ViewData["Cuisines"] = objRestaurantDB.GetCuisines();
            ViewData["Locations"] = objRestaurantDB.GetLocations();
            ViewData["SelectedCuisines"] = "";
            ViewData["SelectedLocation"] = "";
        }

        private SearchCriteria GetSearchCriteria(int[] cuisines, String location)
        {
            SearchCriteria objCriteria = new SearchCriteria();

            String cuisineList = "";
            foreach (int cuisineID in cuisines)
            {
                if (cuisineList != "")
                {
                    cuisineList += ",";
                }
                cuisineList += cuisineID;
            }
            objCriteria.CuisineList = cuisineList;

            String[] parts = location.Split('|');
            if (parts.Length == 2)
            {
                objCriteria.City = parts[0];
                objCriteria.State = parts[1];
            }

            return objCriteria;
        }

        private DataSet ShowResults(SearchCriteria criteria)
        {
            DataSet dsResults = objRestaurantDB.SearchRestraurants(criteria);

            int count = dsResults.Tables[0].Rows.Count;
            if (count == 1)
            {
                ViewData["ResultCount"] = "1 restaurant found";
            }
            else
            {
                ViewData["ResultCount"] = count + " restaurants found";
            }

            ViewData["Criteria"] = GetCriteriaDescription(criteria);
            ViewData["SelectedCuisines"] = "," + criteria.CuisineList + ",";

            if (!criteria.IsAllLocations)
            {
                ViewData["SelectedLocation"] = criteria.City + "|" + criteria.State;
            }

            return dsResults;
        }

        private String GetCriteriaDescription(SearchCriteria criteria)
        {
            String cuisineNames = "";
            String selected = "," + criteria.CuisineList + ",";
            DataSet dsCuisines = (DataSet)ViewData["Cuisines"];

            foreach (DataRow row in dsCuisines.Tables[0].Rows)
            {
                if (selected.Contains("," + row["CuisineID"] + ","))
                {
                    if (cuisineNames != "")
                    {
                        cuisineNames += ", ";
                    }
                    cuisineNames += row["CuisineName"].ToString();
                }
            }

            String location = "All Locations";
            if (!criteria.IsAllLocations)
            {
                location = criteria.City + ", " + criteria.State;
            }

            return cuisineNames + " | " + location;
        }

        public IActionResult Profile(int id)
        {
            try
            {
                DataSet dsRestaurant = objRestaurantDB.GetRestaurant(id);
                if (dsRestaurant.Tables[0].Rows.Count == 0)
                {
                    ViewData["MessageTitle"] = "Restaurant not found";
                    ViewData["MessageText"] = "We couldn't find that restaurant. Try searching again.";
                    return View("Profile");
                }

                DataRow restaurant = dsRestaurant.Tables[0].Rows[0];
                ViewData["Restaurant"] = restaurant;
                ViewData["Title"] = restaurant["RestaurantName"].ToString();

                ShowPhotos(id, restaurant);

                ViewData["Hours"] = objRestaurantDB.GetHours(id);
                ViewData["SocialLinks"] = objRestaurantDB.GetSocialLinks(id);

                ReviewApiClient objReviewApi = new ReviewApiClient(Request);
                ViewData["Reviews"] = objReviewApi.GetReviewsForRestaurant(id);

                UserAccounts objUser = SessionUser.GetUser(HttpContext.Session);
                if (objUser == null)
                {
                    ViewData["WriteReviewText"] = "Log in to write a review";
                    ViewData["WriteReviewUrl"] = "~/Account/Login";
                    ViewData["ShowReserve"] = true;
                }
                else if (objUser.IsReviewer)
                {
                    ViewData["WriteReviewText"] = "Write a Review";
                    ViewData["WriteReviewUrl"] = "~/Review/WriteReview/" + id;
                    ViewData["ShowReserve"] = true;
                }
                else
                {
                    ViewData["ShowReserve"] = false;
                }
            }
            catch (Exception ex)
            {
                ViewData["MessageTitle"] = "Something went wrong";
                ViewData["MessageText"] = "Error loading restaurant. Please try again later.";
                ViewData["Restaurant"] = null;
            }

            return View("Profile");
        }

        private void ShowPhotos(int restaurantID, DataRow restaurant)
        {
            String logoUrl = "https://raw.githubusercontent.com/HecatesRevenge/What-The-Fork-Images/main/Placeholders/logo.svg";
            if (restaurant["LogoImagePath"] != DBNull.Value && restaurant["LogoImagePath"].ToString() != "")
            {
                logoUrl = restaurant["LogoImagePath"].ToString();
            }

            String coverUrl = "";
            if (restaurant["CoverImagePath"] != DBNull.Value)
            {
                coverUrl = restaurant["CoverImagePath"].ToString();
            }

            DataSet dsPhotos = objHelper.GetPhotos(restaurantID);
            DataTable objGallery = dsPhotos.Tables[0].Clone();

            foreach (DataRow row in dsPhotos.Tables[0].Rows)
            {
                String photoType = row["PhotoType"].ToString();

                if (photoType == "Logo")
                {
                    logoUrl = row["ImageUrl"].ToString();
                }
                else if (photoType == "Profile")
                {
                    coverUrl = row["ImageUrl"].ToString();
                }
                else if (photoType == "Gallery")
                {
                    objGallery.ImportRow(row);
                }
            }

            ViewData["LogoUrl"] = logoUrl;
            ViewData["CoverUrl"] = coverUrl;
            ViewData["Gallery"] = objGallery;
        }

    }
}
