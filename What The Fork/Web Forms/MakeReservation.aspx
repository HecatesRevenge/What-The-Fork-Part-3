<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MakeReservation.aspx.cs" Inherits="What_The_Fork.Web_Forms.MakeReservation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en" data-bs-theme="dark">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Reserve a Table | What the Fork</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Styling/theme.css" rel="stylesheet" />
</head>
<body>
    <form id="frmMakeReservation" runat="server">
        <asp:Literal ID="litHeader" runat="server"></asp:Literal>

        <div class="search-hero">
            <div class="container">
                <p class="eyebrow">Reserve a table</p>
                <h1><asp:Label ID="lblRestaurantName" runat="server" Text="Book a table"></asp:Label></h1>
                <p class="lead">Choose a date, time and party size. The restaurant gets your request and confirms it by email.</p>
            </div>
        </div>

        <div class="container search-main">
            <div class="row justify-content-center">
                <div class="col-lg-9 col-xl-8">

                    <div id="divMessage" runat="server" class="account-card text-center" visible="false">
                        <h2 class="filter-title"><asp:Label ID="lblMessageTitle" runat="server"></asp:Label></h2>
                        <p class="filter-hint"><asp:Label ID="lblMessageText" runat="server"></asp:Label></p>
                        <asp:HyperLink ID="lnkMessage" runat="server" CssClass="btn btn-wtf"></asp:HyperLink>
                    </div>

                    <div id="divSuccess" runat="server" class="account-card reservation-summary" visible="false">
                        <h2 class="filter-title">Your request was sent</h2>
                        <p class="filter-hint"><asp:Label ID="lblSuccessText" runat="server"></asp:Label></p>
                        <asp:Label ID="lblEmailNote" runat="server" CssClass="email-note d-block" Visible="false"></asp:Label>
                        <asp:Literal ID="litSummary" runat="server"></asp:Literal>
                        <asp:HyperLink ID="lnkBackToRestaurant" runat="server" CssClass="btn btn-wtf" Text="Back to the restaurant"></asp:HyperLink>
                        <a class="btn btn-outline-light" href="Search.aspx">Find another restaurant</a>
                    </div>

                    <div id="divForm" runat="server" class="account-card">
                        <h2 class="filter-title">Your reservation</h2>
                        <p class="filter-hint">Fill in every part unless it says (optional), then click Request reservation.</p>

                        <asp:Label ID="lblValidation" runat="server" CssClass="validation-summary d-block" Visible="false" EnableViewState="false" />

                        <div class="row">
                            <div class="col-md-6 account-field">
                                <asp:Label ID="lblDate" runat="server" AssociatedControlID="txtDate" CssClass="filter-label" Text="Date"></asp:Label>
                                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control mt-1" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-6 account-field">
                                <asp:Label ID="lblTime" runat="server" AssociatedControlID="txtTime" CssClass="filter-label" Text="Time"></asp:Label>
                                <asp:TextBox ID="txtTime" runat="server" CssClass="form-control mt-1" TextMode="Time"></asp:TextBox>
                            </div>
                        </div>

                        <div class="account-field">
                            <asp:Label ID="lblPartySize" runat="server" AssociatedControlID="ddlPartySize" CssClass="filter-label" Text="Party size"></asp:Label>
                            <asp:DropDownList ID="ddlPartySize" runat="server" CssClass="form-select mt-1 reservation-party"></asp:DropDownList>
                        </div>

                        <div class="account-field">
                            <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" CssClass="filter-label" Text="Name for the reservation"></asp:Label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control mt-1" MaxLength="100"></asp:TextBox>
                        </div>

                        <div class="row">
                            <div class="col-md-6 account-field">
                                <asp:Label ID="lblPhone" runat="server" AssociatedControlID="txtPhone" CssClass="filter-label" Text="Phone"></asp:Label>
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control mt-1" TextMode="Phone" MaxLength="20"></asp:TextBox>
                            </div>
                            <div class="col-md-6 account-field">
                                <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" CssClass="filter-label" Text="Email"></asp:Label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control mt-1" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>
                        <p class="filter-hint"><asp:Label ID="lblContactHint" runat="server"
                            Text="The restaurant uses these to reach you about your reservation."></asp:Label></p>

                        <div class="account-field">
                            <asp:Label ID="lblRequests" runat="server" AssociatedControlID="txtRequests" CssClass="filter-label" Text="Special requests (optional)"></asp:Label>
                            <asp:TextBox ID="txtRequests" runat="server" CssClass="form-control mt-1" TextMode="MultiLine" Rows="4"></asp:TextBox>
                            <p class="filter-hint mt-1 mb-0">Allergies, a high chair, a birthday... Up to 1000 characters.</p>
                        </div>

                        <asp:Button ID="btnReserve" runat="server" Text="Request reservation" CssClass="btn btn-wtf w-100"
                            OnClick="btnReserve_Click" />

                        <p class="account-switch"><asp:HyperLink ID="lnkCancel" runat="server" NavigateUrl="Search.aspx" Text="Cancel"></asp:HyperLink></p>
                    </div>
                </div>
            </div>
        </div>

        <asp:Literal ID="litFooter" runat="server"></asp:Literal>
    </form>
    <script src="<%= ResolveUrl("~/Scripts/bootstrap.bundle.min.js") %>"></script>
</body>
</html>
