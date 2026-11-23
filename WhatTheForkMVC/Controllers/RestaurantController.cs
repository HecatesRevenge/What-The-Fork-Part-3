using System.Data;
using Microsoft.AspNetCore.Mvc;
using WhatTheForkMVC.Classes;
using WhatTheForkMVC.Classes.Restaurant;
using WhatTheForkMVC.Database;

namespace WhatTheForkMVC.Controllers
{
    public class RestaurantController : Controller
    {
        RestaurantDB objRestaurantDB = new RestaurantDB();

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
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "An error occurred while searching for restaurants. Please try again later.";
            }

            return View(dsResults);
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
    }
}
