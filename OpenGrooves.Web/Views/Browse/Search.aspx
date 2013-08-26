<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Browse.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenGrooves.Web.Models.BandModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Search
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="BrowseTitle">
    Search Bands
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% using(Html.BeginRouteForm("browse", new { action = "search" }, FormMethod.Get)) { %>
        <input type="text" name="query" class="large" id="query" value="<%: Request.QueryString["query"] %>" />
        <div class="form-buttons">
            <input type="submit" value="Search" />
        </div>
    <% } %>

    <h2>Search Results</h2>

    <%: Html.Partial("BandsList", Model) %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <%: Html.Partial("AutoComplete", new AutoCompleteModel { DataRequestUrl = "/global/bands", JqueryFieldSelector = "#query", JavaScriptClickAction = "OpenGrooves.AutoCompleteActions.DataRedirect" })%>
</asp:Content>