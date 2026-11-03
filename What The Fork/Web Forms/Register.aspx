<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="What_The_Fork.Web_Forms.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en" data-bs-theme="dark">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Register | What the Fork</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Styling/theme.css" rel="stylesheet" />
</head>
<body>
    <form id="frmRegister" runat="server">
        <asp:Literal ID="litHeader" runat="server" Text="Register"></asp:Literal>

        <div class="search-hero">
            <div class="container">
                <p class="eyebrow">Join What the Fork</p>
                <h1>Create an Account</h1>
                <p class="lead">
                    Register to write and manage reviews, or to manage your restaurant's profile.
                </p>
            </div>
        </div>

        <div class="container search-main">
            <div class="row justify-content-center">
                <div class="col-lg-8 col-xl-7">
                    <div class="account-card">
                        <h2 class="filter-title">Register</h2>
                        <p class="filter-hint">Fill in every required field. Then click Create Account.</p>

                        <asp:Label ID="lblValidation" runat="server" CssClass="validation-summary d-block" Visible="false" />


                        <div class="account-field">
                            <asp:Label ID="lblAccountType" runat="server" AssociatedControlID="rblAccountType" CssClass="filter-label" Text="Account Type"></asp:Label>
                            <p class="filter-hint mb-1">Choose Reviewer to write reviews, or Representative if you manage restaurant.</p>

                            <asp:RadioButtonList ID="rblAccountType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="account-type" AutoPostBack="true" OnSelectedIndexChanged="rblAccountType_SelectedIndexChanged">
                                <asp:ListItem Value="Reviewer" Text="Reviewer"></asp:ListItem>
                                <asp:ListItem Value="Representative" Text="Representative"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>

                        <div class="row">
                            <div class="col-md-6 account-field">
                                <asp:Label ID="lblFirstName" runat="server" AssociatedControlID="txtFirstName" CssClass="filter-label" Text="First Name"></asp:Label>
                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control mt-1" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="col-md-6 account-field">
                                <asp:Label ID="lblLastName" runat="server" AssociatedControlID="txtLastName" CssClass="filter-label" Text="Last Name"></asp:Label>
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control mt-1" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 account-field">
                                <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" CssClass="filter-label" Text="Email"></asp:Label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control mt-1" MaxLength="100"></asp:TextBox>
                            </div>
                            <div class="col-md-6 account-field">
                                <asp:Label ID="lblPhone" runat="server" AssociatedControlID="txtPhone" CssClass="filter-label" Text="Phone (optional)"></asp:Label>
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control mt-1" MaxLength="20"></asp:TextBox>
                            </div>
                        </div>

                        <div id="divReviewerInfo" runat="server" class="row" visible="false">
                            <div class="col-md-8 account-field">
                                <asp:Label ID="lblHomeCity" runat="server" AssociatedControlID="txtHomeCity" CssClass="filter-label" Text="Home City"></asp:Label>
                                <asp:TextBox ID="txtHomeCity" runat="server" CssClass="form-control mt-1" MaxLength="60"></asp:TextBox>
                            </div>
                            <div class="col-md-4 account-field">
                                <asp:Label ID="lblHomeState" runat="server" AssociatedControlID="txtHomeState" CssClass="filter-label" Text="Home State"></asp:Label>
                                <asp:TextBox ID="txtHomeState" runat="server" CssClass="form-control mt-1" MaxLength="2"></asp:TextBox>
                            </div>
                            <p class="filter-hint">Your home city and state are shown next your reviews.</p>
                        </div>

                        <div class="account-field">
                            <asp:Label ID="lblUsername" runat="server"
                                AssociatedControlID="txtUsername" CssClass="filter-label" Text="Username"></asp:Label>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control mt-1" MaxLength="50"></asp:TextBox>
                            <p class="filter-hint mt-1 mb-0">Up to 50 characters. Letters and numbers only, no spaces.</p>
                        </div>

                        <div class="row">
                            <div class="col-md-6 account-field">
                                <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" CssClass="filter-label" Text="Password"></asp:Label>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control mt-1" TextMode="Password" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="col-md-6 account-field">
                                <asp:Label ID="lblConfirmPassword" runat="server" AssociatedControlID="txtConfirmPassword" CssClass="filter-label" Text="Confirm Password"></asp:Label>
                                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control mt-1" TextMode="Password" MaxLength="50"></asp:TextBox>
                            </div>
                            <p class="filter-hint">At least 6 characters. Must match in both boxes/ </p>
                        </div>

                        <asp:Button ID="btnRegister" runat="server" Text="Create Account" CssClass="btn btn-wtf w-100" OnClick="btnRegister_Click" />

                        <p class="account-switch">Already have an account? <a href="Login.aspx">Log in here</a></p>
                    </div>
                </div>
            </div>

        </div>
        <asp:Literal ID="litFooter" runat="server"></asp:Literal>
    </form>
</body>
</html>
