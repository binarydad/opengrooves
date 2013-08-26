<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Browse.Master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Models.BrowseModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Letter
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="BrowseTitle">
    Browse Alphabetically
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <p class="callout">
        Click a letter from the list below to display bands beginning with that letter.
    </p>

    <%: Html.LetterBrowser() %>

    <%: Html.Partial("BandsList", Model.Bands) %>

</asp:Content>
