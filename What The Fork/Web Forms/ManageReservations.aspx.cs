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
    public partial class ManageReservations : System.Web.UI.Page
    {
        RestaurantDB odjRestaurantDB = new RestaurantDB();
        ReservationDB odjReservationDB = new ReservationDB();
        ReservationValidator odjValidator = new ReservationValidator();
        ReservationEmail odjReservationEmail = new ReservationEmail();

        const int ConfirmColumn = 7;

        int restaurantID = 0;
        String restaurantName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            UserAccounts odjUser = (UserAccounts)Session["User"];

            litHeader.Text = UiHeader.GetHeader(odjUser);
            litFooter.Text = UiFooter.GetFooter();

            if (odjUser == null)
            {
                ShowMessage("Log in to manage reservations",
                    "You need a representative account to manage your restaurant's reservations. Log in, or register for free.",
                    "Login.aspx", "Log in");
                return;
            }

            if (!odjUser.IsRepresentative)
            {
                ShowMessage("Representatives only",
                    "Only restaurant representative accounts can manage reservations. To book a table, find a restaurant and click Reserve a Table on its page.",
                    "Dashboard.aspx", "Back to my dashboard");
                return;
            }

            try
            {
                DataSet dsRestaurant = odjRestaurantDB.GetResturantByRep(odjUser.UserID);

                if (dsRestaurant.Tables[0].Rows.Count == 0)
                {
                    ShowMessage("Add your restaurant first",
                        "Once your restaurant is listed, diners can book a table and their reservations show up here.",
                        "ManageRestaurant.aspx", "Add my restaurant");
                    return;
                }

                DataRow restaurantRow = dsRestaurant.Tables[0].Rows[0];
                restaurantID = Convert.ToInt32(restaurantRow["RestaurantID"]);
                restaurantName = restaurantRow["RestaurantName"].ToString();

                lblRestaurantName.Text = Server.HtmlEncode(restaurantName);

                if (!IsPostBack)
                {
                    for (int i = 1; i <= ReservationValidator.MaxPartySize; i++)
                    {
                        ddlPartySize.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }

                    ShowReservations();
                }
            }
            catch (Exception ex)
            {
                restaurantID = 0;
                ShowMessage("Something went wrong",
                    "We couldn't load your restaurant's reservations right now. Please try again later.",
                    "Dashboard.aspx", "Back to my dashboard");
            }
        }

        private void ShowReservations()
        {
            try
            {
                DataSet dsReservations = odjReservationDB.GetReservations(restaurantID);
                DataTable odjTable = dsReservations.Tables[0];

                int pendingCount = 0;

                for (int i = odjTable.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = odjTable.Rows[i];
                    String status = row["Status"].ToString();

                    if (status == "Pending")
                        pendingCount = pendingCount + 1;

                    if (ddlFilter.SelectedValue != "All" && status != ddlFilter.SelectedValue)
                        odjTable.Rows.Remove(row);
                }

                lblReservationCount.Text = pendingCount + " waiting to be confirmed";

                if (ddlFilter.SelectedValue == "Pending")
                    gvReservations.EmptyDataText = "Every reservation is confirmed. Choose All reservations to see them.";
                else if (ddlFilter.SelectedValue == "Confirmed")
                    gvReservations.EmptyDataText = "No reservations have been confirmed yet.";
                else
                    gvReservations.EmptyDataText = "Your restaurant doesn't have any reservations yet. When diners book a table, their reservations show up here.";

                gvReservations.DataSource = odjTable;
                gvReservations.DataBind();
            }
            catch (Exception ex)
            {
                ShowError("We couldn't load your reservations right now. Please try again later.");
            }
        }

        protected void gvReservations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            DataRowView row = (DataRowView)e.Row.DataItem;
            String status = row["Status"].ToString();
            if (status == "Confirmed")
            {
                e.Row.Cells[ConfirmColumn].Controls[0].Visible = false;
                e.Row.Cells[ConfirmColumn - 1].CssClass = "status-confirmed";
            }
            else
            {
                e.Row.Cells[ConfirmColumn - 1].CssClass = "status-pending";
            }
        }

        protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (restaurantID == 0)
                return;

            gvReservations.PageIndex = 0;
            ShowReservations();
        }

        protected void gvReservations_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (restaurantID == 0)
                return;

            gvReservations.PageIndex = e.NewPageIndex;
            ShowReservations();
        }

        protected void gvReservations_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Page" || restaurantID == 0)
                return;

            int rowIndex = int.Parse(e.CommandArgument.ToString());
            int reservationID = Convert.ToInt32(gvReservations.DataKeys[rowIndex].Values["ReservationID"]);

            divConfirmDelete.Visible = false;
            divEdit.Visible = false;

            Reservation odjReservation;
            try
            {
                odjReservation = odjReservationDB.GetReservationByID(reservationID, restaurantID);
            }
            catch (Exception ex)
            {
                ShowError("That reservation couldn't be loaded right now. Please try again later.");
                return;
            }

            if (odjReservation == null)
            {
                ShowError("That reservation couldn't be found. It may have been deleted.");
                ShowReservations();
                return;
            }

            if (e.CommandName == "ConfirmReservation")
            {
                ConfirmReservation(odjReservation);
            }
            else if (e.CommandName == "EditReservation")
            {
                ShowEditForm(odjReservation);
            }
            else if (e.CommandName == "DeleteReservation")
            {
                ViewState["DeleteReservationID"] = reservationID;
                lblConfirmDelete.Text = "Cancel the reservation for " + Server.HtmlEncode(odjReservation.ContactName) + " on " +
                    odjReservation.ReservationDate.ToString("dddd, MMMM d 'at' h:mm tt") +
                    "? It will be deleted and they'll get an email saying it was cancelled. This can't be undone.";
                divConfirmDelete.Visible = true;
            }
        }

        private void ConfirmReservation(Reservation reservation)
        {
            if (reservation.Status == "Confirmed")
            {
                ShowError("That reservation is already confirmed.");
                return;
            }

            try
            {
                if (odjReservationDB.ConfirmReservation(reservation.ReservationID, restaurantID) != 1)
                {
                    ShowError("That reservation couldn't be confirmed. It may have been deleted.");
                    ShowReservations();
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowError("That reservation couldn't be confirmed right now. Please try again later.");
                return;
            }

            ShowSuccess("The reservation for " + reservation.ContactName + " was confirmed.",
                odjReservationEmail.SendConfirmed(restaurantName, reservation), reservation);
            ShowReservations();
        }

        private void ShowEditForm(Reservation reservation)
        {
            ViewState["EditReservationID"] = reservation.ReservationID;

            lblEditTitle.Text = "Change the reservation for " + Server.HtmlEncode(reservation.ContactName);
            txtDate.Text = reservation.ReservationDate.ToString("yyyy-MM-dd");
            txtTime.Text = reservation.ReservationDate.ToString("HH:mm");
            ddlPartySize.ClearSelection();
            if (ddlPartySize.Items.FindByValue(reservation.PartySize.ToString()) != null)
                ddlPartySize.SelectedValue = reservation.PartySize.ToString();
            txtName.Text = reservation.ContactName;
            txtPhone.Text = reservation.Phone;
            txtEmail.Text = reservation.Email;
            txtRequests.Text = reservation.SpecialRequests;

            divEdit.Visible = true;
        }

        protected void btnSaveEdit_Click(object sender, EventArgs e)
        {
            if (restaurantID == 0 || ViewState["EditReservationID"] == null)
            {
                divEdit.Visible = false;
                return;
            }

            int partySize;
            int.TryParse(ddlPartySize.SelectedValue, out partySize);

            Reservation odjReservation = new Reservation();
            odjReservation.ReservationID = Convert.ToInt32(ViewState["EditReservationID"]);
            odjReservation.RestaurantID = restaurantID;
            odjReservation.ContactName = txtName.Text.Trim();
            odjReservation.Phone = txtPhone.Text.Trim();
            odjReservation.Email = txtEmail.Text.Trim();
            odjReservation.PartySize = partySize;
            odjReservation.SpecialRequests = txtRequests.Text.Trim();

            String errors = odjValidator.CheckReservation(odjReservation.ContactName, odjReservation.Phone,
                odjReservation.Email, txtDate.Text.Trim(), txtTime.Text.Trim(), partySize, odjReservation.SpecialRequests);

            if (errors != "")
            {
                lblEditValidation.Text = errors;
                lblEditValidation.Visible = true;
                return;
            }

            odjReservation.ReservationDate = DateTime.Parse(txtDate.Text.Trim() + " " + txtTime.Text.Trim());

            try
            {
                if (odjReservationDB.UpdateReservation(odjReservation) != 1)
                {
                    ShowError("That reservation couldn't be changed. It may have been deleted.");
                    ViewState.Remove("EditReservationID");
                    divEdit.Visible = false;
                    ShowReservations();
                    return;
                }
            }
            catch (Exception ex)
            {
                lblEditValidation.Text = "Your changes couldn't be saved right now. Please try again later.";
                lblEditValidation.Visible = true;
                return;
            }

            ViewState.Remove("EditReservationID");
            divEdit.Visible = false;

            ShowSuccess("The reservation for " + odjReservation.ContactName + " was changed.",
                odjReservationEmail.SendChanged(restaurantName, odjReservation), odjReservation);
            ShowReservations();
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            ViewState.Remove("EditReservationID");
            divEdit.Visible = false;
        }

        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            divConfirmDelete.Visible = false;

            if (restaurantID == 0 || ViewState["DeleteReservationID"] == null)
                return;

            int reservationID = Convert.ToInt32(ViewState["DeleteReservationID"]);
            ViewState.Remove("DeleteReservationID");

            try
            {
                Reservation odjReservation = odjReservationDB.GetReservationByID(reservationID, restaurantID);

                if (odjReservation == null || odjReservationDB.DeleteReservation(reservationID, restaurantID) != 1)
                {
                    ShowError("That reservation couldn't be deleted. It may already be gone.");
                }
                else
                {
                    ShowSuccess("The reservation for " + odjReservation.ContactName + " was deleted.",
                        odjReservationEmail.SendCancelled(restaurantName, odjReservation), odjReservation);
                }
            }
            catch (Exception ex)
            {
                ShowError("That reservation couldn't be deleted right now. Please try again later.");
            }

            ShowReservations();
        }

        protected void btnCancelDelete_Click(object sender, EventArgs e)
        {
            ViewState.Remove("DeleteReservationID");
            divConfirmDelete.Visible = false;
        }

        private void ShowSuccess(String message, bool wasEmailSent, Reservation reservation)
        {
            if (wasEmailSent)
                lblMessage.Text = Server.HtmlEncode(message) + " We emailed " + Server.HtmlEncode(reservation.Email) + " to let them know.";
            else
                lblMessage.Text = Server.HtmlEncode(message);
            lblMessage.Visible = true;

            if (!wasEmailSent)
            {
                lblEmailNote.Text = "The email to " + Server.HtmlEncode(reservation.Email) +
                    " couldn't be sent right now. You may want to call them at " + Server.HtmlEncode(reservation.Phone) + ".";
                lblEmailNote.Visible = true;
            }
        }

        private void ShowMessage(String title, String text, String linkUrl, String linkText)
        {
            lblMessageTitle.Text = title;
            lblMessageText.Text = text;
            lnkMessage.NavigateUrl = linkUrl;
            lnkMessage.Text = linkText;
            divMessage.Visible = true;
            divReservations.Visible = false;
        }

        private void ShowError(String message)
        {
            lblValidation.Text = message;
            lblValidation.Visible = true;
        }
    }
}
