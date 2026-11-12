using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhatTheForkMVC.Classes.Restaurant;

namespace WhatTheForkMVC.Classes
{
    public class ReservationValidator
    {
        RestaurantValidator odjChecks = new RestaurantValidator();

        public const int MaxPartySize = 20;

        public String CheckReservation(String name, String phone, String email,
            String date, String time, int partySize, String requests)
        {
            String errors = "";

            if (name == "")
            {
                errors = errors + "Name is required.<br />";
            }
            else if (name.Length > 100)
            {
                errors = errors + "Name must be 100 characters or less.<br />";
            }

            if (phone == "")
            {
                errors = errors + "Phone number is required.<br />";
            }
            else if (!odjChecks.IsValidPhone(phone) || phone.Length > 20)
            {
                errors = errors + "Phone number must be 10 digits, like 267-867-5309.<br />";
            }

            if (email == "")
            {
                errors = errors + "Email address is required.<br />";
            }
            else if (!odjChecks.IsValidEmail(email))
            {
                errors = errors + "Email address must be a real email address, like chris@temple.edu.<br />";
            }
            else if (email.Length > 100)
            {
                errors = errors + "Email address must be 100 characters or less.<br />";
            }

            if (date == "" || time == "")
            {
                errors = errors + "Date and time are required.<br />";
            }
            else
            {
                DateTime reservationDate;
                if (!DateTime.TryParse(date + " " + time, out reservationDate))
                {
                    errors = errors + "Please enter a real date and time.<br />";
                }
                else if (reservationDate <= DateTime.Now)
                {
                    errors = errors + "The reservation has to be for a date and time in the future.<br />";
                }
                else if (reservationDate > DateTime.Now.AddYears(1))
                {
                    errors = errors + "Reservations can only be made up to one year ahead.<br />";
                }
            }

            if (partySize < 1 || partySize > MaxPartySize)
            {
                errors = errors + "Party size is required.<br />";
            }

            if (requests.Length > 1000)
            {
                errors = errors + "Special requests must be 1000 characters or less.<br />";
            }

            return errors;
        }
    }
}
