<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<OpenGrooves.Web.Models.EventModel>>" %>



<ul class="list">
<% if (Model == null || !Model.Any()) { %><li class="empty">No events were found.</li><% } else { %>
<% Model.ToList().ForEach(f => { %>
    <li>
        <div class="col">
            <%: Html.AvatarImage(f.AvatarUrl) %>
            <%: Html.EventUrl(f.UrlName, f.Name) %>
            <% if (f.Date.Date == DateTime.Now.Date) { %>
                <strong>- TONIGHT!</strong>
            <% } %>
            <small>
                <%: Html.LocationSearchUrl(f.Location, "events") %> /
                With <%= String.Join(", ", f.Bands.Where(b => b.IsActive).OrderBy(b => b.Band.Name).Select(g => Html.BandProfileUrl(g.Band.UrlName, g.Band.Name).ToString()).DefaultIfEmpty("No bands"))%>. 
            </small>
        </div>
        <div class="col right"><%: f.Date.PrettyDateTime()%></div>
        <div class="clear"></div>
    </li>
<% }); } %>
</ul>