using Microsoft.AspNetCore.Mvc;
using System.Data;
using WhatTheForkMVC.Classes;
using WhatTheForkMVC.Classes.Login;
using WhatTheForkMVC.Classes.Reservation;
using WhatTheForkMVC.Database;

namespace WhatTheForkMVC.Controllers
{
    public class ReservationController : Controller
    {
        RestaurantDB objRestaurantDB = new RestaurantDB();
        ReservationDB objReservationDB = new ReservationDB();
        AccountDB objAccountDB = new AccountDB();
        ReservationValidator objValidator = new ReservationValidator();
        ReservationEmail objReservationEmail = new ReservationEmail();

        int restaurantID = 0;
        String restaurantName = "";
        public IActionResult MakeReservation(int id)
        {
            if (!LoadRestaurant(id))
            {
                return View("MakeReservation", new Reservation());
            }

            Reservation objReservation = new Reservation();
            objReservation.RestaurantID = id;
            objReservation.PartySize = 2;

            UserAccounts objUser = SessionUser.GetUser(HttpContext.Session);
            if (objUser != null)
            {
                try
                {
                    UserAccounts objAccount = objAccountDB.GetUserByID(objUser.UserID);
                    objReservation.ContactName = (objAccount.FirstName + " " + objAccount.LastName).Trim();
                    objReservation.Phone = objAccount.PhoneNumber;
                    objReservation.Email = objAccount.Email;
                    ViewData["ContactHint"] = "We filled these in from your account. You can change if needed";
                }
                catch (Exception ex)
                {
                }
            }

            return View("MakeReservation", objReservation);
        }

