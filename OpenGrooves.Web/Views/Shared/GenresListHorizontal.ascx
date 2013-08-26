<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<OpenGrooves.Web.Models.GenreModel>>" %>

<ul class="browse-genres">
    <% Model.ToList().ForEach(g => { %>
        <li><%: Html.RouteLink(g.Name, "Browse", new { action = "genre", filter = g.Name.ToLower() }) %></li>
    <% }); %>
    <li class="clear"></li>
</ul>

<div class="clear"></div>