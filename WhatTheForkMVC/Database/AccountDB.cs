using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using WhatTheForkMVC.Database;


namespace WhatTheForkMVC.Classes
{
    public class AccountDB
    {
        DBConnect odjDB = new DBConnect();
        SqlCommand objCommand = new SqlCommand();

        public bool IsUernameTaken(String username)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "CheckUsername";

            objCommand.Parameters.AddWithValue("@Username", username);

            DataSet ds = odjDB.GetDataSetUsingCmdObj(objCommand);

            return ds.Tables[0].Rows.Count > 0;
        }


        public void AddUserAccount(UserAccounts user)
        {

                #pragma warning disable SYSLIB0011 // Type or member is obsolete
                BinaryFormatter serializer = new BinaryFormatter();
                #pragma warning restore SYSLIB0011 // Type or member is obsolete

            MemoryStream memStream = new MemoryStream();

            Byte[] byteArray;

            serializer.Serialize(memStream, user);
            byteArray = memStream.ToArray();
            objCommand.CommandType = CommandType.StoredProcedure;

            objCommand.CommandText = "TP_StoreRestaurant_Se";

            objCommand.Parameters.AddWithValue("@theID", user.UserID);

            objCommand.Parameters.AddWithValue("@theUser", byteArray);
           /*
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

            return Convert.ToInt32(returnParm.Value);*/
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
            String strSQL = "SELECT User FROM TP_SerUsers WHERE ID ='" + userID + "'";
            odjDB.GetDataSet(strSQL);

            if (odjDB.GetField("Review", 0) != System.DBNull.Value)
            {
                Byte[] byteArray = (Byte[])odjDB.GetField("Review", 0);

#pragma warning disable SYSLIB0011 // Type or member is obsolete
                BinaryFormatter deSerializer = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete

                MemoryStream memStream = new MemoryStream(byteArray);
                UserAccounts objuser = (UserAccounts)deSerializer.Deserialize(memStream);
             

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
            else { return null; }

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
