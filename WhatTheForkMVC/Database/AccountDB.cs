using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.Data.SqlClient;


namespace WhatTheForkMVC.Classes
{
    public class AccountDB
    {
        DBConnect odjDB = new DBConnect();

        public bool IsUernameTaken(String username)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "CheckUsername";

            objCommand.Parameters.AddWithValue("@Username", username);

            DataSet ds = odjDB.GetDataSetUsingCmdObj(objCommand);

            return ds.Tables[0].Rows.Count > 0;
        }


        public int AddUserAccount(UserAccounts user)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "AddUserAccount";

            objCommand.Parameters.AddWithValue("@Username", user.Username);
            objCommand.Parameters.AddWithValue("@Password", user.Password);
            objCommand.Parameters.AddWithValue("@UserType", user.UserType);
            objCommand.Parameters.AddWithValue("@FirstName", user.FirstName);
            objCommand.Parameters.AddWithValue("@LastName", user.LastName);
            objCommand.Parameters.AddWithValue("@Email", user.Email);
            objCommand.Parameters.AddWithValue("@Phone", user.PhoneNumber);

            SqlParameter returnParm = new SqlParameter("NewUserID", SqlDbType.Int);
            returnParm.Direction = ParameterDirection.ReturnValue;
            objCommand.Parameters.Add(returnParm);

            odjDB.DoUpdateUsingCmdObj(objCommand);

            if (returnParm.Value == null || returnParm.Value == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt32(returnParm.Value);
        }

        public int AddReviewer(int userID, String firstName, String lastName, String homeCity, String homeState)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "AddReviewer";

            objCommand.Parameters.AddWithValue("@UserID", userID);
            objCommand.Parameters.AddWithValue("@FirstName", firstName);
            objCommand.Parameters.AddWithValue("@LastName", lastName);
            objCommand.Parameters.AddWithValue("@HomeCity", homeCity);
            objCommand.Parameters.AddWithValue("@HomeState", homeState);

            return odjDB.DoUpdateUsingCmdObj(objCommand);

        }

        public UserAccounts GetUserLogin(String username)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetUserLogin";

            objCommand.Parameters.AddWithValue("@Username", username);

            DataSet ds = odjDB.GetDataSetUsingCmdObj(objCommand);

            if (ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            UserAccounts objUser = MakeUserAccount(ds.Tables[0].Rows[0]);
            objUser.Password = ds.Tables[0].Rows[0]["Password"].ToString();

            return objUser;
        }

        public UserAccounts GetUserByID(int userID)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetUserByID";
            objCommand.Parameters.AddWithValue("@UserID", userID);
            DataSet ds = odjDB.GetDataSetUsingCmdObj(objCommand);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            UserAccounts objUser = MakeUserAccount(ds.Tables[0].Rows[0]);
            return objUser;
        }


        //Dont want public method having direct access to making useraccounts
        private UserAccounts MakeUserAccount(DataRow row)
        {
            UserAccounts objUser = new UserAccounts();
            objUser.UserID = Convert.ToInt32(row["UserID"]);
            objUser.Username = row["Username"].ToString();
            objUser.UserType = row["UserType"].ToString();
            objUser.FirstName = row["FirstName"].ToString();
            objUser.LastName = row["LastName"].ToString();
            objUser.Email = row["Email"].ToString();
            objUser.PhoneNumber = row["Phone"].ToString();

            return objUser;

        }
    }
}