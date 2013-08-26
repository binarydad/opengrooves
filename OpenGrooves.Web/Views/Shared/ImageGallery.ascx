<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<OpenGrooves.Web.Models.ImageModel>>" %>


<% if (!Model.Any()) { %>
    <div class="empty">No images available</div>
<% } %>

<div class="gallery">
<% Model.ForEach(i => { %>
    <div>
        <%: Html.GetImageLink(i, true, anchorAttributes: new { rel = "gallery" }) %>
    </div>
<% }); %>
</div>
<div class="clear"></div>