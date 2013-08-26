<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/OpenGrooves.Master" Inherits="System.Web.Mvc.ViewPage<BandProfileModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.Band.Name %>
    <% if (Model.Band.Location.HasLocation) { %>
        (<%: OpenGrooves.Core.Helpers.LocationHelper.CityStateOrZip(Model.Band.Location) %>)
    <% } %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<%: Html.Partial("ConnectionActions") %>
	
    <h1><%: Model.Band.Name %></h1>
    
    <div class="band-profile-description">
        <%: Html.AvatarImage(Model.Band.AvatarUrl, false) %>
    </div>

    <fieldset>

        <%-- Location --%>
        <div>
            <%: Html.Label("Hometown") %>
            <% if (Model.Band.Location.HasLocation) { %>
            <%: Html.LocationSearchUrl(Model.Band.City, Model.Band.State, Model.Band.Zip) %>
            <% } else { %>Unknown<% } %>
        </div>

        <%-- Genres --%>
        <div>
            <%: Html.LabelFor(m => m.Band.Genres) %>
            <%= String.Join(", ", Model.Band.Genres.Select(g => Html.RouteLink(g.Name, "Browse", new { action = "genre", filter = g.Name.ToLower() }).ToString()).DefaultIfEmpty("N/A"))%>
        </div>

        <%-- Members --%>
        <div>
            <%: Html.LabelFor(m => m.Band.ActiveMembers) %>
            <span><%: Model.Band.ActiveMembers.Select(u => String.Format("{0} ({1})", Html.UserProfileUrl(u.User.UserName, u.User.RealName ?? u.User.UserName).ToHtmlString(), u.MemberType != null ? u.MemberType.Name : "Member")).DefaultIfEmpty("N/A").ToHtmlList()%></span>
        </div>

        <% if (!Model.Band.Facebook.IsNullOrWhiteSpace()) { %>
        <div>
            <%: Html.LabelFor(m => m.Band.Facebook) %>
            <%: Html.FacebookUrl(Model.Band.Facebook) %>
        </div>
        <% } %>

        <% if (!Model.Band.Twitter.IsNullOrWhiteSpace()) { %>
        <div>
            <%: Html.LabelFor(m => m.Band.Twitter)%>
            <%: Html.TwitterUrl(Model.Band.Twitter)%>
        </div>
        <% } %>

        <% if (!Model.Band.Website.IsNullOrWhiteSpace()) { %>
        <div>
            <%: Html.LabelFor(m => m.Band.Website)%>
            <%: Html.WebsiteUrl(Model.Band.Website)%>
        </div>
        <% } %>
    </fieldset>

    <div class="clear"></div>

    <%-- Feed --%>
    <h2><%: Html.DisplayNameFor(m => m.Feed) %></h2>
    <div id="feed"></div>
    <script type="text/javascript">
        OpenGrooves.AjaxAction.Render('bandfeed', 'band', { bandId: '<%: Model.Band.BandId %>' }, '#feed');
    </script>

    <% if (!Model.Band.Description.IsNullOrWhiteSpace()) { %>
    <h2>About <%: Model.Band.Name %></h2>
    <%= Model.Band.Description.NL2BR() %>
    <% } %>

    <%-- Events --%>
	<h2>Upcoming Events</h2>
    <div id="events"></div>
    <script type="text/javascript">
        OpenGrooves.AjaxAction.Render('bandevents', 'band', { bandId: '<%: Model.Band.BandId %>', max: 10 }, '#events');
    </script>

    <%--Audio--%>
    <h2>Audio</h2>
    <div id="audio"></div>
    <script type="text/javascript">
        OpenGrooves.AjaxAction.Render('bandaudio', 'band', { bandId: '<%: Model.Band.BandId %>' }, '#audio');
    </script>

    <%-- Videos --%>
    <h2>Videos</h2>
    <p class="info">Coming soon.</p>

    <%-- Images --%>
    <h2>Images</h2>
    <div id="images"></div>
    <script type="text/javascript">
        OpenGrooves.AjaxAction.Render('bandimages', 'band', { bandId: '<%: Model.Band.BandId %>' }, '#images');
    </script>

    <%-- Galleries --%>
    <h2>Galleries</h2>
    <div id="galleries"></div>
    <script type="text/javascript">
        OpenGrooves.AjaxAction.Render('bandgalleries', 'band', { bandId: '<%: Model.Band.BandId %>' }, '#galleries');
    </script>

    <%-- Fans --%>
	<h2><%: Html.DisplayNameFor(m => m.Band.Fans) %></h2>
	<div id="fans"></div>
    <script type="text/javascript">
        OpenGrooves.AjaxAction.Render('bandfans', 'band', { bandId: '<%: Model.Band.BandId %>' }, '#fans');
    </script>

</asp:Content>
