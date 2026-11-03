<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WriteReview.aspx.cs" Inherits="What_The_Fork.Web_Forms.WriteReview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en" data-bs-theme="dark">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Write a Review | What the Fork</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Styling/theme.css" rel="stylesheet" />
</head>
    <body>
    <form id="frmWriteReview" runat="server">
        <asp:Literal ID="litHeader" runat="server"></asp:Literal>

        <div class="search-hero">
            <div class="container">
                <p class="eyebrow"><asp:Label ID="lblEyebrow" runat="server" Text="Write a review"></asp:Label></p>
                <h1><asp:Label ID="lblRestaurantName" runat="server" Text="Share your visit"></asp:Label></h1>
                <p class="lead">Rate the food, service and atmosphere from 1 to 5, choose the price level, and tell other diners what it was like.</p>
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

                    <div id="divForm" runat="server" class="account-card">
                        <h2 class="filter-title"><asp:Label ID="lblFormTitle" runat="server" Text="Your review"></asp:Label></h2>
                        <p class="filter-hint">Fill in every part unless it says (optional), then click the button at the bottom.</p>

                        <asp:Label ID="lblValidation" runat="server" CssClass="validation-summary d-block" Visible="false" />

                        <div class="row">
                            <div class="col-md-6 account-field">
                                <asp:Label ID="lblFood" runat="server" AssociatedControlID="rblFood" CssClass="filter-label" Text="Food"></asp:Label>
                                <asp:RadioButtonList ID="rblFood" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="rating-choice">
                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class="col-md-6 account-field">
                                <asp:Label ID="lblService" runat="server" AssociatedControlID="rblService" CssClass="filter-label" Text="Service"></asp:Label>
                                <asp:RadioButtonList ID="rblService" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="rating-choice">
                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 account-field">
                                <asp:Label ID="lblAtmosphere" runat="server" AssociatedControlID="rblAtmosphere" CssClass="filter-label" Text="Atmosphere"></asp:Label>
                                <asp:RadioButtonList ID="rblAtmosphere" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="rating-choice">
                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class="col-md-6 account-field">
                                <asp:Label ID="lblPrice" runat="server" AssociatedControlID="ddlPrice" CssClass="filter-label" Text="Price level"></asp:Label>
                                <asp:DropDownList ID="ddlPrice" runat="server" CssClass="form-select mt-1">
                                    <asp:ListItem Value="0" Text="Choose a price level"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="$ (inexpensive)"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="$$"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="$$$"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="$$$$"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="$$$$$ (very expensive)"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <p class="filter-hint">For food, service and atmosphere, 1 is poor and 5 is excellent. The price level isn't part of the overall score.</p>

                        <div class="row">
                            <div class="col-md-6 account-field">
                                <asp:Label ID="lblVisitDate" runat="server" AssociatedControlID="txtVisitDate" CssClass="filter-label" Text="Date of your visit"></asp:Label>
                                <asp:TextBox ID="txtVisitDate" runat="server" CssClass="form-control mt-1" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-6 account-field">
                                <asp:Label ID="lblVisitTime" runat="server" AssociatedControlID="txtVisitTime" CssClass="filter-label" Text="Time of your visit"></asp:Label>
                                <asp:TextBox ID="txtVisitTime" runat="server" CssClass="form-control mt-1" TextMode="Time"></asp:TextBox>
                            </div>
                        </div>

                        <div class="account-field">
                            <asp:Label ID="lblTitle" runat="server" AssociatedControlID="txtTitle" CssClass="filter-label" Text="Title"></asp:Label>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control mt-1" MaxLength="100"></asp:TextBox>
                            <p class="filter-hint mt-1 mb-0">Sum up your visit in a few words, up to 100 characters.</p>
                        </div>

                        <div class="account-field">
                            <asp:Label ID="lblText" runat="server" AssociatedControlID="txtText" CssClass="filter-label" Text="Your review"></asp:Label>
                            <asp:TextBox ID="txtText" runat="server" CssClass="form-control mt-1" TextMode="MultiLine" Rows="7"></asp:TextBox>
                            <p class="filter-hint mt-1 mb-0">What did you order, and how was it? Up to 4000 characters.</p>
                        </div>

                      
                        <div class="account-field">
                            <asp:Label ID="lblPhoto" runat="server" AssociatedControlID="fuPhoto" CssClass="filter-label" Text="Photo (optional)"></asp:Label>
                            <div id="divCurrentPhoto" runat="server" class="review-photo-current" visible="false">
                                <asp:Image ID="imgCurrentPhoto" runat="server" CssClass="review-photo" AlternateText="Your current photo" />
                                <asp:CheckBox ID="chkRemovePhoto" runat="server" Text="Remove this photo" CssClass="account-type" />
                            </div>
                            <asp:FileUpload ID="fuPhoto" runat="server" CssClass="form-control mt-1" accept=".jpg,.jpeg,.gif,.png" />
                            <p class="filter-hint mt-1 mb-0">A .jpg, .gif or .png file up to 1 MB. Choosing a new photo replaces the current one.</p>
                        </div>

                        <asp:Button ID="btnSave" runat="server" Text="Post review" CssClass="btn btn-wtf w-100"
                            OnClick="btnSave_Click" />

                        <p class="account-switch"><asp:HyperLink ID="lnkCancel" runat="server" NavigateUrl="MyReviews.aspx" Text="Cancel"></asp:HyperLink></p>
                    </div>
                </div>
            </div>
        </div>

        <asp:Literal ID="litFooter" runat="server"></asp:Literal>
    </form>
    <script src="<%= ResolveUrl("~/Scripts/bootstrap.bundle.min.js") %>"></script>
</body>
</html>
