<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Messages.Master" Inherits="System.Web.Mvc.ViewPage<string>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Compose Message</h2>

    <strong>Enter a name or username</strong><br />
    <input type="text" id="userId" name="userId" />

    <% using (Ajax.BeginForm(null, null, new AjaxOptions { OnComplete = "OpenGrooves.AjaxForms.OnComplete", OnBegin = "OpenGrooves.AjaxForms.OnBegin" }, new { @class = "checklist", id = "compose-form" })) { %>

        <%-- UI listing of recipients --%>
        <div id="recipients">
            <% if (!Model.IsNullOrWhiteSpace()) { %>
                <span class="message-recipient">
                    <%: Model %>
                    <a class="delete confirm"></a>
                </span>
            <% } %>
            <div class="clear"></div>
        </div>

        <% if (!Model.IsNullOrWhiteSpace()) { %>
            <input type="hidden" name="recipient" value="<%:Model %>" />
        <% } %>

        <div>
            <textarea name="message"></textarea>
        </div>

        <div class="form-buttons">
            <input type="submit" value="Send" />
        </div>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <%: Html.Partial("AutoComplete", new AutoCompleteModel { DataRequestUrl = "/global/users", JqueryFieldSelector = "#userId", JavaScriptClickAction = "window.messageCreateAction" })%>
    <script type="text/javascript">
        var selector = '#userId';
        var messageCreateAction = function (value, data) {

            var form = $('form#compose-form');
            var recipientsList = $('div#recipients');
            var userField = $('#userId');
            var del = $(document.createElement('a')).addClass('delete confirm');

            // add the hidden element
            form.append(
                $(document.createElement('input'))
                    .attr({ type: 'hidden', name: 'Recipient', id: data })
                    .val(data));

            // add to the UI list
            recipientsList.prepend(
                $(document.createElement('span'))
                    .attr({'data-name': data })
                    .addClass('message-recipient')
                    .html(value)
                    .append(del));

            // clear the field
            userField.val('').focus();
        }

        // delete
        $('span.message-recipient a.delete').live('click', function () {
            var recip = $(this).parent();
            var name = recip.attr('data-name');
            $('input#' + name).remove();
            recip.remove();
            return false;
        });
    </script>
</asp:Content>