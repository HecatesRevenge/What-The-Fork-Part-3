using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using What_The_Fork.Database;
using What_The_Fork.Classes;
using System.Web.UI.WebControls;

namespace What_The_Fork.Classes
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


        public void FillCuisines(DropDownList list)
        {
            list.DataSource = objRestaurantDB.GetCuisines();
            list.DataTextField = "CuisineName";
            list.DataValueField = "CuisineID";
            list.DataBind();
            list.Items.Insert(0, new ListItem("Choose a cuisine", "0"));
        }

        public void FillStates(DropDownList list)
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



        public void SaveHours(int restaurantID, GridView hoursGrid)
        {
            objRestaurantDB.DeleteHours(restaurantID);

            foreach (GridViewRow row in hoursGrid.Rows)
            {
                int dayOfWeek = Convert.ToInt32(hoursGrid.DataKeys[row.RowIndex].Value);
                CheckBox chkClosed = (CheckBox)row.FindControl("chkClosed");
                TextBox txtOpen = (TextBox)row.FindControl("txtOpen");
                TextBox txtClose = (TextBox)row.FindControl("txtClose");

                if (chkClosed.Checked)
                {
                    objRestaurantDB.AddHours(restaurantID, dayOfWeek, TimeSpan.Zero, TimeSpan.Zero, true);

                }
                else
                {
                    objRestaurantDB.AddHours(restaurantID, dayOfWeek, TimeSpan.Parse(txtOpen.Text.Trim()),
                       TimeSpan.Parse(txtClose.Text.Trim()), false);
                }
            }

        }

        public void SaveSocialLinks(int restaurantID, GridView linksGrid)
        {
            objRestaurantDB.DeleteSocialLinks(restaurantID);

            foreach (GridViewRow row in linksGrid.Rows)
            {
                TextBox txtLink = (TextBox)row.FindControl("txtLink");
                String link = txtLink.Text.Trim();
                int platformID = Convert.ToInt32(linksGrid.DataKeys[row.RowIndex].Values["PlatformID"]);

                if (link != "")
                    objRestaurantDB.AddSocialLink(restaurantID, platformID, link);
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