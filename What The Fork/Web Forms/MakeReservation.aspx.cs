using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using What_The_Fork.Classes;
using What_The_Fork.Database;
using What_The_Fork.Navigation;

namespace What_The_Fork.Web_Forms
{
    public partial class MakeReservation : System.Web.UI.Page
    {
        RestaurantDB odjRestaurantDB = new RestaurantDB();
        ReservationDB odjReservationDB = new ReservationDB();
        AccountDB odjAccountDB = new AccountDB();
        ReservationValidator odjValidator = new ReservationValidator();
        ReservationEmail odjReservationEmail = new ReservationEmail();

        int restaurantID = 0;
        String restaurantName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            UserAccounts odjUser = (UserAccounts)Session["User"];

            litHeader.Text = UiHeader.GetHeader(odjUser);
            litFooter.Text = UiFooter.GetFooter();

            int.TryParse(Request.QueryString["id"], out restaurantID);

            try
            {
                DataSet dsRestaurant = odjRestaurantDB.GetRestaurant(restaurantID);
                if (dsRestaurant.Tables[0].Rows.Count == 0)
                {
                    restaurantID = 0;
                    ShowMessage("Restaurant not found",
                        "We couldn't find that restaurant. Search for it and click Reserve a Table on its page.",
                        "Search.aspx", "Find a restaurant");
                    return;
                }

                restaurantName = dsRestaurant.Tables[0].Rows[0]["RestaurantName"].ToString();
            }
            catch (Exception ex)
            {
                restaurantID = 0;
                ShowMessage("Something went wrong",
                    "We couldn't load this restaurant right now. Please try again later.",
                    "Search.aspx", "Find a restaurant");
                return;
            }

            lblRestaurantName.Text = Server.HtmlEncode(restaurantName);
            Page.Title = "Reserve a Table at " + Server.HtmlEncode(restaurantName) + " | What the Fork";
            lnkCancel.NavigateUrl = "RestaurantProfile.aspx?id=" + restaurantID;

            if (odjUser != null && odjUser.IsRepresentative)
            {
                restaurantID = 0;
                ShowMessage("Representatives can't book tables",
                    "Reservations are made by diners. You can see and confirm your own restaurant's reservations from your dashboard.",
                    "Dashboard.aspx", "Back to my dashboard");
                return;
            }

            if (!IsPostBack)
            {
                if (Session["NewReservation"] != null)
                {
                    Reservation odjReservation = (Reservation)Session["NewReservation"];
                    String emailResult = Session["ReservationEmailResult"].ToString();
                    Session.Remove("NewReservation");
                    Session.Remove("ReservationEmailResult");

                    if (odjReservation.RestaurantID == restaurantID)
                    {
                        ShowSuccess(odjReservation, emailResult);
                        return;
                    }
                }

                for (int i = 1; i <= ReservationValidator.MaxPartySize; i++)
                {
                    ddlPartySize.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
                ddlPartySize.SelectedValue = "2";

                if (odjUser != null)
                {
                    txtName.Text = (odjUser.FirstName + " " + odjUser.LastName).Trim();
                    txtPhone.Text = odjUser.PhoneNumber;
                    txtEmail.Text = odjUser.Email;
                    lblContactHint.Text = "We filled these in from your account. Change them if the restaurant should reach you another way.";
                }
            }
        }

