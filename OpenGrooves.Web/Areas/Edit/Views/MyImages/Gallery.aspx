<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Edit/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Models.GalleryModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Gallery
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1><%: Model.Name %></h1>

    <% Html.EnableClientValidation(); %>
    <% using (Ajax.BeginForm(new AjaxOptions { OnComplete = "OpenGrooves.AjaxForms.OnComplete", OnBegin = "OpenGrooves.AjaxForms.OnBegin" })) { %>
        <%: Html.AntiForgeryToken() %>
        <%: Html.FormFieldFor(m => m.Name) %>
        <div>
            <% var bands = ViewData["myBands"] as IEnumerable<BandModel>; %>
            <%: Html.Label("Band") %>
            <select name="bandId">
            <option value="">(None)</option>
            <% bands.ToList().ForEach(b => { %>
               <option <% if (b.BandId == Model.BandId) { %>selected="selected"<% } %> value="<%: b.BandId %>"><%: b.Name %></option>
            <% }); %>
            </select>
        </div>
        <%: Html.FormFieldFor(m => m.Description) %>
        <div class="form-buttons">
            <input type="submit" value="Save Gallery" />
        </div>
    <% } %>

    <h2>Images</h2>

    <%-- uploadify --%>
    <%: Html.Partial("UploadifyButton") %>

    <%: Html.Partial("ImageList", Model.Images) %>

    <h2>Delete Gallery</h2>

    <div class="callout">
        This will remove the gallery from the system. It cannot be undone.
    </div>

    <% using(Html.BeginForm(new { action = "deletegallery", galleryId = Model.GalleryId })) {%>
        <%: Html.AntiForgeryToken() %>
        <div class="form-buttons">
            <input type="submit" class="confirm" value="Delete Gallery" />
        </div>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

    <%: Html.Uploadify(Url.RouteUrl(new { controller = "upload", action = "uploadimage", galleryName = Model.UrlName, bandUrl = Model.Band != null ? Model.Band.UrlName : null }), "window.uploadCallback")%>

    <% if (Model.Band != null) {%>
    <% var bandUrl = Model.Band.UrlName; %>
    <script type="text/javascript">
        window.uploadCallback = function (e, d) {
            $.ajax({
                url: '/settings/images/uploadbatchcomplete',
                data: 'bandUrl=<%: bandUrl %>&batchId=' + batchId,
                type: 'POST',
                success: function () {
                    document.location.href = document.location.href;
                }
            });
        };
    </script>            
    <% } %>
</asp:Content>
