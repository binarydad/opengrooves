<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Browse.Master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Models.BrowseModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Nearby
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="BrowseTitle">
    Browse Local Bands
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <p class="callout">
        To find bands in your area, enter a location and a distance below. 
    </p>

    <div id="local-bands"></div>
    
    <script type="text/javascript">
        OpenGrooves.AjaxAction.Render('localbands', 'browse', { max: 30, address: '<%: Model.Address %>' }, '#local-bands');
    </script>
</asp:Content>
