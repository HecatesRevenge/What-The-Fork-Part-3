using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhatTheForkMVC.Classes.Emails;

namespace WhatTheForkMVC.Classes.Reservation
{
    public class ReservationEmail
    {
        const String FromAddress = "tus45674@temple.edu";

        public bool SendNewReservation(String repEmail, String restaurantName, Reservation reservation)
        {
            String subject = "New reservation request for " + restaurantName;
            String body = "<p>" + HttpUtility.HtmlEncode(reservation.ContactName) +
                " would like to book a table at " + HttpUtility.HtmlEncode(restaurantName) + ".</p>" +
                GetDetails(reservation) +
                "<p>Log in to Manage Reservations and confirm it.</p>";

            return Send(repEmail, subject, body);
        }

        public bool SendConfirmed(String restaurantName, Reservation reservation)
        {
            String subject = "Your reservation at " + restaurantName + " is confirmed";
            String body = "<p>Hi " + HttpUtility.HtmlEncode(reservation.ContactName) + ",</p>" +
                "<p>Good news! " + HttpUtility.HtmlEncode(restaurantName) + " has confirmed your reservation.</p>" +
                GetDetails(reservation) +
                "<p>See you soon!</p>";

            return Send(reservation.Email, subject, body);
        }

        public bool SendChanged(String restaurantName, Reservation reservation)
        {
            String subject = "Your reservation at " + restaurantName + " was changed";
            String body = "<p>Hi " + HttpUtility.HtmlEncode(reservation.ContactName) + ",</p>" +
                "<p>" + HttpUtility.HtmlEncode(restaurantName) + " made a change to your reservation. Here are the new details:</p>" +
                GetDetails(reservation) +
                "<p>If this doesn't work for you, please call the restaurant.</p>";

            return Send(reservation.Email, subject, body);
        }

        public bool SendCancelled(String restaurantName, Reservation reservation)
        {
            String subject = "Your reservation at " + restaurantName + " was cancelled";
            String body = "<p>Hi " + HttpUtility.HtmlEncode(reservation.ContactName) + ",</p>" +
                "<p>" + HttpUtility.HtmlEncode(restaurantName) + " cancelled this reservation:</p>" +
                GetDetails(reservation) +
                "<p>Please call the restaurant if you have any questions.</p>";

            return Send(reservation.Email, subject, body);
        }

        private String GetDetails(Reservation reservation)
        {
            String requests = reservation.SpecialRequests;
            if (requests == "")
                requests = "None";

            return "<ul>" +
                "<li>Date: " + reservation.ReservationDate.ToString("dddd, MMMM d, yyyy") + "</li>" +
                "<li>Time: " + reservation.ReservationDate.ToString("h:mm tt") + "</li>" +
                "<li>Party size: " + reservation.PartySize + "</li>" +
                "<li>Name: " + HttpUtility.HtmlEncode(reservation.ContactName) + "</li>" +
                "<li>Phone: " + HttpUtility.HtmlEncode(reservation.Phone) + "</li>" +
                "<li>Email: " + HttpUtility.HtmlEncode(reservation.Email) + "</li>" +
                "<li>Special requests: " + HttpUtility.HtmlEncode(requests) + "</li>" +
                "</ul>";
        }

        private bool Send(String to, String subject, String body)
        {
            try
            {
                Email odjEmail = new Email();
                odjEmail.SendMail(to, FromAddress, subject, body);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
