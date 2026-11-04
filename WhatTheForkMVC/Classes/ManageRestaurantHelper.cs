using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WhatTheForkMVC.Database;


//Note: If Reused will have to update passed objects since the are for Webforms
namespace WhatTheForkMVC.Classes
{
    public class ManageRestaurantHelper
    {
        public void LoadStates(DropDownList list)
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

            list.Items.Add(new ListItem("Choose a state", ""));
            for (int i = 0; i < stateCodes.Length; i++)
            {
                list.Items.Add(new ListItem(stateNames[i], stateCodes[i]));
            }
        }


        public DataTable GetHoursTable(DataSet savedHours)
        {
            String[] dayNames = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

            DataTable odjTable = new DataTable();
            odjTable.Columns.Add("DayOfWeek", typeof(int));
            odjTable.Columns.Add("Day", typeof(String));
            odjTable.Columns.Add("OpenTime", typeof(String));
            odjTable.Columns.Add("CloseTime", typeof(String));
            odjTable.Columns.Add("IsClosed", typeof(bool));

            for (int day = 1; day <= 7; day++)
            {
                odjTable.Rows.Add(day, dayNames[day - 1], "", "", false);
            }

            if (savedHours != null)
            {
                foreach (DataRow savedRow in savedHours.Tables[0].Rows)
                {
                    int day = Convert.ToInt32(savedRow["DayOfWeek"]);
                    DataRow row = odjTable.Rows[day - 1];

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

            return odjTable;
        }


        public DataTable GetSocialLinksTable(DataSet platforms, DataSet savedLinks)
        {
            DataTable odjTable = platforms.Tables[0];
            odjTable.Columns.Add("LinkUrl", typeof(String));

            if (savedLinks != null)
            {
                foreach (DataRow row in odjTable.Rows)
                {
                    foreach (DataRow linkRow in savedLinks.Tables[0].Rows)
                    {
                        if (linkRow["PlatformID"].ToString() == row["PlatformID"].ToString())
                            row["LinkUrl"] = linkRow["LinkUrl"].ToString();
                    }
                }
            }

            return odjTable;
        }

        public DataTable GetPhotosTable(DataSet photos)
        {
            DataTable odjTable = photos.Tables[0];
            odjTable.Columns.Add("ImageUrl", typeof(String));
            foreach (DataRow row in odjTable.Rows)
            {
                row["ImageUrl"] = "data:" + row["ImageType"] + ";base64," + Convert.ToBase64String((byte[])row["ImageData"]);
            }

            return odjTable;
        }

        public void SelectValue(ListControl list, String value)
        {
            ListItem item = list.Items.FindByValue(value);
            if (item != null)
                list.SelectedValue = value;
        }

        public String GetText(Object value)
        {
            if (value == DBNull.Value)
                return "";
            return value.ToString().Trim();
        }
    }
}