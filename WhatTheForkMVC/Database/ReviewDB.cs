using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using WhatTheForkMVC.Classes;


namespace WhatTheForkMVC.Database
{
    public class ReviewDB
    {
        DBConnect odjDB = new DBConnect();
        SqlCommand objCommand = new SqlCommand();

        public DataSet GetReviews(int restaurantID)
        {

            String strSQL = "SELECT Review FROM TP_SerReviews WHERE RestID ='" + restaurantID + "'";
            odjDB.GetDataSet(strSQL);

            if (odjDB.GetField("Review", 0) != System.DBNull.Value)
            {

                Byte[] byteArray = (Byte[])odjDB.GetField("Review", 0);

#pragma warning disable SYSLIB0011 // Type or member is obsolete
                BinaryFormatter deSerializer = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete

                MemoryStream memStream = new MemoryStream(byteArray);
                Review objrev = (Review)deSerializer.Deserialize(memStream);
                SqlCommand objCommand = new SqlCommand();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetReviewsByRestaurantID";
                objCommand.Parameters.AddWithValue("@RestaurantID", restaurantID);

                DataSet results = odjDB.GetDataSetUsingCmdObj(objCommand);


                DataTable objTable = results.Tables[0];
                objTable.Columns.Add("ReviewerName", typeof(String));
                objTable.Columns.Add("ReviewerHome", typeof(String));

                foreach (DataRow row in objTable.Rows)
                {
                    String lastName = row["LastName"].ToString();
                    String lastInitial = "";
                    if (lastName.Length > 0)
                        lastInitial = " " + lastName.Substring(0, 1) + ".";

                    row["ReviewerName"] = row["FirstName"].ToString() + lastInitial;

                    if (row["HomeCity"] == DBNull.Value || row["HomeState"] == DBNull.Value)
                        row["ReviewerHome"] = "";
                    else
                        row["ReviewerHome"] = row["HomeCity"].ToString() + ", " + row["HomeState"].ToString();
                }

                return results;
            }
            else { return null; }

        }

        public int GetReviewerID(int userID)
        {

            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "GetReviewerID";
            objCommand.Parameters.AddWithValue("@UserID", userID);

            DataSet ds = odjDB.GetDataSetUsingCmdObj(objCommand);

            if (ds.Tables[0].Rows.Count == 0)
            {
                return 0;
            }
            return Convert.ToInt32(ds.Tables[0].Rows[0]["ReviewerID"]);

        }

        public DataSet GetReviewsByReviewer(int reviewerID)
        {
            String strSQL = "SELECT Review FROM TP_SerReviews WHERE Reviewer ='" + reviewerID + "'";
            odjDB.GetDataSet(strSQL);

            if (odjDB.GetField("Review", 0) != System.DBNull.Value)
            {

                SqlCommand objCommand = new SqlCommand();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetReviewsByReviewer";
                objCommand.Parameters.AddWithValue("@ReviewerID", reviewerID);

                return odjDB.GetDataSetUsingCmdObj(objCommand);
            }
            else { return null; }
        }

        public Review GetReviewByID(int reviewID, int reviewerID)
        {
            String strSQL = "SELECT Review FROM TP_SerReviews WHERE ID ='" + reviewID + "'";
            odjDB.GetDataSet(strSQL);

            if (odjDB.GetField("Review", 0) != System.DBNull.Value)
            {

                Byte[] byteArray = (Byte[])odjDB.GetField("Review", 0);

#pragma warning disable SYSLIB0011 // Type or member is obsolete
                BinaryFormatter deSerializer = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete

                MemoryStream memStream = new MemoryStream(byteArray);
                Review objrev = (Review)deSerializer.Deserialize(memStream);
                SqlCommand objCommand = new SqlCommand();
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "GetReviewByID";

                objCommand.Parameters.AddWithValue("@ReviewID", reviewID);
                objCommand.Parameters.AddWithValue("@ReviewerID", reviewerID);

                DataSet ds = odjDB.GetDataSetUsingCmdObj(objCommand);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    return null;
                }

                DataRow row = ds.Tables[0].Rows[0];
                Review objReview = new Review();
                objReview.ReviewID = Convert.ToInt32(row["ReviewID"]);
                objReview.RestaurantID = Convert.ToInt32(row["RestaurantID"]);
                objReview.ReviewerID = reviewerID;
                objReview.ReviewTitle = row["ReviewTitle"].ToString();
                objReview.ReviewText = row["ReviewText"].ToString();
                objReview.FoodRating = Convert.ToInt32(row["FoodRating"]);
                objReview.ServiceRating = Convert.ToInt32(row["ServiceRating"]);
                objReview.PriceRating = Convert.ToInt32(row["PriceRating"]);

                if (row["AtmosphereRating"] != DBNull.Value)
                {
                    objReview.AtmosphereRating = Convert.ToInt32(row["AtmosphereRating"]);
                }

                if (row["VisitDate"] != DBNull.Value)
                {
                    objReview.VisitDate = Convert.ToDateTime(row["VisitDate"]);

                }

                if (row["ReviewPhoto"] != DBNull.Value)
                {
                    objReview.ReviewPhoto = (byte[])row["ReviewPhoto"];
                    objReview.ReviewPhotoType = row["ReviewPhotoType"].ToString();
                }

                return objReview;
            }
            else { return null; }

        }

