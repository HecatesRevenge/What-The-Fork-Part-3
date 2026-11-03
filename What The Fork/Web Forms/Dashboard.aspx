<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="What_The_Fork.Web_Forms.Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en" data-bs-theme="dark">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Dashboard | What the Fork</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Styling/theme.css" rel="stylesheet" />
</head>
<body>
    <form id="frmDashboard" runat="server">
        <asp:Literal ID="litHeader" runat="server"></asp:Literal>
        <div class="search-hero">
            <div class="container">
                <p class="eyebrow">
                    <asp:Label ID="lblEyebrow" runat="server" Text="Your Dashboard"></asp:Label>
                </p>
                <h1>
                    <asp:Label ID="lblWelcome" runat="server" Text="Welcome to What the Fork"></asp:Label></h1>
                <p class="lead">
                    <asp:Label ID="lblInstructions" runat="server"></asp:Label>
                </p>
            </div>
        </div>

        <div class="container search-main">
            <asp:Label ID="lblValidation" runat="server" CssClass="validation-summary d-block" Visible="false" />

          
            <div class="row g-4">
                <asp:Repeater ID="rptCards" runat="server">
                    <ItemTemplate>
                        <div class="col-md-6 col-lg-4">
                            <div class="dashboard-card">
                                <p class="eyebrow"><%# DataBinder.Eval(Container.DataItem, "Category") %></p>
                                <h2><%# DataBinder.Eval(Container.DataItem, "Title") %></h2>
                                <p class="dashboard-stat"><%# DataBinder.Eval(Container.DataItem, "Stat") %></p>
                                <p class="dashboard-text"><%# DataBinder.Eval(Container.DataItem, "Description") %></p>
                                <a class="btn btn-wtf w-100" href="<%# DataBinder.Eval(Container.DataItem, "LinkUrl") %>"><%# DataBinder.Eval(Container.DataItem, "LinkText") %></a>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
           
        <asp:Literal ID="litFooter" runat="server"></asp:Literal>

    </form>
    <script src="<%= ResolveUrl("~/Scripts/bootstrap.bundle.min.js") %>"></script>
</body>
</html>
