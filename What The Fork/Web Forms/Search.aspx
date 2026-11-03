<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="What_The_Fork.Web_Forms.Search" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en" data-bs-theme="dark">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Find Restaurants | What the Fork</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Styling/theme.css" rel="stylesheet" />
</head>
<body>
    <form id="search" runat="server">
        <asp:Literal ID="litHeader" runat="server"></asp:Literal>

        <div class="search-hero">
            <div class="container">
                <p class="eyebrow">Restaurant reviews from real diners</p>
                <h1>Find your next favorite plate</h1>
                <p class="lead">Choose one or more cuisines and a location, then compare ratings for food, service and price.</p>
            </div>
        </div>

        <div class="container search-main">
            <div class="row g-4">

                <div class="col-lg-4 col-xl-3">
                    <div class="filter-card">
                        <h2 class="filter-title">Refine Search</h2>
                        <asp:Label ID="lblValidation" runat="server" CssClass="validation-summary d-block" Visible="false" />

                        <div class="filter-section">
                            <div class="d-flex justify-content-between align-items-center mb-2">
                                <span class="filter-label">Cuisines</span>
                                <div>
                                    <asp:Button ID="btnSelectAll" runat="server" Text="All" CssClass="btn btn-outline-light btn-sm"
                                        OnClick="btnSelectAll_Click" />
                                    <asp:Button ID="btnClearAll" runat="server" Text="Clear" CssClass="btn btn-outline-light btn-sm"
                                        OnClick="btnClearAll_Click" />
                                </div>
                            </div>
                            <p class="filter-hint">Choose one or more cuisines.</p>
                            <asp:CheckBoxList ID="cblCuisines" runat="server" RepeatLayout="UnorderedList"
                                CssClass="cuisine-chips" />
                        </div>

                        <div class="filter-section">
                            <asp:Label ID="lblLocation" runat="server" AssociatedControlID="ddlLocation" CssClass="filter-label" Text="Location"></asp:Label>
                            <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-select mt-2" />
                            <p class="filter-hint mt-2 mb-0">Choose a city, or select "All Locations".</p>
                        </div>

                        <asp:Button ID="btnSearch" runat="server" Text="Search restaurants" CssClass="btn btn-wtf w-100"
                            OnClick="btnSearch_Click" />
                    </div>
                </div>


                <div class="col-lg-8 col-xl-9">
                    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false" />

                    <div class="results-toolbar">
                        <h2 class="results-count">
                            <asp:Label ID="lblResultCount" runat="server" Text="Where are we eating?" />
                        </h2>
                        <p class="results-criteria">
                            <asp:Label ID="lblSearchCriteria" runat="server"
                                Text="Pick the cuisines you're craving and a location, then click Search restaurants." />
                        </p>
                    </div>

                    <div class="table-responsive">
                        <asp:GridView ID="gvResults" runat="server" AutoGenerateColumns="False" Visible="false" EnableViewState="false"
                            CssClass="table align-middle results-grid" GridLines="None"
                            DataKeyNames="RestaurantID" AllowPaging="True" PageSize="10"
                            OnPageIndexChanging="gvResults_PageIndexChanging"
                            OnRowCommand="gvResults_RowCommand"
                            EmptyDataText="No restaurants match your search. Try adding more cuisines or choosing All Locations.">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Image ID="imgLogo" runat="server" CssClass="grid-logo"
                                            ImageUrl='<%# Eval("LogoUrl") %>' AlternateText='<%# Eval("RestaurantName") + " logo" %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="RestaurantName" HeaderText="Restaurant" ItemStyle-CssClass="restaurant-name" />

                                <asp:BoundField DataField="CuisineName" HeaderText="Cuisine" ItemStyle-CssClass="cuisine-cell" />

                                <asp:BoundField DataField="Location" HeaderText="Location" ItemStyle-CssClass="muted-cell" />

                                <asp:BoundField DataField="AtmosphereRating" HeaderText="Atmosphere" DataFormatString="{0:0.0}" NullDisplayText="-"
                                    ItemStyle-CssClass="score" HeaderStyle-CssClass="text-center" />

                                <asp:BoundField DataField="PriceLevel" HeaderText="Cost"
                                    ItemStyle-CssClass="price-text" HeaderStyle-CssClass="text-center" />

                                <asp:BoundField DataField="OverallRating" HeaderText="Overall" DataFormatString="{0:0.0}" NullDisplayText="New"
                                    ItemStyle-CssClass="score overall-score" HeaderStyle-CssClass="text-center" />

                                <asp:BoundField DataField="FoodRating" HeaderText="Food" DataFormatString="{0:0.0}" NullDisplayText="-"
                                    ItemStyle-CssClass="score" HeaderStyle-CssClass="text-center" />
                                <asp:BoundField DataField="ServiceRating" HeaderText="Service" DataFormatString="{0:0.0}" NullDisplayText="-"
                                    ItemStyle-CssClass="score" HeaderStyle-CssClass="text-center" />

                                <asp:BoundField DataField="PriceRating" HeaderText="Price" DataFormatString="{0:0.0}" NullDisplayText="-"
                                    ItemStyle-CssClass="score" HeaderStyle-CssClass="text-center" />

                                <asp:BoundField DataField="ReviewCount" HeaderText="Reviews"
                                    ItemStyle-CssClass="score muted-cell" HeaderStyle-CssClass="text-center" />

                                <asp:ButtonField ButtonType="Button" Text="View" CommandName="ViewDetails"
                                    ControlStyle-CssClass="btn-view" ItemStyle-CssClass="text-end" />
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
        </div>

        <asp:Literal ID="litFooter" runat="server"></asp:Literal>
    </form>
    <script src="<%= ResolveUrl("~/Scripts/bootstrap.bundle.min.js") %>"></script>
</body>
</html>
