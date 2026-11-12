using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WhatTheForkMVC.Database;
using Microsoft.Data.SqlClient;


namespace WhatTheForkMVC.Classes.Restaurant
{
    public class RestaurantFormHelper
    {
        RestaurantDB objRestaurantDB = new RestaurantDB();

        public int GetRestaurantID(int repUserID)
        {
            DataSet dsRestaurant = objRestaurantDB.GetResturantByRep(repUserID);

            if (dsRestaurant.Tables[0].Rows.Count == 0)
                return 0;

            return Convert.ToInt32(dsRestaurant.Tables[0].Rows[0]["RestaurantID"]);
        }


        public DataSet GetCuisines()
        {
            return objRestaurantDB.GetCuisines();
        }

        public String[,] GetStates()
        {
            String[] stateCodes = { "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "DC", "FL", "GA", "HI", "ID", "IL",
                "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ",
                "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "WA",
                "WV", "WI", "WY" };
            String[] stateNames = { "Alabama", "Alaska", "Arizona", "Arkansas", "California", "Colorado", "Connecticut",
                "Delaware", "District of Columbia", "Florida", "Georgia", "Hawaii", "Idaho", "Illinois", "Indiana", "Iowa",
                "Kansas", "Kentucky", "Louisiana", "Maine", "Maryland", "Massachusetts", "Michigan", "Minnesota",
                "Mississippi", "Missouri", "Montana", "Nebraska", "Nevada", "New Hampshire", "New Jersey", "New Mexico",
                "New York", "North Carolina", "North Dakota", "Ohio", "Oklahoma", "Oregon", "Pennsylvania", "Rhode Island",
                "South Carolina", "South Dakota", "Tennessee", "Texas", "Utah", "Vermont", "Virginia", "Washington",
                "West Virginia", "Wisconsin", "Wyoming" };

            String[,] states = new String[stateCodes.Length, 2];
            for (int i = 0; i < stateCodes.Length; i++)
            {
                states[i, 0] = stateCodes[i];
                states[i, 1] = stateNames[i];
            }
            return states;
        }


        public DataTable GetHoursTable(int restaurantID)
        {
            String[] dayNames = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

            DataTable objTable = new DataTable();
            objTable.Columns.Add("DayOfWeek", typeof(int));
            objTable.Columns.Add("Day", typeof(String));
            objTable.Columns.Add("OpenTime", typeof(String));
            objTable.Columns.Add("CloseTime", typeof(String));
            objTable.Columns.Add("IsClosed", typeof(bool));

            for (int day = 1; day <= 7; day++)
            {
                objTable.Rows.Add(day, dayNames[day - 1], "", "", false);
            }

            if (restaurantID > 0)
            {
                DataSet dsHours = objRestaurantDB.GetHours(restaurantID);
                foreach (DataRow savedRow in dsHours.Tables[0].Rows)
                {
                    int day = Convert.ToInt32(savedRow["DayOfWeek"]);
                    DataRow row = objTable.Rows[day - 1];

                    if (Convert.ToBoolean(savedRow["IsClosed"]))
                    {
                        row["IsClosed"] = true;
                    }
                    else if (savedRow["OpenTime"] != DBNull.Value && savedRow["CloseTime"] != DBNull.Value)
                    {
                        row["OpenTime"] = DateTime.Today.Add((TimeSpan)savedRow["OpenTime"]).ToString("HH:mm");
                        row["CloseTime"] = DateTime.Today.Add((TimeSpan)savedRow["CloseTime"]).ToString("HH:mm");
                    }
                }
            }

            return objTable;
        }

        public DataTable GetSocialLinksTable(int restaurantID)
        {
            DataSet ds = objRestaurantDB.GetSocialMediaPlatforms();
            DataTable odjTable = ds.Tables[0];
            odjTable.Columns.Add("LinkUrl", typeof(String));

            if (restaurantID > 0)
            {
                DataSet dsLinks = objRestaurantDB.GetSocialLinks(restaurantID);
                foreach (DataRow row in odjTable.Rows)
                {
                    foreach (DataRow linkRow in dsLinks.Tables[0].Rows)
                    {
                        if (linkRow["PlatformID"].ToString() == row["PlatformID"].ToString())
                        {
                            row["LinkUrl"] = linkRow["LinkUrl"].ToString();

                        }

                    }
                }
            }

            return odjTable;
        }


        public DataSet GetPhotos(int restaurantID)
        {
            DataSet ds = objRestaurantDB.GetPhotos(restaurantID);

            DataTable odjTable = ds.Tables[0];
            odjTable.Columns.Add("ImageUrl", typeof(String));
            foreach (DataRow row in odjTable.Rows)
            {
                row["ImageUrl"] = "data:" + row["ImageType"] + ";base64," + Convert.ToBase64String((byte[])row["ImageData"]);
            }

            return ds;
        }



        public void SaveHours(int restaurantID, String[] openTimes, String[] closeTimes, int[] closedDays)
        {
            objRestaurantDB.DeleteHours(restaurantID);

            for (int day = 1; day <= 7; day++)
            {
                if (closedDays != null && closedDays.Contains(day))
                {
                    objRestaurantDB.AddHours(restaurantID, day, TimeSpan.Zero, TimeSpan.Zero, true);
                }
                else
                {
                    objRestaurantDB.AddHours(restaurantID, day, TimeSpan.Parse(openTimes[day - 1].Trim()),
                       TimeSpan.Parse(closeTimes[day - 1].Trim()), false);
                }
            }
        }

        public void SaveSocialLinks(int restaurantID, int[] platformIDs, String[] links)
        {
            objRestaurantDB.DeleteSocialLinks(restaurantID);

            for (int i = 0; i < platformIDs.Length; i++)
            {
                String link = links[i] == null ? "" : links[i].Trim();
                if (link != "")
                    objRestaurantDB.AddSocialLink(restaurantID, platformIDs[i], link);
            }
        }

        public int SavePhoto(int restaurantID, String photoType, String caption, byte[] imageData, String imageType)
        {
            if (photoType != "Gallery")
            {
                DataSet dsPhotos = objRestaurantDB.GetPhotos(restaurantID);
                foreach (DataRow row in dsPhotos.Tables[0].Rows)
                {
                    if (row["PhotoType"].ToString() == photoType)
                        objRestaurantDB.DeletePhoto(Convert.ToInt32(row["PhotoID"]), restaurantID);
                }
            }

            return objRestaurantDB.AddPhoto(restaurantID, photoType, caption, imageData, imageType);
        }


        public String GetText(Object value)
        {
            if (value == DBNull.Value)
                return "";
            return value.ToString().Trim();
        }
    }
}