<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<OpenGrooves.Web.Models.BandModel>>" %>

<ul class="list">
<% if (!Model.Any()) { %><li class="empty">No bands here</li><% } %>
<% Model.ToList().ForEach(f => { %>
    <% var numEvents = f.ActiveEvents != null ? f.ActiveEvents.Count(e => e.Event.Date > DateTime.Now) : 0; %>
    <% var nextEvent = numEvents == 0 ? null : f.ActiveEvents.Where(e => e.Event.Date > DateTime.Now).OrderBy(e => e.Event.Date).FirstOrDefault().Event; %>
    <li>
        <div class="col">
            <%: Html.AvatarImage(f.AvatarUrl) %>
            <%: Html.BandProfileUrl(f.UrlName, f.Name) %>
            <small>
                <% if (f.Location != null && f.Location.HasLocation) { %>
                <%: Html.LocationSearchUrl(f.Location) %>
                <% } %>
                <% if (f.Genres != null && f.Genres.Any()) { %>
                 / <%= String.Join(", ", f.Genres.OrderBy(g => g.Name).Select(g => Html.RouteLink(g.Name, "Browse", new { action = "genre", filter = g.Name.ToLower() }).ToString()))%>
                <% } %>
            </small>
        </div>
        <div class="col right">
            <% if (f.ActiveEvents != null) { %>
            <%: numEvents == 0 ? "No" : numEvents.ToString()%> upcoming <%: numEvents == 1 ? "show" : "shows" %>
            <% if (nextEvent != null) { %><br /> at <%: Html.EventUrl(nextEvent) %> <% } %>
            <% if (f.ActiveEvents.Any(e => e.Event.Date.Date == DateTime.Now.Date)) { %>
                <strong> - <%: Html.RouteLink("SHOW TONIGHT", "event", new { action = "index", name = f.ActiveEvents.SingleOrDefault(e => e.Event.Date.IsToday()).Event.UrlName })%></strong>
            <% } %>
            <% } %>
        </div>
        <div class="clear"></div>
    </li>
<% }); %>
</ul>