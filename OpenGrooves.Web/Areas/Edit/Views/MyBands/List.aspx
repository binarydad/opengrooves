<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Edit/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenGrooves.Web.Models.BandModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	View
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <span class="list-actions"><%: Html.RouteLink("Join/Create a Band", "mybands", new { action = "join" }) %></span>
    <h1>My Bands</h1>

    <p class="info">These are the band you are in.</p>

    <%: Html.Partial("BandsList", Model) %>

</asp:Content>
