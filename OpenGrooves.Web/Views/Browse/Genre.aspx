<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Browse.Master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Models.BrowseModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: ViewData["genre-name"] %>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="BrowseTitle">
    <%: ViewData["genre-name"] %> Music
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%: Html.Partial("GenresListHorizontal", Model.Genres) %>

    <%: Html.Partial("BandsList", Model.Bands) %>

</asp:Content>
