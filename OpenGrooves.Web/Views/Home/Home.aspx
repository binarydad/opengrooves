<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/OpenGrooves.Master" Inherits="System.Web.Mvc.ViewPage<HomeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Dashboard
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Dashboard</h1>

    <div id="home-feed-items"></div>
    <script type="text/javascript">
        OpenGrooves.AjaxAction.Render('homefeeditems', 'home', { max: 8 }, '#home-feed-items');
    </script>

    <h2>Upcoming Events</h2>
    <div id="upcoming-events"></div>
    <script type="text/javascript">
        OpenGrooves.AjaxAction.Render('upcomingevents', 'browse', { max: 8 }, '#upcoming-events');
    </script>
    
    <h2>New Bands In My Area</h2>
    <div id="local-bands"></div>
    <p><%: Html.RouteLink("Browse more bands in this location", "browse", new { action = "nearby" }) %></p>
    <script type="text/javascript">
        OpenGrooves.AjaxAction.Render('localbands', 'browse', { max: 3 }, '#local-bands');
    </script>

    <h2>Recent Photos</h2>
    <div id="gallery"></div>
    <script type="text/javascript">
        OpenGrooves.AjaxAction.Render('recentphotos', 'galleryajax', null, '#gallery');
    </script>

</asp:Content>