<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Browse.Master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Models.BrowseModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Genre
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="BrowseTitle">
    Newest Band Signups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<ul class="list">
    <% if (!Model.Bands.Any()) { %><li class="empty">No new bands</li><% } %>
	<% Model.Bands.ToList().ForEach(b => { %>
		<li>
            <div class="col">
                <%: Html.AvatarImage(b.AvatarUrl) %>
                <%: Html.BandProfileUrl(b.UrlName, b.Name) %>
                <small>
                    <% if (b.Location != null && b.Location.HasLocation) { %>
                    <%: Html.LocationSearchUrl(b.Location) %>
                    <% } %>
                    <% if (b.Genres != null && b.Genres.Any()) { %>
                     / <%= String.Join(", ", b.Genres.OrderBy(g => g.Name).Select(g => Html.RouteLink(g.Name, "Browse", new { action = "genre", filter = g.Name.ToLower() }).ToString()).DefaultIfEmpty("No genre"))%>
                    <% } %>
                </small>
            </div>
            <div class="col right">joined <%: b.Date.PrettyDate() %></div>
            <div class="clear"></div>
        </li>
	<% }); %>
	</ul>

</asp:Content>
