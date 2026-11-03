<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RestaurantReviews.aspx.cs" Inherits="What_The_Fork.Web_Forms.RestaurantReviews" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en" data-bs-theme="dark">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Respond to Reviews | What the Fork</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Styling/theme.css" rel="stylesheet" />
</head>
<body>
    <form id="frmRestaurantReviews" runat="server">
        <asp:Literal ID="litHeader" runat="server"></asp:Literal>

        <div class="search-hero">
            <div class="container">
                <p class="eyebrow">Representative dashboard</p>
                <h1>Respond to Reviews</h1>
                <p class="lead"><asp:Label ID="lblInstructions" runat="server"
                    Text="Type a response under any review and click Save response. Your response shows under the review on your restaurant's page."></asp:Label></p>
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

                    <div id="divReviews" runat="server">
                        <asp:Label ID="lblMessage" runat="server" CssClass="success-message d-block" Visible="false" EnableViewState="false" />
                        <asp:Label ID="lblValidation" runat="server" CssClass="validation-summary d-block" Visible="false" EnableViewState="false" />

                        <div class="results-toolbar d-flex flex-wrap justify-content-between align-items-center gap-2 mb-3">
                            <div>
                                <h2 class="results-count"><asp:Label ID="lblRestaurantName" runat="server"></asp:Label></h2>
                                <asp:Label ID="lblReviewCount" runat="server" CssClass="results-criteria"></asp:Label>
                            </div>
                            <div class="respond-filter">
                                <asp:Label ID="lblFilter" runat="server" AssociatedControlID="ddlFilter" CssClass="filter-label" Text="Show"></asp:Label>
                                <asp:DropDownList ID="ddlFilter" runat="server" CssClass="form-select form-select-sm" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged">
                                    <asp:ListItem Value="Unanswered" Text="Needs a response" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="All" Text="All reviews"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <asp:Label ID="lblNoReviews" runat="server" CssClass="account-card d-block text-center filter-hint" Visible="false"></asp:Label>

                        <asp:Repeater ID="rptReviews" runat="server" EnableViewState="false" OnItemCommand="rptReviews_ItemCommand">
                            <ItemTemplate>
                                <div class="review-card respond-card">
                                    <div class="review-header">
                                        <div>
                                            <span class="review-name"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "ReviewerName").ToString()) %></span>
                                            <div class="review-date"><%# DataBinder.Eval(Container.DataItem, "VisitText") %></div>
                                        </div>
                                    </div>

                                    <h3 class="review-title"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "ReviewTitle").ToString()) %></h3>
                                    <p class="review-text"><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "ReviewText").ToString()) %></p>

                                    <asp:Image ID="imgReviewPhoto" runat="server" CssClass="review-card-photo"
                                        ImageUrl='<%# DataBinder.Eval(Container.DataItem, "PhotoUrl") %>'
                                        Visible='<%# DataBinder.Eval(Container.DataItem, "HasPhoto") %>'
                                        AlternateText="Photo from the reviewer" />

                                    <div class="review-scores">
                                        Food <span class="review-score"><%# DataBinder.Eval(Container.DataItem, "FoodRating") %></span>
                                        | Service <span class="review-score"><%# DataBinder.Eval(Container.DataItem, "ServiceRating") %></span>
                                        | Atmosphere <span class="review-score"><%# DataBinder.Eval(Container.DataItem, "AtmosphereText") %></span>
                                    </div>

                                    <div class="respond-box">
                                        <asp:Label ID="lblResponse" runat="server" AssociatedControlID="txtResponse" CssClass="filter-label" Text="Your response"></asp:Label>
                                        <span class="review-response-date"><%# DataBinder.Eval(Container.DataItem, "ResponseDateText") %></span>
                                        <asp:TextBox ID="txtResponse" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control mt-1"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "ResponseText") %>'></asp:TextBox>
                                        <asp:Button ID="btnSaveResponse" runat="server" Text="Save response" CssClass="btn btn-wtf btn-sm mt-2"
                                            CommandName="SaveResponse" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ReviewID") %>' />
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                </div>
            </div>
        </div>

        <asp:Literal ID="litFooter" runat="server"></asp:Literal>
    </form>
    <script src="<%= ResolveUrl("~/Scripts/bootstrap.bundle.min.js") %>"></script>
</body>
</html>
