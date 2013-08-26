<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<OpenGrooves.Web.Models.UserModel>>" %>

<ul class="list columns">
<% if (!Model.Any()) { %><li class="empty">There's no one here</li><% } %>
<% Model.ToList().ForEach(f => { %>
    <li>
        <%: Html.AvatarImage(f.AvatarUrl) %>
        <%: Html.UserProfileUrl(f.UserName, f.RealName ?? f.UserName) %>
        <small><%: Html.LocationSearchUrl(f.Location) %></small>
        <div class="clear"></div>
    </li>
<% }); %>
</ul>
