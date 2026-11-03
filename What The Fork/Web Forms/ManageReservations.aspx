<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageReservations.aspx.cs" Inherits="What_The_Fork.Web_Forms.ManageReservations" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en" data-bs-theme="dark">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Manage Reservations | What the Fork</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Styling/theme.css" rel="stylesheet" />
</head>
<body>
    <form id="frmManageReservations" runat="server">
        <asp:Literal ID="litHeader" runat="server"></asp:Literal>

        <div class="search-hero">
            <div class="container">
                <p class="eyebrow">Representative dashboard</p>
                <h1>Manage Reservations</h1>
                <p class="lead"><asp:Label ID="lblInstructions" runat="server"
                    Text="Confirm, change or cancel your restaurant's reservations. The customer gets an email each time."></asp:Label></p>
            </div>
        </div>

        <div class="container search-main">

            <div class="row justify-content-center">
                <div class="col-lg-10 col-xl-9">
                    <div id="divMessage" runat="server" class="account-card text-center" visible="false">
                        <h2 class="filter-title"><asp:Label ID="lblMessageTitle" runat="server"></asp:Label></h2>
                        <p class="filter-hint"><asp:Label ID="lblMessageText" runat="server"></asp:Label></p>
                        <asp:HyperLink ID="lnkMessage" runat="server" CssClass="btn btn-wtf"></asp:HyperLink>
                    </div>
                </div>
            </div>

            <div id="divReservations" runat="server">
                <asp:Label ID="lblMessage" runat="server" CssClass="success-message d-block" Visible="false" EnableViewState="false" />
                <asp:Label ID="lblEmailNote" runat="server" CssClass="email-note d-block" Visible="false" EnableViewState="false" />
                <asp:Label ID="lblValidation" runat="server" CssClass="validation-summary d-block" Visible="false" EnableViewState="false" />

                <div id="divConfirmDelete" runat="server" class="account-card confirm-card" visible="false">
                    <p><asp:Label ID="lblConfirmDelete" runat="server"></asp:Label></p>
                    <asp:Button ID="btnConfirmDelete" runat="server" Text="Yes, cancel it" CssClass="btn btn-wtf"
                        OnClick="btnConfirmDelete_Click" />
                    <asp:Button ID="btnCancelDelete" runat="server" Text="No, keep it" CssClass="btn btn-outline-light"
                        OnClick="btnCancelDelete_Click" />
                </div>

                <div id="divEdit" runat="server" class="account-card reservation-edit" visible="false">
                    <h2 class="filter-title"><asp:Label ID="lblEditTitle" runat="server" Text="Change the reservation"></asp:Label></h2>
                    <p class="filter-hint">Change anything below and click Save changes. The customer gets an email with the new details.</p>

                    <asp:Label ID="lblEditValidation" runat="server" CssClass="validation-summary d-block" Visible="false" EnableViewState="false" />

                    <div class="row">
                        <div class="col-md-4 account-field">
                            <asp:Label ID="lblDate" runat="server" AssociatedControlID="txtDate" CssClass="filter-label" Text="Date"></asp:Label>
                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control mt-1" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="col-md-4 account-field">
                            <asp:Label ID="lblTime" runat="server" AssociatedControlID="txtTime" CssClass="filter-label" Text="Time"></asp:Label>
                            <asp:TextBox ID="txtTime" runat="server" CssClass="form-control mt-1" TextMode="Time"></asp:TextBox>
                        </div>
                        <div class="col-md-4 account-field">
                            <asp:Label ID="lblPartySize" runat="server" AssociatedControlID="ddlPartySize" CssClass="filter-label" Text="Party size"></asp:Label>
                            <asp:DropDownList ID="ddlPartySize" runat="server" CssClass="form-select mt-1"></asp:DropDownList>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-4 account-field">
                            <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" CssClass="filter-label" Text="Name"></asp:Label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control mt-1" MaxLength="100"></asp:TextBox>
                        </div>
                        <div class="col-md-4 account-field">
                            <asp:Label ID="lblPhone" runat="server" AssociatedControlID="txtPhone" CssClass="filter-label" Text="Phone"></asp:Label>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control mt-1" TextMode="Phone" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="col-md-4 account-field">
                            <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" CssClass="filter-label" Text="Email"></asp:Label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control mt-1" MaxLength="100"></asp:TextBox>
                        </div>
                    </div>

                    <div class="account-field">
                        <asp:Label ID="lblRequests" runat="server" AssociatedControlID="txtRequests" CssClass="filter-label" Text="Special requests (optional)"></asp:Label>
                        <asp:TextBox ID="txtRequests" runat="server" CssClass="form-control mt-1" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>

                    <asp:Button ID="btnSaveEdit" runat="server" Text="Save changes" CssClass="btn btn-wtf"
                        OnClick="btnSaveEdit_Click" />
                    <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel" CssClass="btn btn-outline-light"
                        OnClick="btnCancelEdit_Click" />
                </div>

                <div class="results-toolbar d-flex flex-wrap justify-content-between align-items-center gap-2 mb-3">
                    <div>
                        <h2 class="results-count"><asp:Label ID="lblRestaurantName" runat="server"></asp:Label></h2>
                        <asp:Label ID="lblReservationCount" runat="server" CssClass="results-criteria"></asp:Label>
                    </div>
                    <div class="respond-filter">
                        <asp:Label ID="lblFilter" runat="server" AssociatedControlID="ddlFilter" CssClass="filter-label" Text="Show"></asp:Label>
                        <asp:DropDownList ID="ddlFilter" runat="server" CssClass="form-select form-select-sm" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged">
                            <asp:ListItem Value="All" Text="All reservations" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="Pending" Text="Waiting to be confirmed"></asp:ListItem>
                            <asp:ListItem Value="Confirmed" Text="Confirmed"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="table-responsive">
                    <asp:GridView ID="gvReservations" runat="server" AutoGenerateColumns="False"
                        CssClass="table align-middle results-grid reservation-grid" GridLines="None"
                        DataKeyNames="ReservationID" AllowPaging="True" PageSize="10"
                        OnPageIndexChanging="gvReservations_PageIndexChanging"
                        OnRowCommand="gvReservations_RowCommand"
                        OnRowDataBound="gvReservations_RowDataBound"
                        EmptyDataText="There are no reservations to show.">
                        <Columns>
                            <asp:BoundField DataField="ContactName" HeaderText="Name" ItemStyle-CssClass="restaurant-name" />
                            <asp:BoundField DataField="Phone" HeaderText="Phone" ItemStyle-CssClass="muted-cell" />
                            <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-CssClass="muted-cell" />
                            <asp:BoundField DataField="ReservationDate" HeaderText="Date and time" DataFormatString="{0:ddd, MMM d, yyyy h:mm tt}" />
                            <asp:BoundField DataField="PartySize" HeaderText="Party"
                                ItemStyle-CssClass="score" HeaderStyle-CssClass="text-center" />
                            <asp:BoundField DataField="SpecialRequests" HeaderText="Special requests" NullDisplayText="-"
                                ItemStyle-CssClass="muted-cell reservation-requests" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />

                            <asp:ButtonField ButtonType="Button" Text="Confirm" CommandName="ConfirmReservation"
                                ControlStyle-CssClass="btn-view" />
                            <asp:ButtonField ButtonType="Button" Text="Edit" CommandName="EditReservation"
                                ControlStyle-CssClass="btn-view" />
                            <asp:ButtonField ButtonType="Button" Text="Delete" CommandName="DeleteReservation"
                                ControlStyle-CssClass="btn-delete" />
                        </Columns>
                        <HeaderStyle CssClass="grid-header" />
                        <RowStyle CssClass="grid-row" />
                        <AlternatingRowStyle CssClass="grid-row grid-alt" />
                        <PagerStyle CssClass="grid-pager" />
                        <EmptyDataRowStyle CssClass="grid-empty" />
                    </asp:GridView>
                </div>
            </div>
        </div>

        <asp:Literal ID="litFooter" runat="server"></asp:Literal>
    </form>
    <script src="<%= ResolveUrl("~/Scripts/bootstrap.bundle.min.js") %>"></script>
</body>
</html>
