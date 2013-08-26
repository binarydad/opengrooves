<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/OpenGrooves.Master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Models.HomeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Band Connections
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="profile-page-action">
        <%: Html.RouteLink("My Bands", "mybands", new { action = "list" }) %>
    </div>

    <h1>Connections</h1>

    <% if (!Model.Bands.Any()) { %>
        <p class="callout">
            <strong>You don't seem to be following any artists!</strong> Get started by clicking <strong><%: Html.RouteLink("Browse", "browse", new { action = "nearby" })%></strong> to find bands in your area. 
        </p>
    <% } %>

    <%: Html.Partial("BandsList", Model.Bands) %>

    <% if (Model.MyBands.Any()) { %>
    <h2>Bands I'm In</h2>
    <%: Html.Partial("BandsList", Model.MyBands)%>
    <% } %>

</asp:Content>
