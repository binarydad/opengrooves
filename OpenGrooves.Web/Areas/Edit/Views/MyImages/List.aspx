<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Edit/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<EditImagesModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>My Images</h1>

    <h2>Create Gallery</h2>
    <%: Html.ValidationSummary() %>
    <% using (Html.BeginForm(new { action = "creategallery" })) { %>
        <%: Html.AntiForgeryToken() %>
        <%: Html.Hidden("bandUrl", Request.QueryString["bandUrl"]) %>
        <%: Html.TextBox("newGalleryName", null, new { @class = "large" }) %>
        <div class="form-buttons">
            <input type="submit" value="Create Gallery" />
        </div>
    <% } %>

    <h2>Galleries</h2>
    <%: Html.Partial("GalleryList", Model.Galleries) %>

    <h2>Uncategorized</h2>
    <%-- uploadify --%>
    <%: Html.Partial("UploadifyButton") %>

    <%: Html.Partial("ImageList", Model.Images) %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <%: Html.Uploadify(Url.RouteUrl(new { controller = "upload", action = "uploadimage" }))%>
</asp:Content>
