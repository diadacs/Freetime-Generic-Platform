<%@ Control Language="C#" Inherits="Freetime.Web.View.BaseViewUserControl" %>
<%
    if (CurrentUser.IsAuthorized) {
%>
        Welcome <strong><%= Html.Encode(CurrentUser.Name) %></strong>! 
        |
        <%= Html.ActionLink("Log Off", "LogOff", "Account", null, new { @class = "topNavLinks" })%>
<%
    }
    else {
%> 
        <%= Html.ActionLink("Log On", "LogOn", "Account", null, new { @class = "topNavLinks" })%>
<%
    }
%>
