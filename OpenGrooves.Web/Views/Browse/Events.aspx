<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Browse.Master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Models.BrowseModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Events
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="BrowseTitle">
    Browse Local Events
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <p class="callout">
        To find events and shows in your area, enter a location and a distance below. 
    </p>

    <div id="local-events"></div>
    <script type="text/javascript">
        OpenGrooves.AjaxAction.Render('localevents', 'browse', { max: 30, address: '<%: Model.Address %>' }, '#local-events');
    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
