using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using What_The_Fork.Classes;

namespace What_The_Fork.Database
{
    public class RestaurantDB
    {

        DBConnect odjDB = new DBConnect();

        public DataSet GetCuisines()
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetCuisines";
            return odjDB.GetDataSetUsingCmdObj(objCommand);
        }

        public DataSet GetLocations()
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetLocations";
            return odjDB.GetDataSetUsingCmdObj(objCommand);
        }

        public DataSet SearchRestraurants(SearchCriteria criteria)
        {

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "SearchRestaurants";

            objCommand.Parameters.AddWithValue("@CuisineList", criteria.CuisineList);
            objCommand.Parameters.AddWithValue("@City", criteria.City);
            objCommand.Parameters.AddWithValue("@State", criteria.State);

            DataSet results = odjDB.GetDataSetUsingCmdObj(objCommand);


            // Add a new column to the DataTable for the price level
            //Changes icon of price level based on price in the database
            DataTable objTable = results.Tables[0];
            objTable.Columns.Add("PriceLevel", typeof(String));
            objTable.Columns.Add("LogoUrl", typeof(String));
            foreach (DataRow row in objTable.Rows)
            {
                row["PriceLevel"] = GetDollarSigns(Convert.ToDouble(row["PriceRange"]));

                DataSet dsLogo = GetRestaurantLogo(Convert.ToInt32(row["RestaurantID"]));
                if (dsLogo.Tables[0].Rows.Count > 0)
                {
                    DataRow logoRow = dsLogo.Tables[0].Rows[0];
                    row["LogoUrl"] = "data:" + logoRow["ImageType"] + ";base64," + Convert.ToBase64String((byte[])logoRow["ImageData"]);
                }
                else if (row["LogoImagePath"] == DBNull.Value || row["LogoImagePath"].ToString() == "")
                {
                    row["LogoUrl"] = "~/Images/Placeholders/logo.svg";
                }
                else
                {
                    row["LogoUrl"] = row["LogoImagePath"];
                }
            }
            return results;

        }
        public DataSet GetRestaurant(int restaurantID)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetRestaurantByID";

            objCommand.Parameters.AddWithValue("@RestaurantID", restaurantID);
            return odjDB.GetDataSetUsingCmdObj(objCommand);
        }
        public DataSet GetRestaurantLogo(int RestaurantID)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetRestaurantLogo";
            objCommand.Parameters.AddWithValue("@RestaurantID", RestaurantID);
            return odjDB.GetDataSetUsingCmdObj(objCommand);
        }

        public DataSet GetPhotos(int restaurantID)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetRestaurantPhotos";
            objCommand.Parameters.AddWithValue("@RestaurantID", restaurantID);
            return odjDB.GetDataSetUsingCmdObj(objCommand);
        }

        public DataSet GetRatingCounts(int restaurantID)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetRatingCounts";

            objCommand.Parameters.AddWithValue("@RestaurantID", restaurantID);
            return odjDB.GetDataSetUsingCmdObj(objCommand);
        }

        public DataSet GetResturantByRep(int repUserID)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetRestaurantByRep";
            objCommand.Parameters.AddWithValue("@RepUserID", repUserID);
            return odjDB.GetDataSetUsingCmdObj(objCommand);
        }


        public DataSet GetSocialMediaPlatforms()
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetSocialMediaPlatforms";

            return odjDB.GetDataSetUsingCmdObj(objCommand);
        }


        public int AddRestaurant(Restaurant restaurant)
        {
            SqlCommand odjCommand = new SqlCommand();
            odjCommand.CommandType = CommandType.StoredProcedure;
            odjCommand.CommandText = "AddRestaurant";

            odjCommand.Parameters.AddWithValue("@RepUserID", restaurant.RepUserID);
            AddRestaurantParameters(odjCommand, restaurant);

            SqlParameter returnParam = new SqlParameter("@NewResturantID", SqlDbType.Int);
            returnParam.Direction = ParameterDirection.ReturnValue;
            odjCommand.Parameters.Add(returnParam);

            odjDB.DoUpdateUsingCmdObj(odjCommand);

            if (returnParam.Value == null || returnParam.Value == DBNull.Value)
            {
                return -0;
            }
            return Convert.ToInt32(returnParam.Value);

        }

        public int UpdateRestaurant(Restaurant restaurant)
        {
            SqlCommand odjCommand = new SqlCommand();
            odjCommand.CommandType = CommandType.StoredProcedure;
            odjCommand.CommandText = "UpdateRestaurant";
            odjCommand.Parameters.AddWithValue("@RestaurantID", restaurant.RestaurantID);
            odjCommand.Parameters.AddWithValue("@RepUserID", restaurant.RepUserID);
            AddRestaurantParameters(odjCommand, restaurant);

            return odjDB.DoUpdateUsingCmdObj(odjCommand);
        }

        private void AddRestaurantParameters(SqlCommand odjCommand, Restaurant restaurant)
        {
            odjCommand.Parameters.AddWithValue("@RestaurantName", restaurant.RestaurantName);
            odjCommand.Parameters.AddWithValue("@CuisineID", restaurant.CuisineID);
            odjCommand.Parameters.AddWithValue("@Description", restaurant.Description);
            odjCommand.Parameters.AddWithValue("@StreetAddress", restaurant.StreetAddress);
            odjCommand.Parameters.AddWithValue("@City", restaurant.City);
            odjCommand.Parameters.AddWithValue("@State", restaurant.State);
            odjCommand.Parameters.AddWithValue("@ZipCode", restaurant.ZipCode);
            odjCommand.Parameters.AddWithValue("@Phone", restaurant.Phone);
            odjCommand.Parameters.AddWithValue("@Email", restaurant.Email);
            odjCommand.Parameters.AddWithValue("@WebsiteUrl", restaurant.WebsiteUrl);
            odjCommand.Parameters.AddWithValue("@OwnerName", restaurant.OwnerName);
            odjCommand.Parameters.AddWithValue("@PriceRange", restaurant.PriceRange);
        }

        public int DeleteHours(int restaurantID)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "DeleteRestaurantHours";
            objCommand.Parameters.AddWithValue("@RestaurantID", restaurantID);
            return odjDB.DoUpdateUsingCmdObj(objCommand);
        }

        public int AddHours(int restaurantID, int dayOfWeek, TimeSpan openTime, TimeSpan closeTime, bool isClosed)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "AddRestaurantHours";
            objCommand.Parameters.AddWithValue("@RestaurantID", restaurantID);
            objCommand.Parameters.AddWithValue("@DayOfWeek", dayOfWeek);
            objCommand.Parameters.AddWithValue("@OpenTime", openTime);
            objCommand.Parameters.AddWithValue("@CloseTime", closeTime);
            objCommand.Parameters.AddWithValue("@IsClosed", isClosed);
            return odjDB.DoUpdateUsingCmdObj(objCommand);
        }

        public int DeleteSocialLinks(int restaurantID)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "DeleteSocialLinks";

            objCommand.Parameters.AddWithValue("@RestaurantID", restaurantID);
            return odjDB.DoUpdateUsingCmdObj(objCommand);

        }

        public int AddSocialLink(int restaurantID, int platformID, String url)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "AddSocialLink";
            objCommand.Parameters.AddWithValue("@RestaurantID", restaurantID);
            objCommand.Parameters.AddWithValue("@PlatformID", platformID);
            objCommand.Parameters.AddWithValue("@LinkURL", url);
            return odjDB.DoUpdateUsingCmdObj(objCommand);
        }

        public int AddPhoto(int restaurantID, String photoType, String caption, byte[] imageData, String imageType)
        {
            SqlCommand odjComamand = new SqlCommand();
            odjComamand.CommandType = CommandType.StoredProcedure;
            odjComamand.CommandText = "AddRestaurantPhoto";

            odjComamand.Parameters.AddWithValue("@RestaurantID", restaurantID);
            odjComamand.Parameters.AddWithValue("@PhotoType", photoType);
            odjComamand.Parameters.AddWithValue("@Caption", caption);
            odjComamand.Parameters.AddWithValue("@ImageData", imageData);
            odjComamand.Parameters.AddWithValue("@ImageType", imageType);
            odjComamand.Parameters.AddWithValue("@ImageLength", imageData.Length);

            return odjDB.DoUpdateUsingCmdObj(odjComamand);
        }

        public int DeletePhoto(int photoID, int restaurantID)
        {
            SqlCommand odjCommand = new SqlCommand();
            odjCommand.CommandType = CommandType.StoredProcedure;
            odjCommand.CommandText = "DeleteRestaurantPhoto";
            odjCommand.Parameters.AddWithValue("@PhotoID", photoID);
            odjCommand.Parameters.AddWithValue("@RestaurantID", restaurantID);
            return odjDB.DoUpdateUsingCmdObj(odjCommand);
        }


        public DataSet GetHours(int theRestaurantID)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetRestaurantHours";

            objCommand.Parameters.AddWithValue("@RestaurantID", theRestaurantID);

            DataSet hours = odjDB.GetDataSetUsingCmdObj(objCommand);

            String[] days = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            DataTable objTables = hours.Tables[0];
            objTables.Columns.Add("Day", typeof(String));
            objTables.Columns.Add("Hours", typeof(String));

            foreach (DataRow row in objTables.Rows)
            {
                int dayOfWeek = Convert.ToInt32(row["DayOfWeek"]);
                row["Day"] = days[dayOfWeek - 1];

                if (Convert.ToBoolean(row["IsClosed"]) || row["OpenTime"] == DBNull.Value)
                {
                    row["Hours"] = "Closed";
                }
                else
                {
                    DateTime openTime = DateTime.Today.Add((TimeSpan)row["OpenTime"]);
                    DateTime closeTime = DateTime.Today.Add((TimeSpan)row["CloseTime"]);
                    String convHours = openTime.ToString("h:mm tt") + " - " + closeTime.ToString("h:mm tt");

                    if (convHours == "12:00 AM - 11:59 PM")
                        convHours = "Open 24 hours";

                    row["Hours"] = convHours;
                }
            }
            return hours;
        }

        public DataSet GetSocialLinks(int theRestaurantID)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetRestaurantSocialLinks";
            objCommand.Parameters.AddWithValue("@RestaurantID", theRestaurantID);
            return odjDB.GetDataSetUsingCmdObj(objCommand);
        }

       public static String GetDollarSigns(double priceRating)
        {
            int dollarCount = (int)Math.Round(priceRating);
            if (dollarCount < 1)
            {
                dollarCount = 1;


            }
            return new String('$', dollarCount);
        }
    }
}