<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<GalleryModel>>" %>

<ul class="list">
<% if (!Model.Any()) { %><li class="empty">No galleries</li><% } %>
<% Model.OrderByDescending(b => b.Date).ToList().ForEach(b =>{ %>
    <li>
        <div class="col">
            <% var firstImage = b.Images.FirstOrDefault(); %>
            <% if (firstImage != null) { %>
                <%: Html.AvatarImage(firstImage.Url) %>
            <% } %>
            <%: Html.RouteLink(b.Name.DefaultIfEmpty("N/A"), "Gallery", new { action = "view", galleryName = b.UrlName }) %>
            <% if (b.Band != null) { %>
                by <%: Html.BandProfileUrl(b.Band.UrlName, b.Band.Name)%>
            <% } %>
            <small><%: b.Description.Truncate(100) %></small>
        </div>
        <div class="col right"><%: b.Images.Count() %> images / Added <%: b.Date.PrettyDate() %></div>
        <div class="clear"></div>
    </li>
<% }); %>
</ul>