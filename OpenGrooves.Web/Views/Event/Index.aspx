<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/OpenGrooves.Master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Models.EventModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Upcoming events
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="profile-page-action">
        <% if (Request.IsAuthenticated && Model.Users.Any(e => e.UserId == (Guid)ViewData["loggedUserGuid"])) { %><%: Html.RouteLink("Edit Event", "myevents", new { action = "edit", eventName = Model.UrlName }) %><% } %>
    </div>

    <h1><%: Model.Name %></h1>

    <div class="event-profile-description">
        <%: Html.AvatarImage(Model.AvatarUrl, false) %>
        <%= Model.Description.NL2BR().DetectLinks() %>
    </div>
    
    <div class="clear"></div>
    <h2>General Information</h2>

    <fieldset>

        <%-- Date --%>
        <div>
            <%: Html.LabelFor(m => m.Date) %>
            <strong><%: Model.Date.PrettyDateTime() %></strong>
            <% if (Model.Date.IsToday()) { %> - TODAY!<% } %>
        </div>
        
        <%-- Location, city, state, distance --%>
        <% var userLocation = (OpenGrooves.Core.Location)ViewData["userLocation"]; %>
        <div>
            <%: Html.Label("Location") %>
            <span>
                <strong><%: Html.LineItem(Model.VenueName) %></strong>
                <%: Html.LocationSearchUrl(Model.Location) %>

                <% if (userLocation != null) { %>
                    (<%: OpenGrooves.Core.Helpers.LocationHelper.Radius(Model.Latitude, Model.Longitude, userLocation.Coordinate.Latitude, userLocation.Coordinate.Longitude)%>
                    miles from <%: OpenGrooves.Core.Helpers.LocationHelper.CityStateOrZip(userLocation) %>)
                <% } %>
            </span>
        </div>

        <%-- Genres --%>
        <div>
            <label>Types of Music</label>
            <span>
                <% var bandGenres = Model.Bands.Select(be => be.Band).SelectMany(b => b.Genres).Distinct(new OpenGrooves.Web.Comparers.GenreComparer());%>
                <% if (bandGenres != null) { %>
                <%= String.Join(", ", bandGenres.OrderBy(g => g.Name).Select(g => Html.RouteLink(g.Name, "Browse", new { action = "genre", filter = g.Name.ToLower() }).ToString()).DefaultIfEmpty("Not available"))%>
                <% } %>
            </span>
        </div>

        <%-- Address --%>
        <%: Html.ReadOnlyFieldFor(m => m.Address) %>

        <% if (!Model.OtherBands.IsNullOrWhiteSpace()) { %>
        <%: Html.ReadOnlyFieldFor(m => m.OtherBands) %>
        <% } %>

    </fieldset>

    <h2>Bands Playing</h2>
    <%: Html.Partial("EventBandsList", Model.Bands) %>

    <%: Html.Partial("GoogleMap", Model.Location) %>

</asp:Content>
