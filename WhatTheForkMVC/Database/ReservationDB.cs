using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using WhatTheForkMVC.Classes;

namespace WhatTheForkMVC.Database
{
    public class ReservationDB
    {
        DBConnect odjDB = new DBConnect();
        SqlCommand objCommand = new SqlCommand();
        public void AddReversation(Reservation reservation)
        {

#pragma warning disable SYSLIB0011 // Type or member is obsolete
            BinaryFormatter serializer = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete

            MemoryStream memStream = new MemoryStream();

            Byte[] byteArray;

            serializer.Serialize(memStream, reservation);
            byteArray = memStream.ToArray();
            objCommand.CommandType = CommandType.StoredProcedure;

            objCommand.CommandText = "TP_StoreReservations_Se";

            objCommand.Parameters.AddWithValue("@theID", reservation.ReservationID);
            objCommand.Parameters.AddWithValue("@theRest", reservation.RestaurantID);
            objCommand.Parameters.AddWithValue("@theReservation", byteArray);
            /*
            SqlCommand odjCommand = new SqlCommand();
            odjCommand.CommandType = System.Data.CommandType.StoredProcedure;
            odjCommand.CommandText = "AddReservation";

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
            return odjDB.DoUpdateUsingCmdObj(odjCommand);*/

        }

        public DataSet GetReservations(int restaurantID)
        {
            String strSQL = "SELECT Review FROM TP_SerReviews WHERE restID ='" + restaurantID + "'";
            odjDB.GetDataSet(strSQL);

            if (odjDB.GetField("Reservation", 0) != System.DBNull.Value)
            {

                Byte[] byteArray = (Byte[])odjDB.GetField("Reservation", 0);

#pragma warning disable SYSLIB0011 // Type or member is obsolete
                BinaryFormatter deSerializer = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete

                MemoryStream memStream = new MemoryStream(byteArray);
                Reservation objres = (Reservation)deSerializer.Deserialize(memStream);
                SqlCommand objCommand = new SqlCommand();
                objCommand.CommandType = System.Data.CommandType.StoredProcedure;
                objCommand.CommandText = "GetReservations";
                objCommand.Parameters.AddWithValue("@RestaurantID", restaurantID);
                return odjDB.GetDataSetUsingCmdObj(objCommand);
            }
            else { return null; }
        }

        public Reservation GetReservationByID(int reservationID, int restaurantID)
        {
            String strSQL = "SELECT Review FROM TP_SerReviews WHERE ID ='" + reservationID + "'";
            odjDB.GetDataSet(strSQL);

            if (odjDB.GetField("Reservation", 0) != System.DBNull.Value)
            {
                Byte[] byteArray = (Byte[])odjDB.GetField("Reservation", 0);

#pragma warning disable SYSLIB0011 // Type or member is obsolete
                BinaryFormatter deSerializer = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete

                MemoryStream memStream = new MemoryStream(byteArray);
                Reservation objres = (Reservation)deSerializer.Deserialize(memStream);

                SqlCommand objCommand = new SqlCommand();
                objCommand.CommandType = System.Data.CommandType.StoredProcedure;
                objCommand.CommandText = "GetReservationByID";

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
            else { return null; }
        }

        public void UpdateReservation(Reservation reservation)
        {

#pragma warning disable SYSLIB0011 // Type or member is obsolete
            BinaryFormatter serializer = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete

            MemoryStream memStream = new MemoryStream();

            Byte[] byteArray;

            serializer.Serialize(memStream, reservation);
            byteArray = memStream.ToArray();
            objCommand.CommandType = CommandType.StoredProcedure;

            objCommand.CommandText = "TP_StoreReservations_Se";

            objCommand.Parameters.AddWithValue("@theID", reservation.ReservationID);

            objCommand.Parameters.AddWithValue("@theReservation", byteArray);
            /*
            SqlCommand odjCommand = new SqlCommand();
            odjCommand.CommandType = System.Data.CommandType.StoredProcedure;
            odjCommand.CommandText = "UpdateReservation";

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
            odjCommand.Parameters.AddWithValue("@SpecialRequests", reservation.SpecialRequests);*/
        
        }

        public int ConfirmReservation(int reservationID, int restaurantID)
        {
            SqlCommand odjCommand = new SqlCommand();
            odjCommand.CommandType = System.Data.CommandType.StoredProcedure;
            odjCommand.CommandText = "ConfirmReservation";

            odjCommand.Parameters.AddWithValue("@ReservationID", reservationID);
            odjCommand.Parameters.AddWithValue("@RestaurantID", restaurantID);

            return odjDB.DoUpdateUsingCmdObj(odjCommand);
        }

        public int DeleteReservation(int reservationID, int restaurantID)
        {
            SqlCommand odjCommand = new SqlCommand();
            odjCommand.CommandType = System.Data.CommandType.StoredProcedure;
            odjCommand.CommandText = "DeleteReservation";

            odjCommand.Parameters.AddWithValue("@ReservationID", reservationID);
            odjCommand.Parameters.AddWithValue("@RestaurantID", restaurantID);

            return odjDB.DoUpdateUsingCmdObj(odjCommand);
        }

    }
}
