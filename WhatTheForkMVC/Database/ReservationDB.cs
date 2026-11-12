using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhatTheForkMVC.Classes;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using WhatTheForkMVC.Classes.Reservation;

namespace WhatTheForkMVC.Database
{
    public class ReservationDB
    {
        DBConnect odjDB = new DBConnect();

        public int AddReversation(Reservation reservation)
        {
            SqlCommand odjCommand = new SqlCommand();
            odjCommand.CommandType = System.Data.CommandType.StoredProcedure;
            odjCommand.CommandText = "TP_AddReservation";

            odjCommand.Parameters.AddWithValue("@restaurantID", reservation.RestaurantID);

            if (reservation.UserID == 0)
            {
                odjCommand.Parameters.AddWithValue("@UserID", DBNull.Value);
            }
            else
            {
                odjCommand.Parameters.AddWithValue("@UserID", reservation.UserID);
            }

            AddReversationParameters(odjCommand, reservation);
            return odjDB.DoUpdateUsingCmdObj(odjCommand);
        }

        public DataSet GetReservations(int restaurantID)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = System.Data.CommandType.StoredProcedure;
            objCommand.CommandText = "TP_GetReservations";
            objCommand.Parameters.AddWithValue("@RestaurantID", restaurantID);
            return odjDB.GetDataSetUsingCmdObj(objCommand);
        }

        public Reservation GetReservationByID(int reservationID, int restaurantID)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = System.Data.CommandType.StoredProcedure;
            objCommand.CommandText = "TP_GetReservationByID";

            objCommand.Parameters.AddWithValue("@ReservationID", reservationID);
            objCommand.Parameters.AddWithValue("@RestaurantID", restaurantID);

            DataSet ds = odjDB.GetDataSetUsingCmdObj(objCommand);

            if (ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            DataRow row = ds.Tables[0].Rows[0];
            Reservation objReservation = new Reservation();
            objReservation.ReservationID = Convert.ToInt32(row["ReservationID"]);
            objReservation.RestaurantID = restaurantID;
            objReservation.ContactName = row["ContactName"].ToString();
            objReservation.Phone = row["Phone"].ToString();
            objReservation.Email = row["Email"].ToString();
            objReservation.ReservationDate = Convert.ToDateTime(row["ReservationDate"]);
            objReservation.PartySize = Convert.ToInt32(row["PartySize"]);
            objReservation.SpecialRequests = row["SpecialRequests"].ToString();
            objReservation.Status = row["Status"].ToString();

            return objReservation;
        }

        public int UpdateReservation(Reservation reservation)
        {
            SqlCommand odjCommand = new SqlCommand();
            odjCommand.CommandType = System.Data.CommandType.StoredProcedure;
            odjCommand.CommandText = "TP_TP_UpdateReservation";

            odjCommand.Parameters.AddWithValue("@ReservationID", reservation.ReservationID);
            odjCommand.Parameters.AddWithValue("@RestaurantID", reservation.RestaurantID);
            AddReversationParameters(odjCommand, reservation);

            return odjDB.DoUpdateUsingCmdObj(odjCommand);
        }

        //helper function so i dont need to keep retyping these pass parm.
        private void AddReversationParameters(SqlCommand odjCommand, Reservation reservation)
        {
            odjCommand.Parameters.AddWithValue("@ContactName", reservation.ContactName);
            odjCommand.Parameters.AddWithValue("@Phone", reservation.Phone);
            odjCommand.Parameters.AddWithValue("@Email", reservation.Email);
            odjCommand.Parameters.AddWithValue("@ReservationDate", reservation.ReservationDate);
            odjCommand.Parameters.AddWithValue("@PartySize", reservation.PartySize);
            odjCommand.Parameters.AddWithValue("@SpecialRequests", reservation.SpecialRequests);
        }

        public int ConfirmReservation(int reservationID, int restaurantID)
        {
            SqlCommand odjCommand = new SqlCommand();
            odjCommand.CommandType = System.Data.CommandType.StoredProcedure;
            odjCommand.CommandText = "TP_TP_ConfirmReservation";

            odjCommand.Parameters.AddWithValue("@ReservationID", reservationID);
            odjCommand.Parameters.AddWithValue("@RestaurantID", restaurantID);

            return odjDB.DoUpdateUsingCmdObj(odjCommand);
        }

        public int DeleteReservation(int reservationID, int restaurantID)
        {
            SqlCommand odjCommand = new SqlCommand();
            odjCommand.CommandType = System.Data.CommandType.StoredProcedure;
            odjCommand.CommandText = "TP_TP_DeleteReservation";

            odjCommand.Parameters.AddWithValue("@ReservationID", reservationID);
            odjCommand.Parameters.AddWithValue("@RestaurantID", restaurantID);

            return odjDB.DoUpdateUsingCmdObj(odjCommand);
        }

    }
}