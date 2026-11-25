using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography.X509Certificates;
using System.Web.Helpers;
using System.Web.WebPages.Html;
using System.Xml.Linq;
using WhatTheForkMVC.Classes;
using WhatTheForkMVC.Classes.Login;
using WhatTheForkMVC.Classes.Restaurant;
using WhatTheForkMVC.Classes.Security;
using WhatTheForkMVC.Database;
using WhatTheForkMVC.Models;

namespace WhatTheForkMVC.Controllers
{
    public class ResultCardController : Controller
    {
        
        RestaurantDB restFunctions = new RestaurantDB();
        SqlCommand objCommand = new SqlCommand();
        DBConnect objDB = new DBConnect();
        Restaurant newrest = new Restaurant();

        public IActionResult Submit(int restaurant, WebImage logo)
        {

            DataSet dsRate= restFunctions.GetRatingCounts(restaurant);
            DataSet dsRest = restFunctions.GetRestaurant(restaurant);
            DataSet dsLogo = restFunctions.GetRestaurantLogo(restaurant);
            DataRow drRest = dsRest.Tables[0].Rows[0];
            DataRow drRate = dsRate.Tables[0].Rows[0];
            DataRow drlogo = dsLogo.Tables[0].Rows[0];
            Object webImage = drlogo["ImageData"];
            string restName = drRest["RestaurantName"].ToString();
            string restDesc = drRest["Description"].ToString();
            int restRate = Convert.ToInt32(drRate["Stars"]);



          //  ViewData["Title"] = restName + " | What the Fork";

           // logo.ImageUrl
           ViewBag.ImageData = GetImageUrl(drRest["LogoImagePath"], "https://raw.githubusercontent.com/HecatesRevenge/What-The-Fork-Images/main/Placeholders/logo.svg");

            //logo.AlternateText = restName + " logo";
            ViewData["Restaurant"] = restName;
            ViewData["Description"] = restDesc;
            ViewData["Rating"] = restRate;

            return View();
        }
        private String GetImageUrl(Object thePath, String theDefaultUrl)
        {
            if (thePath == DBNull.Value || thePath.ToString() == "")
                return theDefaultUrl;
            return thePath.ToString();
        }

        public IActionResult ViewMore(int restaurant) {

            DataSet dsRest = restFunctions.GetRestaurant(restaurant);
            DataRow drRest = dsRest.Tables[0].Rows[0];
            string rcuisine = drRest["CuisineName"].ToString();
            string radd = drRest["StreetAddress"].ToString();
            string rcity= drRest["City"].ToString();
            string rstate = drRest["State"].ToString();
            string rzip = drRest["ZipCode"].ToString();
            string rphone = drRest["Phone"].ToString();
            string remail = drRest["Email"].ToString();
            string rweb = drRest["WebsiteUrl"].ToString();
            string rowner = drRest["OwnerName"].ToString();
            string rprice = drRest["PriceRange"].ToString();
            int rating = Convert.ToInt32(drRest["OverallRating"]);
            int rfood = Convert.ToInt32(drRest["FoodRating"]);
            int rservice = Convert.ToInt32(drRest["ServiceRating"]);
            int ratmo = Convert.ToInt32(drRest["AtmosphereRating"]);
            int rateprice = Convert.ToInt32(drRest["PriceRating"]);
            ViewData["Cuisine"] = rcuisine;
            ViewData["Address"] = radd;
            ViewData["City"] = rcity;
            ViewData["State"] = rstate;
            ViewData["Zip"] = rzip;
            ViewData["Phone"] = rphone;
            ViewData["Email"] = remail;
            ViewData["Web"] = rweb;
            ViewData["Owner"] = rowner;
            ViewData["Price"] = rprice;
            ViewData["PriceRating"] = rateprice;
            ViewData["Overall"] = rating;
            ViewData["Service"] = rservice;
            ViewData["Atmo"] = ratmo;
            ViewData["Food"] = rfood;

            List<int> items = new List<int>();

            DataSet dsphotos = restFunctions.GetPhotos(restaurant);
            foreach (DataRow row in dsphotos.Tables)
            {
                items.Add(Convert.ToInt32(row["PhotoID"].ToString()));
            }
            ViewBag.Items = items;



            return View();
        }

        public IActionResult ChooseImage(int chosenimage) {


            return View();
        }

    }
}
