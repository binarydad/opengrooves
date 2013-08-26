<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<OpenGrooves.Web.Models.BandModel>>" %>

<ul class="list">
<% if (!Model.Any()) { %><li class="empty">You are not in any bands</li><% } %>
<% Model.ToList().ForEach(b => { %>
    <li><%: Html.RouteLink(b.Name.DefaultIfEmpty("N/A"), "MyBands", new { action = "edit", bandId = b.BandId }) %></li>
<% }); %>
</ul>