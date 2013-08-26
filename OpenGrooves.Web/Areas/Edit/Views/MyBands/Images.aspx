<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Edit/Views/Shared/BandProfile.Master" Inherits="System.Web.Mvc.ViewPage<EditImagesModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Images
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create Gallery</h2>
    <%: Html.ValidationSummary() %>
    <% using (Html.BeginRouteForm("myimages", new { action = "creategallery", bandUrl = ViewData["bandUrl"] as string })) { %>
        <%: Html.AntiForgeryToken() %>
        <%: Html.TextBox("newGalleryName", null, new { @class = "large" }) %>
        <div class="form-buttons">
            <input type="submit" value="Create Gallery" />
        </div>
    <% } %>

    <h2>Galleries</h2>

    <ul class="list">
    <% if (!Model.Galleries.Any()) { %><li class="empty">No galleries</li><% } %>
    <% Model.Galleries.ToList().ForEach(b =>{ %>
        <li>
            <div class="col"><%: Html.RouteLink(b.Name.DefaultIfEmpty("N/A"), "MyImages", new { action = "gallery", galleryName = b.UrlName }) %></div>
            <div class="col right"><%: b.Images.Count() %> images</div>
            <div class="clear"></div>
        </li>
    <% }); %>
    </ul>

    <h2>Band Photos & Images</h2>

    <div class="uploadify-shell"><input id="file_upload" name="file_upload" type="file" /></div>

    <%: Html.Partial("ImageList", Model.Images) %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <% var bandId =  ViewData["bandId"] as string; %>
    <%: Html.Uploadify(Url.RouteUrl(new { controller = "upload", action = "uploadimage", bandId = bandId }))%>
    <script type="text/javascript">
        var onComplete = function (e, d) {
            $.ajax({
                url: '/settings/upload/uploadimagebatchcomplete',
                data: 'bandId=<%: bandId %>&batchId=' + batchId,
                type: 'POST',
                success: function () {
                    document.location.href = document.location.href;
                }
            });
        };
    </script>
</asp:Content>
