<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<GalleryModel>>" %>
<%
    var groups = Model.GroupBy(g => g.Band != null ? g.Band.Name : "User Galleries");
%>

<% if (!Model.Any()) { %><ul><li class="empty">No galleries</li></ul><% } %>

<% groups.ForEach(g => { %>
        <h3><%: g.Key %></h3>
        <ul class="list">
        <% g.ForEach(b => { %>
            <li>
                <div class="col">
                    <% var firstImage = b.Images.FirstOrDefault(); %>
                    <% if (firstImage != null) { %>
                        <%: Html.AvatarImage(firstImage.Url)%>
                    <% } %>
                    <%: Html.RouteLink(b.Name.DefaultIfEmpty("N/A"), "myimages", new { action = "gallery", galleryName = b.UrlName })%>
                    <small><%: b.Description.Truncate(100)%></small>
                </div>
                <div class="col right"><%: b.Images.Count()%> images</div>
                <div class="clear"></div>
            </li>
        <% }); %>
        </ul>
<% }); %>