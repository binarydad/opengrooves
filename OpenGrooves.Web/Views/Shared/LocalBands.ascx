<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OpenGrooves.Web.Models.BrowseModel>" %>

<%: Html.Partial("EmptyLocation", Model) %>

<%: Html.Partial("TinySearch") %>

<ul class="list">
<% if (Model.Bands == null || !Model.Bands.Any()) { %><li class="empty">There are no bands in your area. Try expanding your search radius.</li><% } else { %>
<% var locations = Model.Bands.Select(b => new { Band = b, Radius = LocationHelper.Radius(Model.Location.Coordinate.Latitude, Model.Location.Coordinate.Longitude, b.Latitude, b.Longitude) }).ToList(); %>
<% locations.ForEach(f => { %>
    <% 
        var numEvents = f.Band.ActiveEvents.Where(d => d.Event.Date >= DateTime.Now).Count();
        var radius = OpenGrooves.Core.Helpers.LocationHelper.Radius(Model.Location.Coordinate.Latitude, Model.Location.Coordinate.Longitude, f.Band.Latitude, f.Band.Longitude);   
    %>
    <li>
        <div class="col">
            <%: Html.AvatarImage(f.Band.AvatarUrl) %>
            <%: Html.BandProfileUrl(f.Band.UrlName, f.Band.Name) %>
            <small>
                <% if (f.Band.Genres != null && f.Band.Genres.Any()) { %>
                    <%= String.Join(", ", f.Band.Genres.OrderBy(g => g.Name).Select(g => Html.RouteLink(g.Name, "Browse", new { action = "genre", filter = g.Name.ToLower() }).ToString()).DefaultIfEmpty("No genre"))%>
                <% } %>
                <% if (!f.Band.Description.IsNullOrWhiteSpace()) {  %>
                 / <%: f.Band.Description.Truncate(50) %>
                <% } %>
            </small>
        </div>
        <div class="col right">
            <%: Html.LocationSearchUrl(f.Band.Location) %>
            (<%: radius%> miles)
        </div>
        <div class="clear"></div>
    </li>
<% }); }%>
</ul>
