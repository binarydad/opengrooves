<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Edit/Views/Shared/BandProfile.master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Areas.Edit.Models.EditAudioModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Audio
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Audio</h2>

    <p class="info">AS OF 4/16, this section is currently underdevelopment! - RP</p>

    <%: Html.Partial("UploadifyButton") %>

    <ul class="list audio">
    <% if (!Model.Audio.Any()) { %><li class="empty">You have no audio for this band</li><% } %>
    <% Model.Audio.ForEach(a => { %>
        <li id="<%: Html.AttributeEncode(a.AudioID) %>">
            <div class="col">
                <%: a.Title %>
            </div>
            <div class="col right">
                <a href="#" class="confirm audio-delete delete"></a>
            </div>
            <div class="clear"></div>        
        </li>
    <% }); %>
    </ul>

    <script type="text/javascript">

        (function () {

            $('ul.audio > li').bind('click change', function (e) {

                var target = $(e.target);
                var audId = $(this).attr('id');

                var removeRow = function () {
                    $('ul.list > li#' + audId).fadeOut('slow', function () { $(this).remove(); });
                };

                // delete image
                if (target.hasClass('audio-delete') && e.type == 'click') {

                    $.ajax({
                        url: '/settings/bands/deleteaudio',
                        type: 'POST',
                        data: 'audioId=' + audId,
                        error: function () {
                            alert('Could not delete audio.');
                        }
                    });

                    removeRow();

                    return false;
                }
            });
        })();
    
    
    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <%: Html.Uploadify(Url.RouteUrl(new { controller = "upload", action = "uploadaudio", bandId = ViewData["bandId"] }))%>
</asp:Content>
