<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<OpenGrooves.Web.Models.VenueModel>>" %>

<h2>
    My Venues
    <span class="list-actions"><%: Html.RouteLink("Create Venue", "MyVenues", new { action = "create" }) %></span>
</h2>

<ul class="list">
<% Model.ToList().ForEach(v => { %>
    <li><%: Html.RouteLink(v.Name, "MyVenues", new { action = "edit", venueName = v.UrlName })%></li>
<% }); %>
</ul>