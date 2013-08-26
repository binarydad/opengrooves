<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Anonymous.Master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Models.DefaultModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Welcome Baltimore musicians and fans!
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="home-bg">
        <div class="intro">
            <h1>Discover Local Music</h1>
            <p class="info">
                WELCOME BALTIMORE MUSICIANS! It's a little quiet on the home front, but things will pick up soon. Thank you for the support!
            </p>

            <p>
                OpenGrooves is a community dedicated to connecting fans to the local music and artists in Baltimore and the surrounding areas. Nothing more. <a href="http://blog.opengrooves.com" target="_blank">Read more about OpenGrooves on our blog!</a>
            </p>
            
            <p>
                <strong>Like a band?</strong> Keep track of events, photos, music, and news all from one place. 
                <strong>Into new music?</strong> Discover new bands in your area. <strong>Want to find events in your area?</strong> 
                Upcoming shows are at your fingertips. Unlike other social networking sites, 
                OpenGrooves is dedicated solely to local bands, artists, and fans without all the fluff.
            </p>
            
        </div>
        <div class="sign-in">
            <h2>Sign In</h2>
            <p class="callout">Don't have an account? <strong><%: Html.RouteLink("Sign up!", "signup")%></strong>.</p>
            <%: Html.Partial("LoginControl", new OpenGrooves.Web.Models.LoginModel())%>
        </div>
        <div class="clear"></div>
    </div>

    <div class="home-browse">
    
        <div class="browse">

            <h2>Find Local Bands</h2>

            <% using(Html.BeginRouteForm("browse", new { action = "nearby" }, FormMethod.Get)) { %>
                <input type="text" name="address" id="band-query" class="large watermark" data-watermark="Enter your city, state, or zip" />
                <div class="form-buttons">
                    <input type="submit" value="Find Bands"  />
                </div>
            <% } %>
        
        </div>

        <div class="browse">

            <h2>Find Local Events</h2>
    
            <% using(Html.BeginRouteForm("browse", new { action = "events" }, FormMethod.Get)) { %>
            <input type="text" name="address" id="event-query" class="large watermark" data-watermark="Enter your city, state, or zip" />
            <div class="form-buttons">
                <input type="submit" value="Find Events" />
            </div>
        <% } %>
        
        </div>
    </div>
    

    <h2>See who's signing up!</h2>
    <%: Html.Partial("BandsList", Model.NewBands) %>

</asp:Content>
