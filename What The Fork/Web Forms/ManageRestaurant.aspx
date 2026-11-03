<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageRestaurant.aspx.cs" Inherits="What_The_Fork.Web_Forms.ManageRestaurant" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en" data-bs-theme="dark">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>My Restaurant | What the Fork</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Styling/theme.css" rel="stylesheet" />
</head>
<body>
    <form id="frmManageRestaurant" runat="server">
        <asp:Literal ID="litHeader" runat="server"></asp:Literal>

        <div class="search-hero">
            <div class="container">
                <p class="eyebrow"><asp:Label ID="lblEyebrow" runat="server" Text="Representative dashboard"></asp:Label></p>
                <h1><asp:Label ID="lblPageTitle" runat="server" Text="Add Your Restaurant"></asp:Label></h1>
                <p class="lead"><asp:Label ID="lblInstructions" runat="server"
                    Text="Fill in your restaurant's details, location, hours and social media links, then click Save at the bottom. You can add photos after it's saved."></asp:Label></p>
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

                    <div id="divForm" runat="server">

                        <asp:Label ID="lblMessage" runat="server" CssClass="success-message d-block" Visible="false" />
                        <asp:Label ID="lblValidation" runat="server" CssClass="validation-summary d-block" Visible="false" />
                        <p id="pViewProfile" runat="server" class="text-end mb-3" visible="false">
                            <asp:HyperLink ID="lnkViewProfile" runat="server" CssClass="btn btn-outline-light btn-sm" Text="View my restaurant's page"></asp:HyperLink>
                        </p>

                        <div class="account-card manage-card">

                            <h2 class="filter-title">Restaurant details</h2>
                            <p class="filter-hint">Fill in every box unless it says (optional).</p>

                            <div class="account-field">

                                <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" CssClass="filter-label" Text="Restaurant name"></asp:Label>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control mt-1" MaxLength="100"></asp:TextBox>

                            </div>

                            <div class="row">

                                <div class="col-md-6 account-field">
                                    <asp:Label ID="lblCuisine" runat="server" AssociatedControlID="ddlCuisine" CssClass="filter-label" Text="Cuisine"></asp:Label>
                                    <asp:DropDownList ID="ddlCuisine" runat="server" CssClass="form-select mt-1"></asp:DropDownList>
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

                            <div class="account-field">

                                <asp:Label ID="lblDescription" runat="server" AssociatedControlID="txtDescription" CssClass="filter-label" Text="Description"></asp:Label>
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control mt-1" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                <p class="filter-hint mt-1 mb-0">Tell diners what makes your restaurant special.</p>

                            </div>

                            <div class="account-field mb-0">

                                <asp:Label ID="lblOwner" runat="server" AssociatedControlID="txtOwner" CssClass="filter-label" Text="Owner or chef (optional)"></asp:Label>
                                <asp:TextBox ID="txtOwner" runat="server" CssClass="form-control mt-1" MaxLength="100"></asp:TextBox>

                            </div>
                        </div>

                        <div class="account-card manage-card">

                            <h2 class="filter-title">Location and contact</h2>

                            <div class="account-field">
                                <asp:Label ID="lblStreet" runat="server" AssociatedControlID="txtStreet" CssClass="filter-label" Text="Street address"></asp:Label>
                                <asp:TextBox ID="txtStreet" runat="server" CssClass="form-control mt-1" MaxLength="150"></asp:TextBox>
                            </div>

                            <div class="row">

                                <div class="col-md-5 account-field">
                                    <asp:Label ID="lblCity" runat="server" AssociatedControlID="txtCity" CssClass="filter-label" Text="City"></asp:Label>
                                    <asp:TextBox ID="txtCity" runat="server" CssClass="form-control mt-1" MaxLength="60"></asp:TextBox>
                                </div>

                                <div class="col-md-4 account-field">
                                    <asp:Label ID="lblState" runat="server" AssociatedControlID="ddlState" CssClass="filter-label" Text="State"></asp:Label>
                                    <asp:DropDownList ID="ddlState" runat="server" CssClass="form-select mt-1"></asp:DropDownList>
                                </div>

                                <div class="col-md-3 account-field">
                                    <asp:Label ID="lblZip" runat="server" AssociatedControlID="txtZip" CssClass="filter-label" Text="ZIP code"></asp:Label>
                                    <asp:TextBox ID="txtZip" runat="server" CssClass="form-control mt-1" MaxLength="10"></asp:TextBox>
                                </div>

                            </div>

                            <div class="row">

                                <div class="col-md-6 account-field">
                                    <asp:Label ID="lblPhone" runat="server" AssociatedControlID="txtPhone" CssClass="filter-label" Text="Phone"></asp:Label>
                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control mt-1" MaxLength="20" placeholder="(215) 555-0123"></asp:TextBox>
                                </div>

                                <div class="col-md-6 account-field">
                                    <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" CssClass="filter-label" Text="Email (optional)"></asp:Label>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control mt-1" MaxLength="100"></asp:TextBox>
                                    <p class="filter-hint mt-1 mb-0">Shown on your restaurant's page for diners who want to get in touch.</p>
                                </div>

                            </div>

                            <div class="account-field mb-0">
                                <asp:Label ID="lblWebsite" runat="server" AssociatedControlID="txtWebsite" CssClass="filter-label" Text="Website (optional)"></asp:Label>
                                <asp:TextBox ID="txtWebsite" runat="server" CssClass="form-control mt-1" MaxLength="200" placeholder="https://"></asp:TextBox>
                            </div>

                        </div>

                        <div class="account-card manage-card">
                            <h2 class="filter-title">Hours</h2>
                            <p class="filter-hint">Enter the opening and closing time for each day, or check Closed.</p>

                            <div class="table-responsive">
                                <asp:GridView ID="gvHours" runat="server" AutoGenerateColumns="False"
                                    CssClass="table align-middle results-grid manage-grid" GridLines="None"
                                    DataKeyNames="DayOfWeek">
                                    <Columns>
                                        <asp:BoundField DataField="Day" HeaderText="Day" ItemStyle-CssClass="restaurant-name" />
                                        <asp:TemplateField HeaderText="Opens">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtOpen" runat="server" CssClass="form-control form-control-sm" TextMode="Time"
                                                    Text='<%# Eval("OpenTime") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Closes">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtClose" runat="server" CssClass="form-control form-control-sm" TextMode="Time"
                                                    Text='<%# Eval("CloseTime") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Closed">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkClosed" runat="server" CssClass="account-type"
                                                    Checked='<%# Convert.ToBoolean(Eval("IsClosed")) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="grid-header" />
                                    <RowStyle CssClass="grid-row" />
                                    <AlternatingRowStyle CssClass="grid-row grid-alt" />
                                </asp:GridView>
                            </div>
                        </div>

                        <div class="account-card manage-card">
                            <h2 class="filter-title">Social media (optional)</h2>
                            <p class="filter-hint">Paste the full link to each page you have.</p>

                            <div class="table-responsive">
                                <asp:GridView ID="gvSocialLinks" runat="server" AutoGenerateColumns="False"
                                    CssClass="table align-middle results-grid manage-grid" GridLines="None"
                                    DataKeyNames="PlatformID,PlatformName">
                                    <Columns>
                                        <asp:BoundField DataField="PlatformName" HeaderText="Site" ItemStyle-CssClass="restaurant-name" />
                                        <asp:TemplateField HeaderText="Link">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLink" runat="server" CssClass="form-control form-control-sm" MaxLength="200"
                                                    Text='<%# Eval("LinkUrl") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="grid-header" />
                                    <RowStyle CssClass="grid-row" />
                                    <AlternatingRowStyle CssClass="grid-row grid-alt" />
                                </asp:GridView>
                            </div>
                        </div>

                        <asp:Button ID="btnSave" runat="server" Text="Add my restaurant" CssClass="btn btn-wtf w-100"
                            OnClick="btnSave_Click" />
                        <p class="account-switch"><asp:HyperLink ID="lnkCancel" runat="server" NavigateUrl="Dashboard.aspx" Text="Cancel"></asp:HyperLink></p>

                        <div class="account-card manage-card mt-4">
                            <h2 class="filter-title">Photos</h2>

                            <div id="divPhotosLater" runat="server">
                                <p class="filter-hint mb-0">Save your restaurant first. Then you can upload a logo, a profile photo and gallery photos here.</p>
                            </div>

                            <div id="divPhotos" runat="server" visible="false">
                                <p class="filter-hint">Upload one photo at a time. The logo and the profile photo replace the ones you have now, and
                                    gallery photos are added to your gallery. Photos have to be .jpg, .gif or .png files up to 1 MB.</p>

                                <asp:Label ID="lblPhotoMessage" runat="server" CssClass="success-message d-block" Visible="false" />
                                <asp:Label ID="lblPhotoValidation" runat="server" CssClass="validation-summary d-block" Visible="false" />

                                <div class="row">
                                    <div class="col-md-6 account-field">
                                        <asp:Label ID="lblPhotoFile" runat="server" AssociatedControlID="fuPhoto" CssClass="filter-label" Text="Photo"></asp:Label>
                                        <asp:FileUpload ID="fuPhoto" runat="server" CssClass="form-control mt-1" accept=".jpg,.jpeg,.gif,.png" />
                                    </div>
                                    <div class="col-md-6 account-field">
                                        <asp:Label ID="lblPhotoType" runat="server" AssociatedControlID="ddlPhotoType" CssClass="filter-label" Text="Use it as"></asp:Label>
                                        <asp:DropDownList ID="ddlPhotoType" runat="server" CssClass="form-select mt-1">
                                            <asp:ListItem Value="Gallery" Text="A gallery photo"></asp:ListItem>
                                            <asp:ListItem Value="Logo" Text="My logo"></asp:ListItem>
                                            <asp:ListItem Value="Profile" Text="My profile (cover) photo"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="account-field">
                                    <asp:Label ID="lblCaption" runat="server" AssociatedControlID="txtCaption" CssClass="filter-label" Text="Caption"></asp:Label>
                                    <asp:TextBox ID="txtCaption" runat="server" CssClass="form-control mt-1" MaxLength="200"></asp:TextBox>
                                    <p class="filter-hint mt-1 mb-0">Say what the photo shows.</p>
                                </div>

                                <asp:Button ID="btnUpload" runat="server" Text="Upload photo" CssClass="btn btn-wtf"
                                    OnClick="btnUpload_Click" />

                                <div id="divConfirmDelete" runat="server" class="account-card confirm-card mt-3" visible="false">
                                    <p><asp:Label ID="lblConfirmDelete" runat="server"></asp:Label></p>
                                    <asp:Button ID="btnConfirmDelete" runat="server" Text="Yes, delete it" CssClass="btn btn-wtf"
                                        OnClick="btnConfirmDelete_Click" />
                                    <asp:Button ID="btnCancelDelete" runat="server" Text="No, keep it" CssClass="btn btn-outline-light"
                                        OnClick="btnCancelDelete_Click" />
                                </div>

                     
                                <div class="table-responsive mt-3">
                                    <asp:GridView ID="gvPhotos" runat="server" AutoGenerateColumns="False" EnableViewState="false"
                                        CssClass="table align-middle results-grid manage-grid" GridLines="None"
                                        DataKeyNames="PhotoID,PhotoType" OnRowCommand="gvPhotos_RowCommand"
                                        EmptyDataText="You haven't uploaded any photos yet.">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Photo">
                                                <ItemTemplate>
                                                    <asp:Image ID="imgPhoto" runat="server" CssClass="manage-photo"
                                                        ImageUrl='<%# Eval("ImageUrl") %>' AlternateText='<%# Eval("Caption") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PhotoType" HeaderText="Used as" />
                                            <asp:BoundField DataField="Caption" HeaderText="Caption" NullDisplayText="-" ItemStyle-CssClass="muted-cell" />
                                            <asp:ButtonField ButtonType="Button" Text="Delete" CommandName="DeletePhoto"
                                                ControlStyle-CssClass="btn-delete" />
                                        </Columns>
                                        <HeaderStyle CssClass="grid-header" />
                                        <RowStyle CssClass="grid-row" />
                                        <AlternatingRowStyle CssClass="grid-row grid-alt" />
                                        <EmptyDataRowStyle CssClass="grid-empty" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:Literal ID="litFooter" runat="server"></asp:Literal>
    </form>
    <script src="<%= ResolveUrl("~/Scripts/bootstrap.bundle.min.js") %>"></script>
</body>
</html>