        public void AddReview(Review theReview)
        {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
            BinaryFormatter serializer = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete

            MemoryStream memStream = new MemoryStream();

            Byte[] byteArray;

            serializer.Serialize(memStream, theReview);
            byteArray = memStream.ToArray();
            objCommand.CommandType = CommandType.StoredProcedure;

            objCommand.CommandText = "TP_StoreReviews_Se";

            objCommand.Parameters.AddWithValue("@theID", theReview.ReviewID);

            objCommand.Parameters.AddWithValue("@theReview", byteArray);
            /*
            
            SqlCommand odjCommand = new SqlCommand();
            odjCommand.CommandType = CommandType.StoredProcedure;
            odjCommand.CommandText = "AddReview";

            odjCommand.Parameters.AddWithValue("@RestaurantID", theReview.RestaurantID);
            odjCommand.Parameters.AddWithValue("@ReviewerID", theReview.ReviewerID);
            odjCommand.Parameters.AddWithValue("@VisitDate", theReview.VisitDate);
            odjCommand.Parameters.AddWithValue("@ReviewTitle", theReview.ReviewTitle);
            odjCommand.Parameters.AddWithValue("@ReviewText", theReview.ReviewText);
            odjCommand.Parameters.AddWithValue("@FoodRating", theReview.FoodRating);
            odjCommand.Parameters.AddWithValue("@ServiceRating", theReview.ServiceRating);
            odjCommand.Parameters.AddWithValue("@AtmosphereRating", theReview.AtmosphereRating);
            odjCommand.Parameters.AddWithValue("@PriceRating", theReview.PriceRating);
            AddPhotoParameters(odjCommand, theReview.ReviewPhoto, theReview.ReviewPhotoType);

            return odjDB.DoUpdateUsingCmdObj(odjCommand);
            */
        }

        public void UpdateReview(Review review)
        {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
            BinaryFormatter serializer = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete

            MemoryStream memStream = new MemoryStream();

            Byte[] byteArray;

            serializer.Serialize(memStream, review);
            byteArray = memStream.ToArray();
            objCommand.CommandType = CommandType.StoredProcedure;

            objCommand.CommandText = "TP_StoreReview_Se";

            objCommand.Parameters.AddWithValue("@theID", review.ReviewID);

            objCommand.Parameters.AddWithValue("@theReview", byteArray);
            /*
            SqlCommand odjCommand = new SqlCommand();
            odjCommand.CommandType = CommandType.StoredProcedure;
            odjCommand.CommandText = "UpdateReview";

            odjCommand.Parameters.AddWithValue("@ReviewID", review.ReviewID);
            odjCommand.Parameters.AddWithValue("ReviewerID", review.ReviewerID);
            odjCommand.Parameters.AddWithValue("@VisitDate", review.VisitDate);
            odjCommand.Parameters.AddWithValue("@ReviewTitle", review.ReviewTitle);
            odjCommand.Parameters.AddWithValue("@ReviewText", review.ReviewText);
            odjCommand.Parameters.AddWithValue("@FoodRating", review.FoodRating);
            odjCommand.Parameters.AddWithValue("@ServiceRating", review.ServiceRating);
            odjCommand.Parameters.AddWithValue("@AtmosphereRating", review.AtmosphereRating);
            odjCommand.Parameters.AddWithValue("@PriceRating", review.PriceRating);

            return odjDB.DoUpdateUsingCmdObj(odjCommand);
            */
            
        }

        public int UpdateReviewPhoto(int review, int reviewerID, byte[] photo, String photoType)
        {
            SqlCommand odjCommand = new SqlCommand();
            odjCommand.CommandType = CommandType.StoredProcedure;
            odjCommand.CommandText = "UpdateReviewPhoto";

            odjCommand.Parameters.AddWithValue("@ReviewID", review);
            odjCommand.Parameters.AddWithValue("@ReviewerID", reviewerID);
            AddPhotoParameters(odjCommand, photo, photoType);

            return odjDB.DoUpdateUsingCmdObj(odjCommand);
        }

        private void AddPhotoParameters(SqlCommand command, byte[] photo, string photoType)
        {
            SqlParameter photoParam = new SqlParameter("@ReviewPhoto", SqlDbType.VarBinary, -1);
            SqlParameter photoTypeParam = new SqlParameter("@ReviewPhotoType", SqlDbType.VarChar, 50);

            if (photo == null || photo.Length == 0)
            {
                photoParam.Value = DBNull.Value;
                photoTypeParam.Value = DBNull.Value;
            }
            else
            {
                photoParam.Value = photo;
                photoTypeParam.Value = photoType;
            }

            command.Parameters.Add(photoParam);
            command.Parameters.Add(photoTypeParam);

        }

        public int DeleteReview(int reviewID, int reviewerID)
        {
            SqlCommand odjCommand = new SqlCommand();
            odjCommand.CommandType = CommandType.StoredProcedure;
            odjCommand.CommandText = "DeleteReview";
            odjCommand.Parameters.AddWithValue("@ReviewID", reviewID);
            odjCommand.Parameters.AddWithValue("@ReviewerID", reviewerID);
            return odjDB.DoUpdateUsingCmdObj(odjCommand);
        }

        public int AddReviewVote(int reviewID, bool isHelpful)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            if (isHelpful)
            {
                objCommand.CommandText = "AddHelpfulVote";
            }
            else
            {
                objCommand.CommandText = "AddNotHelpfulVote";
            }
            objCommand.Parameters.AddWithValue("@ReviewID", reviewID);
            return odjDB.DoUpdateUsingCmdObj(objCommand);
        }


        public int AddReviewResponse(int reviewID, int resturantID, String responseText)
        {
            SqlCommand objCommand = new SqlCommand();
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.CommandText = "AddReviewResponse";

            objCommand.Parameters.AddWithValue("@ReviewID", reviewID);
            objCommand.Parameters.AddWithValue("@RestaurantID", resturantID);
            objCommand.Parameters.AddWithValue("@ResponseText", responseText);
            return odjDB.DoUpdateUsingCmdObj(objCommand);
        }

    }
}
