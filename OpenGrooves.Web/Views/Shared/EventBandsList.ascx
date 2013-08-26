<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<BandEventRelation>>" %>

<ul class="list">
<% if (!Model.Any()) { %><li class="empty">No bands here</li><% } %>
<% Model.OrderBy(b => b.ShowTime).OrderBy(b => b.Order).ToList().ForEach(f => { %>
    <% var numEvents = f.Band.ActiveEvents != null ? f.Band.ActiveEvents.Count(e => e.Event.Date > DateTime.Now) : 0; %>
    <li>
        <div class="col">
            <%: Html.AvatarImage(f.Band.AvatarUrl) %>
            <%: Html.BandProfileUrl(f.Band.UrlName, f.Band.Name) %>
            <small>
                <% if (f.Band.Genres != null) { %>
                <%= String.Join(", ", f.Band.Genres.OrderBy(g => g.Name).Select(g => Html.RouteLink(g.Name, "Browse", new { action = "genre", filter = g.Name.ToLower() }).ToString()).DefaultIfEmpty("No genre"))%> / 
                <% } %>
                <%: f.Band.Description.DefaultIfEmpty("No descrption").Truncate(50) %>
            </small>
        </div>
        <div class="col right">
            <% if (f.ShowTime.HasValue || f.Order.HasValue) { %>
                Going on <strong><%: f.ShowTime.HasValue ? String.Format("at {0}", ((DateTime)f.ShowTime).PrettyTime()) : ((int)f.Order).AddOrdinal()%></strong>
            <% } %>
        </div>
        <div class="clear"></div>
    </li>
<% }); %>
</ul>