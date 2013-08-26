<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/OpenGrooves.Master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Models.GalleryModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.Name %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1><%: Model.Name %></h1>

    <% if (!String.IsNullOrWhiteSpace(Model.Description)) { %>
    <p class="callout"><%: Model.Description.NL2BR() %></p>
    <% } %>

    <div class="photos-view-more">
        <% if (Model.Band != null) { %>
           <%: Html.RouteLink("More photos from " + Model.Band.Name, "band", new { action = "profile", name = Model.Band.UrlName }) %> &raquo; 
        <% } else if (Model.User != null) { %>
           <%: Html.RouteLink("More photos from " + Model.User.RealName ?? Model.User.UserName, "user", new { action = "profile", username = Model.User.UserName }) %> &raquo; 
        <% } %>
    </div>
    <div class="clear"></div>

    <%: Html.Partial("ImageGallery", Model.Images) %>

</asp:Content>