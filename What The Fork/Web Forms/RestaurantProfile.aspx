<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RestaurantProfile.aspx.cs" Inherits="What_The_Fork.Web_Forms.RestaurantProfile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en" data-bs-theme="dark">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Restaurant | What the Fork</title>
  
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
      <link href="~/Styling/theme.css" rel="stylesheet" />

</head>
<body>
    <form id="frmProfile" runat="server">
        <asp:Literal ID="litHeader" runat="server"></asp:Literal>

        <div id="divMessage" runat="server" class="container profile-message" visible="false">
            <h1>
                <asp:Label ID="lblMessageTitle" runat="server" />
            </h1>
            <p>
                <asp:Label ID="lblMessageText" runat="server" />
            </p>
            <a class="btn btn-wtf" href="Search.aspx">Back to search</a>
        </div>

        <div id="divProfile" runat="server">

            <div class="profile-cover">
                <asp:Image ID="imgCover" runat="server" CssClass="profile-cover-img" AlternateText="" EnableViewState="false" />
            </div>

            <div class="container profile-main">
                <a class="back-link" href="Search.aspx"> Back to search results</a>

                <div class="profile-header-card">
                    <asp:Image ID="imgLogo" runat="server" CssClass="profile-logo" EnableViewState="false" />
                    <div class="profile-title">
                        <h1>
                            <asp:Label ID="lblRestaurantName" runat="server" />
                        </h1>
                        <div class="profile-meta">
                            <asp:Label ID="lblCuisine" runat="server" CssClass="cuisine-badge" />
                            <asp:Label ID="lblPriceLevel" runat="server" CssClass="price-text" />
                            <asp:Label ID="lblLocation" runat="server" />
                        </div>
                    </div>
                    <div class="profile-overall">
                        <asp:Label ID="lblOverallRating" runat="server" CssClass="profile-overall-number" />
                        <div id="divOverallStars" runat="server"></div>
                        <asp:Label ID="lblReviewCount" runat="server" CssClass="profile-review-count" />
                    </div>
                  
                    <asp:HyperLink ID="lnkReserve" runat="server" CssClass="btn btn-wtf profile-reserve" Text="Reserve a Table" />
                </div>

                <div class="row g-4">


                    <div class="col-lg-8">
                        <div class="profile-card">
                            <h2>About</h2>
                            <p class="profile-description">
                                <asp:Label ID="lblDescription" runat="server" />
                            </p>
                        </div>

                        <div class="profile-card">
                            <h2>Gallery</h2>
                            <asp:Label ID="lblNoPhotos" runat="server" CssClass="text-muted-light" Visible="false"
                                Text="No photos yet." />

                            <asp:Repeater ID="rptGallery" runat="server" EnableViewState="false">
                                <HeaderTemplate>
                                    <div class="gallery-grid">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <figure class="gallery-item">
                                        <asp:Image ID="imgGallery" runat="server" CssClass="gallery-img"
                                            ImageUrl='<%# Eval("ImageUrl") %>' AlternateText='<%# Eval("Caption") %>' />
                                        <figcaption class="gallery-caption"><%# Server.HtmlEncode(Eval("Caption").ToString()) %></figcaption>
                                    </figure>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>

                        <div class="profile-card">
                            <h2>Ratings</h2>
                            <div id="divRatings" runat="server"></div>
                        </div>

                        <div class="profile-card">
                            <div class="d-flex flex-wrap justify-content-between align-items-center gap-2 mb-3">
                                <h2 class="mb-0">
                                    <asp:Label ID="lblReviewsHeading" runat="server" Text="Reviews" />
                                </h2>
                                <asp:HyperLink ID="lnkWriteReview" runat="server" CssClass="btn btn-wtf btn-sm" />
                            </div>
                            <asp:Label ID="lblNoReviews" runat="server" CssClass="text-muted-light" Visible="false"
                                Text="No one has reviewed this restaurant yet." />

                            <asp:PlaceHolder ID="phReviews" runat="server" />
                        </div>
                    </div>

                    <div class="col-lg-4">
                        <div class="profile-card">
                            <h2>Contact &amp; Location</h2>
                            <div class="contact-item">
                                <span class="contact-label">Address</span>
                                <asp:Label ID="lblAddress" runat="server" />
                            </div>
                            <div class="contact-item">
                                <span class="contact-label">Phone</span>
                                <asp:HyperLink ID="lnkPhone" runat="server" />
                            </div>
                            <div class="contact-item">
                                <span class="contact-label">Email</span>
                                <asp:HyperLink ID="lnkEmail" runat="server" />
                            </div>
                            <div class="contact-item">
                                <span class="contact-label">Website</span>
                                <asp:HyperLink ID="lnkWebsite" runat="server" Target="_blank" />
                            </div>
                            <div class="contact-item">
                                <span class="contact-label">Owner</span>
                                <asp:Label ID="lblOwner" runat="server" />
                            </div>
                            <div class="contact-item">
                                <span class="contact-label">Social Media</span>
                                <asp:GridView ID="gvSocialLinks" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                    CssClass="social-links" GridLines="None"
                                    EmptyDataText="Not listed">
                                    <Columns>
                                        <asp:HyperLinkField DataTextField="PlatformName" DataNavigateUrlFields="LinkUrl"
                                            Target="_blank" ControlStyle-CssClass="social-link" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>

                        <div class="profile-card">
                            <h2>Hours</h2>
                            <asp:GridView ID="gvHours" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                CssClass="hours-table" GridLines="None"
                                EmptyDataText="Hours have not been posted yet.">
                                <Columns>
                                    <asp:BoundField DataField="Day" />
                                    <asp:BoundField DataField="Hours" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <asp:Literal ID="litFooter" runat="server"></asp:Literal>
    </form>
</body>
</html>
