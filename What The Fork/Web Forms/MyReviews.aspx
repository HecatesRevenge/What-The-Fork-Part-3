<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyReviews.aspx.cs" Inherits="What_The_Fork.Web_Forms.MyReviews" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en" data-bs-theme="dark">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>My Reviews | What the Fork</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Styling/theme.css" rel="stylesheet" />
</head>

<body>
    <form id="frmMyReviews" runat="server">
        <asp:Literal ID="litHeader" runat="server"></asp:Literal>

        <div class="search-hero">
            <div class="container">
                <p class="eyebrow">Reviewer dashboard</p>
                <h1>My Reviews</h1>
                <p class="lead">
                    <asp:Label ID="lblInstructions" runat="server"
                        Text="Every review you've written. Click Edit to change one, or Delete to remove it. To write a new review, find the restaurant and click Write a Review on its page."></asp:Label>
                </p>
            </div>
        </div>

        <div class="container search-main">
            <asp:Label ID="lblMessage" runat="server" CssClass="success-message d-block" Visible="false" />
            <asp:Label ID="lblValidation" runat="server" CssClass="validation-summary d-block" Visible="false" />


            <div id="divConfirmDelete" runat="server" class="account-card confirm-card" visible="false">
                <p>
                    <asp:Label ID="lblConfirmDelete" runat="server"></asp:Label></p>
                <asp:Button ID="btnConfirmDelete" runat="server" Text="Yes, delete it" CssClass="btn btn-wtf"
                    OnClick="btnConfirmDelete_Click" />
                <asp:Button ID="btnCancelDelete" runat="server" Text="No, keep it" CssClass="btn btn-outline-light"
                    OnClick="btnCancelDelete_Click" />
            </div>

            <div class="results-toolbar d-flex flex-wrap justify-content-between align-items-center gap-2">
                <h2 class="results-count">
                    <asp:Label ID="lblReviewCount" runat="server" Text="Your reviews"></asp:Label></h2>
                <a class="btn btn-wtf btn-sm" href="Search.aspx">Find a restaurant to review</a>
            </div>

            <div class="table-responsive">
                <asp:GridView ID="gvMyReviews" runat="server" AutoGenerateColumns="False"
                    CssClass="table align-middle results-grid" GridLines="None"
                    DataKeyNames="ReviewID,RestaurantID,RestaurantName" AllowPaging="True" PageSize="10"
                    OnPageIndexChanging="gvMyReviews_PageIndexChanging"
                    OnRowCommand="gvMyReviews_RowCommand"
                    EmptyDataText="You haven't written any reviews yet. Click Find a restaurant to review to get started.">
                    <Columns>
                        <asp:BoundField DataField="RestaurantName" HeaderText="Restaurant" ItemStyle-CssClass="restaurant-name" />
                        <asp:BoundField DataField="ReviewTitle" HeaderText="Title" />
                        <asp:BoundField DataField="VisitDate" HeaderText="Visited" DataFormatString="{0:MMM d, yyyy}" NullDisplayText="-"
                            ItemStyle-CssClass="muted-cell" />

                        <asp:BoundField DataField="FoodRating" HeaderText="Food"
                            ItemStyle-CssClass="score" HeaderStyle-CssClass="text-center" />
                        <asp:BoundField DataField="ServiceRating" HeaderText="Service"
                            ItemStyle-CssClass="score" HeaderStyle-CssClass="text-center" />
                        <asp:BoundField DataField="AtmosphereRating" HeaderText="Atmosphere" NullDisplayText="-"
                            ItemStyle-CssClass="score" HeaderStyle-CssClass="text-center" />
                        <asp:BoundField DataField="PriceLevel" HeaderText="Price"
                            ItemStyle-CssClass="price-text text-center" HeaderStyle-CssClass="text-center" />

                        <asp:BoundField DataField="ReviewDate" HeaderText="Posted" DataFormatString="{0:MMM d, yyyy}"
                            ItemStyle-CssClass="muted-cell" />

                        <asp:ButtonField ButtonType="Button" Text="View" CommandName="ViewRestaurant"
                            ControlStyle-CssClass="btn-view" />
                        <asp:ButtonField ButtonType="Button" Text="Edit" CommandName="EditReview"
                            ControlStyle-CssClass="btn-view" />
                        <asp:ButtonField ButtonType="Button" Text="Delete" CommandName="DeleteReview"
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

        <asp:Literal ID="litFooter" runat="server"></asp:Literal>
    </form>
    <script src="<%= ResolveUrl("~/Scripts/bootstrap.bundle.min.js") %>"></script>
</body>
</html>