        [HttpPost]
        public IActionResult MakeReservation(int id, String reservationDate, String reservationTime, int partySize,
            String contactName, String phone, String email, String specialRequests)
        {
            if (!LoadRestaurant(id))
            {
                return View("MakeReservation", new Reservation());
            }

            if (reservationDate == null)
            {
                reservationDate = "";
            }
            if (reservationTime == null)
            {
                reservationTime = "";
            }
            if (contactName == null)
            {
                contactName = "";
            }
            if (phone == null)
            {
                phone = "";
            }
            if (email == null)
            {
                email = "";
            }
            if (specialRequests == null)
            {
                specialRequests = "";
            }

            Reservation objReservation = new Reservation();
            objReservation.RestaurantID = id;
            objReservation.ContactName = contactName.Trim();
            objReservation.Phone = phone.Trim();
            objReservation.Email = email.Trim();
            objReservation.PartySize = partySize;
            objReservation.SpecialRequests = specialRequests.Trim();

            UserAccounts objUser = SessionUser.GetUser(HttpContext.Session);
            if (objUser != null)
            {
                objReservation.UserID = objUser.UserID;
            }

            ViewData["ReservationDate"] = reservationDate.Trim();
            ViewData["ReservationTime"] = reservationTime.Trim();

            String errors = objValidator.CheckReservation(objReservation.ContactName, objReservation.Phone,
                objReservation.Email, reservationDate.Trim(), reservationTime.Trim(), partySize, objReservation.SpecialRequests);

            if (errors != "")
            {
                ViewData["Error"] = errors;
                return View("MakeReservation", objReservation);
            }

            objReservation.ReservationDate = DateTime.Parse(reservationDate.Trim() + " " + reservationTime.Trim());

            try
            {
                if (objReservationDB.AddReversation(objReservation) != 1)
                {
                    ViewData["Error"] = "Your reservation couldn't be saved. Please try again later.";
                    return View("MakeReservation", objReservation);
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Your reservation couldn't be saved right now. Please try again later.";
                return View("MakeReservation", objReservation);
            }

            String emailResult = EmailRepresentative(objReservation);
            String restaurantName = ViewData["RestaurantName"].ToString();

            if (emailResult == "Sent")
            {
                ViewData["SuccessText"] = restaurantName + " got your request. You'll get an email at " +
                    objReservation.Email + " when they confirm it.";
            }
            else
            {
                ViewData["SuccessText"] = "Your reservation at " + restaurantName +
                    " was saved. You'll get an email at " + objReservation.Email + " when the restaurant confirms it.";
            }

            if (emailResult == "Failed")
            {
                ViewData["EmailNote"] = "Error  emailing restaurant regarding your reservation. It was saved and the restaurant will see it when they log in.";
            }

            ViewData["Success"] = true;
            return View("MakeReservation", objReservation);
        }

        public IActionResult ManageReservation(String filter)
        {
            if (!LoadRepRestaurant())
            {
                return View("ManageReservation", new DataTable());
            }

            return ShowReservations(filter);
        }

        [HttpPost]
        public IActionResult ConfirmReservation(int reservationID, String filter)
        {
            if (!LoadRepRestaurant())
            {
                return View("ManageReservation", new DataTable());
            }

            Reservation objReservation = LoadReservation(reservationID);
            if (objReservation == null)
            {
                return ShowReservations(filter);
            }

            if (objReservation.Status == "Confirmed")
            {
                ViewData["Error"] = "That reservation is already confirmed.";
                return ShowReservations(filter);
            }

            try
            {
                if (objReservationDB.ConfirmReservation(reservationID, restaurantID) != 1)
                {
                    ViewData["Error"] = "That reservation couldn't be confirmed. It may have been deleted.";
                    return ShowReservations(filter);
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "That reservation couldn't be confirmed right now. Please try again later.";
                return ShowReservations(filter);
            }

            ShowSuccess("The reservation for " + objReservation.ContactName + " was confirmed.",
                objReservationEmail.SendConfirmed(restaurantName, objReservation), objReservation);
            return ShowReservations(filter);
        }

        public IActionResult EditReservation(int id, String filter)
        {
            if (!LoadRepRestaurant())
            {
                return View("ManageReservation", new DataTable());
            }

            Reservation objReservation = LoadReservation(id);
            if (objReservation != null)
            {
                ViewData["EditReservation"] = objReservation;
                ViewData["EditDate"] = objReservation.ReservationDate.ToString("yyyy-MM-dd");
                ViewData["EditTime"] = objReservation.ReservationDate.ToString("HH:mm");
                ViewData["EditTitle"] = "Change the reservation for " + objReservation.ContactName;
            }

            return ShowReservations(filter);
        }

        [HttpPost]
        public IActionResult EditReservation(int reservationID, String filter, String reservationDate, String reservationTime,
            int partySize, String contactName, String phone, String email, String specialRequests)
        {
            if (!LoadRepRestaurant())
            {
                return View("ManageReservation", new DataTable());
            }

            if (reservationDate == null)
            {
                reservationDate = "";
            }
            if (reservationTime == null)
            {
                reservationTime = "";
            }
            if (contactName == null)
            {
                contactName = "";
            }
            if (phone == null)
            {
                phone = "";
            }
            if (email == null)
            {
                email = "";
            }
            if (specialRequests == null)
            {
                specialRequests = "";
            }

            Reservation objReservation = new Reservation();
            objReservation.ReservationID = reservationID;
            objReservation.RestaurantID = restaurantID;
            objReservation.ContactName = contactName.Trim();
            objReservation.Phone = phone.Trim();
            objReservation.Email = email.Trim();
            objReservation.PartySize = partySize;
            objReservation.SpecialRequests = specialRequests.Trim();

            String errors = objValidator.CheckReservation(objReservation.ContactName, objReservation.Phone,
                objReservation.Email, reservationDate.Trim(), reservationTime.Trim(), partySize, objReservation.SpecialRequests);

            if (errors != "")
            {
                ViewData["EditError"] = errors;
                ViewData["EditReservation"] = objReservation;
                ViewData["EditDate"] = reservationDate.Trim();
                ViewData["EditTime"] = reservationTime.Trim();
                ViewData["EditTitle"] = "Change the reservation";
                return ShowReservations(filter);
            }

            objReservation.ReservationDate = DateTime.Parse(reservationDate.Trim() + " " + reservationTime.Trim());

            try
            {
                if (objReservationDB.UpdateReservation(objReservation) != 1)
                {
                    ViewData["Error"] = "That reservation couldn't be changed. It may have been deleted.";
                    return ShowReservations(filter);
                }
            }
            catch (Exception ex)
            {
                ViewData["EditError"] = "Your changes couldn't be saved right now. Please try again later.";
                ViewData["EditReservation"] = objReservation;
                ViewData["EditDate"] = reservationDate.Trim();
                ViewData["EditTime"] = reservationTime.Trim();
                ViewData["EditTitle"] = "Change the reservation";
                return ShowReservations(filter);
            }

            ShowSuccess("The reservation for " + objReservation.ContactName + " was changed.",
                objReservationEmail.SendChanged(restaurantName, objReservation), objReservation);
            return ShowReservations(filter);
        }

        public IActionResult ConfirmDeleteReservation(int id, String filter)
        {
            if (!LoadRepRestaurant())
            {
                return View("ManageReservation", new DataTable());
            }

            Reservation objReservation = LoadReservation(id);
            if (objReservation != null) 
            {
                ViewData["DeleteReservationID"] = id;
                ViewData["ConfirmDelete"] = "Cancel the reservation for " + objReservation.ContactName + " on " +
                    objReservation.ReservationDate.ToString("dddd, MMMM d 'at' h:mm tt") +
                    "? It will be deleted and they'll get an email saying it was cancelled. This can't be undone.";
            }

            return ShowReservations(filter);
        }

        [HttpPost]
        public IActionResult DeleteReservation(int reservationID, String filter)
        {
            if (!LoadRepRestaurant())
            {
                return View("ManageReservation", new DataTable());
            }

            try
            {
                Reservation objReservation = objReservationDB.GetReservationByID(reservationID, restaurantID);

                if (objReservation == null || objReservationDB.DeleteReservation(reservationID, restaurantID) != 1)
                {
                    ViewData["Error"] = "That reservation couldn't be deleted. It may already be gone.";
                }
                else
                {
                    ShowSuccess("The reservation for " + objReservation.ContactName + " was deleted.",
                        objReservationEmail.SendCancelled(restaurantName, objReservation), objReservation);
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "That reservation couldn't be deleted right now. Please try again later.";
            }

            return ShowReservations(filter);
        }
        private bool LoadRestaurant(int restaurantID)
        {
            try
            {
                DataSet dsRestaurant = objRestaurantDB.GetRestaurant(restaurantID);
                if (dsRestaurant.Tables[0].Rows.Count == 0)
                {
                    ShowMessage("Restaurant not found",
                        "We couldn't find that restaurant. Search for it and click Reserve a Table on its page.",
                        "~/Restaurant/Search", "Find a restaurant");
                    return false;
                }

                DataRow restaurant = dsRestaurant.Tables[0].Rows[0];
                ViewData["RestaurantName"] = restaurant["RestaurantName"].ToString();
                ViewData["RestaurantID"] = restaurantID;
                ViewData["RepUserID"] = restaurant["RepresentativeUserID"];
            }
            catch (Exception ex)
            {
                ShowMessage("Something went wrong",
                    "We couldn't load this restaurant right now. Please try again later.",
                    "~/Restaurant/Search", "Find a restaurant");
                return false;
            }

            UserAccounts objUser = SessionUser.GetUser(HttpContext.Session);
            if (objUser != null && objUser.IsRepresentative)
            {
                ShowMessage("Representatives can't book tables",
                    "Reservations are made by diners. You can see and confirm your own restaurant's reservations from your dashboard.",
                    "~/Dashboard/Index", "Back to my dashboard");
                return false;
            }

            return true;
        }

        private String EmailRepresentative(Reservation reservation)
        {
            try
            {
                if (ViewData["RepUserID"] == DBNull.Value)
                {
                    return "NoRep";
                }

                UserAccounts objRep = objAccountDB.GetUserByID(Convert.ToInt32(ViewData["RepUserID"]));
                if (objRep == null || objRep.Email == "")
                {
                    return "NoRep";
                }

                if (objReservationEmail.SendNewReservation(objRep.Email, ViewData["RestaurantName"].ToString(), reservation))
                {
                    return "Sent";
                }
            }
            catch (Exception ex)
            {
            }
            return "Failed";
        }

        private void ShowMessage(String title, String text, String linkUrl, String linkText)
        {
            ViewData["MessageTitle"] = title;
            ViewData["MessageText"] = text;
            ViewData["MessageLink"] = linkUrl;
            ViewData["MessageLinkText"] = linkText;
        }

        private void ShowSuccess(String message, bool wasEmailSent, Reservation reservation)
        {
            if (wasEmailSent)
            {
                ViewData["Message"] = message + " We emailed " + reservation.Email + " to let them know.";
            }
            else
            {
                ViewData["Message"] = message;
                ViewData["EmailNote"] = "The email to " + reservation.Email +
                    " couldn't be sent right now. You may want to call them at " + reservation.Phone + ".";
            }
        }


        private bool LoadRepRestaurant()
        {
            UserAccounts objUser = SessionUser.GetUser(HttpContext.Session);

            if (objUser == null)
            {
                ShowMessage("Log in to manage reservations",
                    "You need a representative account to manage your restaurant's reservations. Log in, or register for free.",
                    "~/Account/Login", "Log in");
                return false;
            }

            if (!objUser.IsRepresentative)
            {
                ShowMessage("Representatives only",
                    "Only restaurant representative accounts can manage reservations. To book a table, find a restaurant and click Reserve a Table on its page.",
                    "~/Dashboard/Index", "Back to my dashboard");
                return false;
            }

            try
            {
                DataSet dsRestaurant = objRestaurantDB.GetResturantByRep(objUser.UserID);

                if (dsRestaurant.Tables[0].Rows.Count == 0)
                {
                    ShowMessage("Add your restaurant first",
                        "Once your restaurant is listed, diners can book a table and their reservations show up here.",
                        "~/Restaurant/Manage", "Add my restaurant");
                    return false;
                }

                DataRow restaurantRow = dsRestaurant.Tables[0].Rows[0];
                restaurantID = Convert.ToInt32(restaurantRow["RestaurantID"]);
                restaurantName = restaurantRow["RestaurantName"].ToString();
                ViewData["RestaurantName"] = restaurantName;
            }
            catch (Exception ex)
            {
                ShowMessage("Something went wrong",
                    "We couldn't load your restaurant's reservations right now. Please try again later.",
                    "~/Dashboard/Index", "Back to my dashboard");
                return false;
            }

            return true;
        }

        private Reservation LoadReservation(int reservationID)
        {
            Reservation objReservation = null;

            try
            {
                objReservation = objReservationDB.GetReservationByID(reservationID, restaurantID);
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "That reservation couldn't be loaded right now. Please try again later.";
                return null;
            }

            if (objReservation == null)
            {
                ViewData["Error"] = "That reservation couldn't be found. It may have been deleted.";
            }

            return objReservation;
        }

        private IActionResult ShowReservations(String filter)
        {
            if (filter != "Pending" && filter != "Confirmed")
            {
                filter = "All";
            }
            ViewData["Filter"] = filter;

            if (filter == "Pending")
            {
                ViewData["EmptyText"] = "Every reservation is confirmed. Choose All reservations to see them.";
            }
            else if (filter == "Confirmed")
            {
                ViewData["EmptyText"] = "No reservations have been confirmed yet.";
            }
            else
            {
                ViewData["EmptyText"] = "Your restaurant doesn't have any reservations yet. When diners book a table, their reservations show up here.";
            }

            DataTable objTable = new DataTable();

            try
            {
                DataSet dsReservations = objReservationDB.GetReservations(restaurantID);
                objTable = dsReservations.Tables[0];

                int pendingCount = 0;

                for (int i = objTable.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = objTable.Rows[i];
                    String status = row["Status"].ToString();

                    if (status == "Pending")
                    {
                        pendingCount = pendingCount + 1;
                    }

                    if (filter != "All" && status != filter)
                    {
                        objTable.Rows.Remove(row);
                    }
                }

                ViewData["ReservationCount"] = pendingCount + " waiting to be confirmed";
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "We couldn't load your reservations right now. Please try again later.";
            }

            return View("ManageReservation", objTable);
        }




    }
}
