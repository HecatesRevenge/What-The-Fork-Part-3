using System;
using System.Data;
using Microsoft.Data.SqlClient;
using WhatTheForkMVC.Classes;
using WhatTheForkMVC.Classes.Restaurant;

namespace WhatTheForkMVC.Database
{
    public class SearchDB
    {
        DBConnect odjDB = new DBConnect();

        public int AddSearchLog(int userID,SearchCriteria criteria)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "TP_AddSearchLog";
            objCommand.Parameters.AddWithValue("@UserID", userID);
            objCommand.Parameters.AddWithValue("@CuisineList", criteria.CuisineList);
            objCommand.Parameters.AddWithValue("@City", criteria.City);
            objCommand.Parameters.AddWithValue("@State", criteria.State);
            return odjDB.DoUpdateUsingCmdObj(objCommand);
        }
    }
}
