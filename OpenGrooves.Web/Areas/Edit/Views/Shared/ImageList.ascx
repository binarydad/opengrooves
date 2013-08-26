<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<OpenGrooves.Web.Models.ImageModel>>" %>

<ul class="list images">
    <% Model.ToList().ForEach(i => { %>
    <li id="<%: Html.AttributeEncode(i.ImageId) %>">
        <div class="col">
            <%-- icon --%>
            <div class="image-icon">
                <%: Html.GetImageLink(i, true, anchorAttributes: new { rel = "gallery" })%>
            </div>

            <%-- info --%>
            <div class="image-info">
                <strong><%: i.Caption.Truncate(20) %></strong><br />
                Uploaded by: <%: i.User.UserName %><br />
                Date: <%: i.Date.PrettyDateTime() %>
            </div>

            <%-- gallery --%>
            <div class="image-gallery">
            <% var galleryList = ViewData["galleryList"] as IEnumerable<GalleryModel>; %>    
            <select name="GalleryId" class="img-move">
            <option value="">(No gallery)</option>
            <% galleryList.ToList().ForEach(g => { %>
                <option value="<%: g.GalleryId %>" <% if (g.GalleryId == i.GalleryId) { %>selected="selected" <% } %>><%: g.Name %> <%: g.Band != null ? String.Format("({0})", g.Band.Name) : String.Empty %></option>
            <% }); %>    
            </select>
            </div>
        </div>
        <div class="col right">
            <a href="#" class="confirm img-delete delete"></a>
        </div>
        <div class="clear"></div>
    </li>
    <% }); %>
</ul>

<script type="text/javascript">

    (function () {

        $('ul.images > li').bind('click change', function (e) {

            var target = $(e.target);
            var imgId = $(this).attr('id');

            var removeRow = function () {
                $('ul.list > li#' + imgId).fadeOut('slow', function () { $(this).remove(); });
            };

            // delete image
            if (target.hasClass('img-delete') && e.type == 'click') {

                $.ajax({
                    url: '/settings/images/deleteimage',
                    type: 'POST',
                    data: 'imageId=' + imgId,
                    error: function () {
                        alert('Could not delete image.');
                    }
                });

                removeRow();

                return false;
            }
            // move image
            else if (target.hasClass('img-move') && e.type == 'change') {

                var galleryId = target.val();

                $.ajax({
                    url: '/settings/images/moveimage',
                    type: 'POST',
                    data: 'imageId=' + imgId + '&galleryId=' + galleryId,
                    error: function () {
                        alert('Could not move image.');
                    }
                });

                removeRow();

                return false;
            }

            
        });
    })();

</script>