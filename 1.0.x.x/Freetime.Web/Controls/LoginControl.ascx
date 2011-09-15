<%@ Control Language="C#" Inherits="Freetime.Web.View.BaseViewUserControl" %>

<% using (Html.BeginForm()) { %>
        <div id="divlc">          

            <div class="lclabel">
                <label for="username">Username:</label>
            </div>
            <div class="lccontrol">
                <%= Html.TextBox("username", null, new { @class = "login-text" })%>
                <div class="lcerr">
                <%= Html.ValidationMessage("username") %>
                </div>
            </div>
       
            <div class="lcspacer"></div>
       
            <div class="lclabel">
                <label for="password">Password:</label>
            </div>            
            <div class="lccontrol">
                <%= Html.Password("password", null, new { @class = "login-text" }) %>
                <div class="lcerr">
                <%= Html.ValidationMessage("password")%>
                </div>
            </div>
       
            <div class="lclabel">
            &nbsp;
            </div>
            <div class="lccontrol">
                <%= Html.CheckBox("rememberMe") %> 
                <label class="lclabelremember" for="rememberMe">Remember me?</label>
            </div>
       
            <div class="lcspacer"></div>
              
            <div class="lclabel">
            &nbsp;
            </div>              
            <div class="lcsubmit">
                <input type="submit" value="Log On" />
            </div>
    
        </div>
<% } %>