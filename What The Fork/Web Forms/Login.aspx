<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="What_The_Fork.Web_Forms.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en" data-bs-theme="dark" >
<head runat="server">
    <title>Log In | What the Fork</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Styling/theme.css" rel="stylesheet" />
</head>
<body>
    <form id="frmLogin" runat="server">
        <asp:Literal ID="litHeader" runat="server" Text="Log In"></asp:Literal>

        <div class="search-hero">
            <p class="eyebrow">Welcome back</p>
            <h1>Log into What the Fork</h1>
            <p class="lead">
                Reviewers can write and manage reviews.
                Resturants representives can mange their resturarant.
            </p>
        </div>

        <div class="container search-main">
            <div class="row justify-content-center">
                <div class="col-md-8 col-lg-5">
                    <div class="account-card">
                        <h2 class="filter-title">Log In</h2>
                        <p class="filter-hint">Enter the username and password. Passwords are case senstive</p>

                        <asp:Label ID="lblMessage" runat="server" CssClass="success-message d-block" Visible="false"></asp:Label>
                        <asp:Label ID="lblValidation" runat="server" CssClass="validation-summary d-block" Visible="false"></asp:Label>

                        <div class="account-field">
                            <asp:Label ID="lblUsername" runat="server" AssociatedControlID="txtUsername" CssClass="filter-label" Text="Username"></asp:Label>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control mt-1" MaxLength="50"></asp:TextBox>
                        </div>

                        <div class="account-field">
                            <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" CssClass="filter-label" Text="Password"></asp:Label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control mt-1" TextMode="Password" MaxLength="50"></asp:TextBox>
                        </div>

                        <asp:Button ID="btnLogin" runat="server" Text="Log In" CssClass="btn btn-wtf w-100 mt-3"
                            OnClick="btnLogin_Click" />

                        <p class="account-switch">Don't have an account? <a href="Register.aspx">Register Here</a></p>
                    </div>
                </div>
            </div>
        </div>
        <asp:Literal ID="litFooter" runat="server" Text="Footer content"></asp:Literal>
    </form>
    <script src="<%= ResolveUrl("~/Scripts/bootstrap.bundle.min.js") %>"></script>
</body>
</html>
