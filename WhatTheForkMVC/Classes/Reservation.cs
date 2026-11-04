using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace WhatTheForkMVC.Classes
{
    public class Reservation
    {
        private int reservationID = 0;
        private int restaurantID = 0;
        private int userID = 0;
        private String contactName = "";
        private String phone= "";
        private String email = "";
        private DateTime reservationDate = DateTime.MinValue;
        private int partySize = 0;
        private String specialRequests = "";
        private String status = "";

        public int ReservationID
        {
            get { return reservationID; }
            set { reservationID = value; }
        }

        public int RestaurantID
        {
            get { return restaurantID; }
            set { restaurantID = value; }
        }

        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public String ContactName
        {
            get { return contactName; }
            set { contactName = value; }
        }

        public String Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public String Email
        {
            get { return email; }
            set { email = value; }
        }

        public DateTime ReservationDate
        {
            get { return reservationDate; }
            set { reservationDate = value; }
        }

        public int PartySize
        {
            get { return partySize; }
            set { partySize = value; }
        }

        public String SpecialRequests
        {
            get { return specialRequests; }
            set { specialRequests = value; }
        }

        public String Status
        {
            get { return status; }
            set { status = value; }
        }
    }
}