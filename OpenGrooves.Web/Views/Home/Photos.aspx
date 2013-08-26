<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/OpenGrooves.Master" Inherits="System.Web.Mvc.ViewPage<HomeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Images
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="profile-page-action">
        <%: Html.RouteLink("My Photos", "myimages", new { action = "list" }) %>
    </div>

    <h1>Recent Photos</h1>

    <% if (Request.IsAuthenticated) { %>

        <%--Authenticated--%>
        <%: Html.Partial("GalleryList", Model.Galleries)%>
        <h2>Individual Photos</h2>
        <%: Html.Partial("RecentPhotos", Model.Images)%>

    <% } else { %>

        <%--Anonymous--%>
        <p class="info">A handful of recently posted photos are below. <%: Html.RouteLink("Sign in", "login", new { action = "login" }) %> or <%: Html.RouteLink("Sign Up", "signup", new { action = "signup" }) %> to access more photos</p>
        <%: Html.Partial("RecentPhotos", Model.Images)%>

    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
