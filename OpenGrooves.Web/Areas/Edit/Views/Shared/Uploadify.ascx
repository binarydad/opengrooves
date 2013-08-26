<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OpenGrooves.Web.Areas.Edit.Models.UploadifyModel>" %>

<%: Html.CssInclude("~/content/uploadify/uploadify.css") %>
<%: Html.ScriptInclude("~/content/uploadify/swfobject.js") %>
<%: Html.ScriptInclude("~/content/uploadify/jquery.uploadify.v2.1.4.min.js") %>
<%
    var cookieName = FormsAuthentication.FormsCookieName;
    var cookieValue = Request.Cookies[cookieName].Value;
    var batchId = Guid.NewGuid();

    var uploadPath = Model.UploadPath;
    var callbackJs = Model.CallbackJS.IsNullOrWhiteSpace() ? "null" : Model.CallbackJS;
%>
<script type="text/javascript">

    var batchId = '<%: batchId %>';
    var callbackJs = <%= callbackJs %>;

    $(document).ready(function () {

        $('#file_upload').uploadify({
            uploader: '/content/uploadify/uploadify.swf',
            script: '<%: uploadPath %>',
            scriptData: ({ 'token': '<%: cookieValue %>', 'batchId': batchId }),
            cancelImg: '/content/uploadify/cancel.png',
            folder: '/uploads',
            auto: true,
            multi: true,
            onAllComplete: function () {
                // if we have a calback defined (usually from the HtmlHelper), check it and call it
                if (callbackJs) {
                    callbackJs();
                }
                // otherwise, just refresh the page to display newly uploaded items
                else {
                    document.location.href = document.location.href;
                }
            }
        });
    });
</script>