        protected void btnReserve_Click(object sender, EventArgs e)
        {
            if (restaurantID == 0)
                return;

            UserAccounts odjUser = (UserAccounts)Session["User"];

            int partySize;
            int.TryParse(ddlPartySize.SelectedValue, out partySize);

            Reservation odjReservation = new Reservation();
            odjReservation.RestaurantID = restaurantID;
            odjReservation.ContactName = txtName.Text.Trim();
            odjReservation.Phone = txtPhone.Text.Trim();
            odjReservation.Email = txtEmail.Text.Trim();
            odjReservation.PartySize = partySize;
            odjReservation.SpecialRequests = txtRequests.Text.Trim();

            if (odjUser != null)
                odjReservation.UserID = odjUser.UserID;

            String errors = odjValidator.CheckReservation(odjReservation.ContactName, odjReservation.Phone,
                odjReservation.Email, txtDate.Text.Trim(), txtTime.Text.Trim(), partySize, odjReservation.SpecialRequests);

            if (errors != "")
            {
                ShowError(errors);
                return;
            }

            odjReservation.ReservationDate = DateTime.Parse(txtDate.Text.Trim() + " " + txtTime.Text.Trim());

            try
            {
                if (odjReservationDB.AddReversation(odjReservation) != 1)
                {
                    ShowError("Your reservation couldn't be saved. Please try again later.");
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowError("Your reservation couldn't be saved right now. Please try again later.");
                return;
            }

            String emailResult = EmailRepresentative(odjReservation);

            Session["NewReservation"] = odjReservation;
            Session["ReservationEmailResult"] = emailResult;
            Response.Redirect("MakeReservation.aspx?id=" + restaurantID);
        }

        private String EmailRepresentative(Reservation reservation)
        {
            try
            {
                DataRow row = odjRestaurantDB.GetRestaurant(restaurantID).Tables[0].Rows[0];
                if (row["RepresentativeUserID"] == DBNull.Value)
                    return "NoRep";

                UserAccounts odjRep = odjAccountDB.GetUserByID(Convert.ToInt32(row["RepresentativeUserID"]));
                if (odjRep == null || odjRep.Email == "")
                    return "NoRep";

                if (odjReservationEmail.SendNewReservation(odjRep.Email, restaurantName, reservation))
                    return "Sent";
            }
            catch (Exception ex)
            {
            }
            return "Failed";
        }

        private void ShowSuccess(Reservation reservation, String emailResult)
        {
            divForm.Visible = false;
            divSuccess.Visible = true;
            lnkBackToRestaurant.NavigateUrl = "RestaurantProfile.aspx?id=" + restaurantID;

            if (emailResult == "Sent")
            {
                lblSuccessText.Text = Server.HtmlEncode(restaurantName) + " got your request. You'll get an email at " +
                    Server.HtmlEncode(reservation.Email) + " when they confirm it.";
            }
            else
            {
                lblSuccessText.Text = "Your reservation at " + Server.HtmlEncode(restaurantName) +
                    " was saved. You'll get an email at " + Server.HtmlEncode(reservation.Email) + " when the restaurant confirms it.";
            }

            if (emailResult == "Failed")
            {
                lblEmailNote.Text = "We couldn't email the restaurant about your reservation right now, but it was saved and they'll see it when they log in.";
                lblEmailNote.Visible = true;
            }

            String requests = reservation.SpecialRequests;
            if (requests == "")
                requests = "None";

            litSummary.Text = "<dl class=\"reservation-details\">" +
                "<dt>Date</dt><dd>" + reservation.ReservationDate.ToString("dddd, MMMM d, yyyy") + "</dd>" +
                "<dt>Time</dt><dd>" + reservation.ReservationDate.ToString("h:mm tt") + "</dd>" +
                "<dt>Party size</dt><dd>" + reservation.PartySize + "</dd>" +
                "<dt>Name</dt><dd>" + Server.HtmlEncode(reservation.ContactName) + "</dd>" +
                "<dt>Phone</dt><dd>" + Server.HtmlEncode(reservation.Phone) + "</dd>" +
                "<dt>Email</dt><dd>" + Server.HtmlEncode(reservation.Email) + "</dd>" +
                "<dt>Special requests</dt><dd>" + Server.HtmlEncode(requests) + "</dd>" +
                "<dt>Status</dt><dd>Waiting for the restaurant to confirm</dd>" +
                "</dl>";
        }

        private void ShowMessage(String title, String text, String linkUrl, String linkText)
        {
            lblMessageTitle.Text = title;
            lblMessageText.Text = text;
            lnkMessage.NavigateUrl = linkUrl;
            lnkMessage.Text = linkText;
            divMessage.Visible = true;
            divForm.Visible = false;
        }

        private void ShowError(String message)
        {
            lblValidation.Text = message;
            lblValidation.Visible = true;
        }
    }
}
