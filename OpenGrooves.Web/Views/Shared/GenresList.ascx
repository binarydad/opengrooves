<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<OpenGrooves.Web.Models.GenreModel>>" %>

<ul class="list">
<% if (!Model.Any()) { %><li class="empty">No events</li><% } %>
<% Model.ToList().ForEach(f => { %>
    <li>
        <div class="col"><%: Html.RouteLink(f.Name, "browse", new { action = "genre", filter = f.Name.ToLower() }) %></div>
        <div class="col right"><%: f.Bands.Count() %> bands</div>
        <div class="clear"></div>
    </li>
<% }); %>
</ul>