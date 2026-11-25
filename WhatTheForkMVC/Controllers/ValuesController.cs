using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using WhatTheForkMVC.Classes;
using WhatTheForkMVC.Database;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WhatTheForkMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        DBConnect objDB = new DBConnect(); 

        SqlCommand objCommand = new SqlCommand();
      

        // GET api/<ValuesController>/5
        [HttpGet]
        public List<Restaurant> Get(int id)
        {
            
            DataSet ds = objDB.GetDataSet("SELECT * FROM TP_Restaurants");
            List<Restaurant> RestaurantList = new List<Restaurant>();
            Restaurant newrest;

            foreach (DataRow record in ds.Tables[0].Rows)
           
            {

                newrest = new Restaurant();

                newrest.RestaurantID = int.Parse(record["RestaurantID"].ToString());
                newrest.RepUserID = int.Parse(record["RepUserID"].ToString());
                newrest.RestaurantName = record["RestaurantName"].ToString();
                newrest.CuisineID = int.Parse(record["CuisineID"].ToString());
                newrest.PriceRange = int.Parse(record["PriceRange"].ToString());
                newrest.Phone = record["Phone"].ToString();
                newrest.ZipCode = record["ZipCode"].ToString();
                newrest.Description = record["Description"].ToString();
                newrest.StreetAddress = record["StreetAddress"].ToString();
                newrest.City = record["City"].ToString();
                newrest.State = record["State"].ToString();
                newrest.Email = record["Email"].ToString();
                newrest.OwnerName = record["OwnerName"].ToString();
                newrest.WebsiteUrl = record["WebsiteUrl"].ToString();
              

                RestaurantList.Add(newrest);

            }


            return RestaurantList;
      
        }

        [HttpGet("{id}")]
        public Restaurant GetById(int Id)
        {
            DataSet ds = objDB.GetDataSet("SELECT * FROM TP_Restaurants WHERE RestaurantID = '"+Id+"'");
            DataRow record;

            Restaurant newrest = new Restaurant();
            if (ds.Tables[0].Rows.Count!=0) {
                record=ds.Tables[0].Rows[0];
                newrest.RestaurantID = int.Parse(record["RestaurantID"].ToString());
                newrest.RepUserID = int.Parse(record["RepUserID"].ToString());
                newrest.RestaurantName = record["RestaurantName"].ToString();
                newrest.CuisineID = int.Parse(record["CuisineID"].ToString());
                newrest.PriceRange = int.Parse(record["PriceRange"].ToString());
                newrest.Phone = record["Phone"].ToString();
                newrest.ZipCode = record["ZipCode"].ToString();
                newrest.Description = record["Description"].ToString();
                newrest.StreetAddress = record["StreetAddress"].ToString();
                newrest.City = record["City"].ToString();
                newrest.State = record["State"].ToString();
                newrest.Email = record["Email"].ToString();
                newrest.OwnerName = record["OwnerName"].ToString();
                newrest.WebsiteUrl = record["WebsiteUrl"].ToString();
            }
            return newrest;
        }
        //cuisines and location
        [HttpGet("cuisineid")]
        public Restaurant GetByCuisine(int id) {
            Restaurant newrest = new Restaurant();
            DataSet ds = objDB.GetDataSet("SELECT * FROM TP_Restaurants WHERE CuisineID = '" + id + "'");
            DataRow record;
            if (ds.Tables[0].Rows.Count != 0)
            {
                record = ds.Tables[0].Rows[0];
                newrest.RestaurantID = int.Parse(record["RestaurantID"].ToString());
                newrest.RepUserID = int.Parse(record["RepUserID"].ToString());
                newrest.RestaurantName = record["RestaurantName"].ToString();
                newrest.CuisineID = int.Parse(record["CuisineID"].ToString());
                newrest.PriceRange = int.Parse(record["PriceRange"].ToString());
                newrest.Phone = record["Phone"].ToString();
                newrest.ZipCode = record["ZipCode"].ToString();
                newrest.Description = record["Description"].ToString();
                newrest.StreetAddress = record["StreetAddress"].ToString();
                newrest.City = record["City"].ToString();
                newrest.State = record["State"].ToString();
                newrest.Email = record["Email"].ToString();
                newrest.OwnerName = record["OwnerName"].ToString();
                newrest.WebsiteUrl = record["WebsiteUrl"].ToString();
            }
            return newrest;
        }
        [HttpGet("location")]
        public Restaurant GetByLocation(string loc)
        {
            Restaurant newrest = new Restaurant();
            DataSet ds = objDB.GetDataSet("SELECT * FROM TP_Restaurants WHERE City = '" + loc + "'");
            DataRow record;
            if (ds.Tables[0].Rows.Count != 0)
            {
                record = ds.Tables[0].Rows[0];
                newrest.RestaurantID = int.Parse(record["RestaurantID"].ToString());
                newrest.RepUserID = int.Parse(record["RepUserID"].ToString());
                newrest.RestaurantName = record["RestaurantName"].ToString();
                newrest.CuisineID = int.Parse(record["CuisineID"].ToString());
                newrest.PriceRange = int.Parse(record["PriceRange"].ToString());
                newrest.Phone = record["Phone"].ToString();
                newrest.ZipCode = record["ZipCode"].ToString();
                newrest.Description = record["Description"].ToString();
                newrest.StreetAddress = record["StreetAddress"].ToString();
                newrest.City = record["City"].ToString();
                newrest.State = record["State"].ToString();
                newrest.Email = record["Email"].ToString();
                newrest.OwnerName = record["OwnerName"].ToString();
                newrest.WebsiteUrl = record["WebsiteUrl"].ToString();
            }
            return newrest;
        }
        //I'm not even sure if these last two gets are needed but I'm keeping them just in case

        // POST api/<ValuesController>
        [HttpPost]
        public Boolean PostJSON([FromBody] Restaurant newrest)
        {

            DBConnect objDB = new DBConnect();

            string strSQL = "INSERT INTO TP_Restaurants (RestaurantID, RepUserID, RestaurantName, CuisineID, PriceRange, Phone, ZipCode, Description, StreetAddress, City, State, Email, OwnerName, WebsiteUrl) " +

                            "VALUES ('" + newrest.RestaurantID + "', '" + newrest.RepUserID + "', '" + newrest.RestaurantName + "', '" +newrest.CuisineID + "', '" +newrest.PriceRange + "', '" +newrest.Phone + "', '" +newrest.ZipCode + "', '" +newrest.Description + "', '" +newrest.StreetAddress + "', '" +newrest.City + "', '" +newrest.State + "', '" +newrest.Email+ "', '" + newrest.OwnerName + "', '" + newrest.WebsiteUrl+ "')";
            // Execute the INSERT statement in the database

            // The DoUpdate() method returns the number of records effected by the INSERT statement.

            // Otherwise, it returns -1 when there was an error exception.

            int result = objDB.DoUpdate(strSQL);

            if (result > 0) { return true; }
            else { return false; }
               
           
        }
        [HttpPost]
        public Boolean Post(Restaurant newrest)
        {

            DBConnect objDB = new DBConnect();

            string strSQL = "INSERT INTO TP_Restaurants (RestaurantID, RepUserID, RestaurantName, CuisineID, PriceRange, Phone, ZipCode, Description, StreetAddress, City, State, Email, OwnerName, WebsiteUrl) " +

                            "VALUES ('" + newrest.RestaurantID + "', '" + newrest.RepUserID + "', '" + newrest.RestaurantName + "', '" + newrest.CuisineID + "', '" + newrest.PriceRange + "', '" + newrest.Phone + "', '" + newrest.ZipCode + "', '" + newrest.Description + "', '" + newrest.StreetAddress + "', '" + newrest.City + "', '" + newrest.State + "', '" + newrest.Email + "', '" + newrest.OwnerName + "', '" + newrest.WebsiteUrl + "')";
            // Execute the INSERT statement in the database

            // The DoUpdate() method returns the number of records effected by the INSERT statement.

            // Otherwise, it returns -1 when there was an error exception.

            int result = objDB.DoUpdate(strSQL);

            if (result > 0) { return true; }
            else { return false; }


        }

        // PUT api/<ValuesController>/5
        [HttpPut("putwithid")]
        public Boolean Put(Restaurant newrest)
        {
            DBConnect objDB = new DBConnect();

            string strSQL =  "UPDATE TP_Restaurants SET RestaurantID = '" + newrest.RestaurantID + "', RepUserID = '" + newrest.RepUserID + "', RestaurantName ='" + newrest.RestaurantName + "', CuisineID='" + newrest.CuisineID + "', PriceRange='" + newrest.PriceRange + "', Phone='" + newrest.Phone + "', ZipCode='" + newrest.ZipCode + "', Description='" + newrest.Description + "', StreetAddress='" + newrest.StreetAddress + "', City='" + newrest.City + "', State='" + newrest.State + "', Email='" + newrest.Email + "', OwnerName='" + newrest.OwnerName + "', WebsiteUrl='" + newrest.WebsiteUrl + "')";
            // Execute the INSERT statement in the database

            // The DoUpdate() method returns the number of records effected by the INSERT statement.

            // Otherwise, it returns -1 when there was an error exception.

            int result = objDB.DoUpdate(strSQL);

            if (result > 0) { return true; }
            else { return false; }
        }

        // PUT api/<ValuesController>/5
        [HttpPut("putwithjson")]
        public Boolean PutJSON([FromBody] Restaurant newrest)
        {
            DBConnect objDB = new DBConnect();

            string strSQL = "UPDATE TP_Restaurants SET RestaurantID = '" + newrest.RestaurantID + "', RepUserID = '" + newrest.RepUserID + "', RestaurantName ='" + newrest.RestaurantName + "', CuisineID='" + newrest.CuisineID + "', PriceRange='" + newrest.PriceRange + "', Phone='" + newrest.Phone + "', ZipCode='" + newrest.ZipCode + "', Description='" + newrest.Description + "', StreetAddress='" + newrest.StreetAddress + "', City='" + newrest.City + "', State='" + newrest.State + "', Email='" + newrest.Email + "', OwnerName='" + newrest.OwnerName + "', WebsiteUrl='" + newrest.WebsiteUrl + "')";
            // Execute the INSERT statement in the database

            // The DoUpdate() method returns the number of records effected by the INSERT statement.

            // Otherwise, it returns -1 when there was an error exception.

            int result = objDB.DoUpdate(strSQL);

            if (result > 0) { return true; }
            else { return false; }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(Restaurant newrest,int id)
        {
            DBConnect objDB = new DBConnect();

            string strSQL = "DELETE FROM TP_Restaurants WHERE RestaurantID='"+newrest.RestaurantID+"'";
            
           objDB.DoUpdate(strSQL);

        }

   
    }
}
