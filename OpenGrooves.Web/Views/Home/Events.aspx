<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/OpenGrooves.Master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Models.HomeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Upcoming Events
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="profile-page-action">
        <%: Html.RouteLink("My Events", "myevents", new { action = "list" }) %>
    </div>

    <h1>Local Events</h1>

    <% if (Request.IsAuthenticated) { %>
        
        <h2>Upcoming Events</h2>

        <div id="upcoming-events"></div>
        <script type="text/javascript">
            OpenGrooves.AjaxAction.Render('upcomingevents', 'browse', null, '#upcoming-events');
        </script>

    <% } %>

    <h2>Events In My Area</h2>
    <div id="local-events"></div>
    <script type="text/javascript">
        OpenGrooves.AjaxAction.Render('localevents', 'browse', { max: 5 }, '#local-events');
    </script>

</asp:Content>
