<%@ Page Language="C#" Inherits="Freetime.Web.View.BaseViewPage" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
Freetime Login
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <strong>You have been logged out. Login to Freetime</strong>
    <p>
        Please enter your username and password. <%= Html.ActionLink("Register", "Register") %> if you don't have an account.
    </p>
    <div id="divLoginControl">
    <% Html.RenderPartial("LoginControl"); %>    
    </div>
</asp:Content>
