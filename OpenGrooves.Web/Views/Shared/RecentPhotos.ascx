<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<ImageModel>>" %>

<% if (Model.Any()) { %>
<div class="gallery">
<% Model.ToList().ForEach(i => { %>
    <div>
        <%: Html.GetImageLink(i, true, anchorAttributes: new { rel = "gallery" }) %>

        <small>
            <div><strong><%= i.Gallery == null ? i.Caption.Truncate(14) : Html.RouteLink(i.Gallery.Name.Truncate(14), "gallery", new { action = "view", galleryName = i.Gallery.UrlName }).ToString()%></strong></div>
            <div><%: Html.BandProfileUrl(i.Bands.LastOrDefault().UrlName, i.Bands.LastOrDefault().Name.Truncate(20))%></div>
        </small>
    </div>
<% }); %>
</div>
<div class="clear"></div>
<% } else { %>
    <div class="empty">No photos have been posted recently.</div>
<% } %>