<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Messages.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<MessageRecipientRelation>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <p class="info">
        ATTENTION! This section is incomplete at the moment and may be buggy. Please use with discretion. 
    </p>

    <span class="list-actions"><%: Html.RouteLink("Compose Message", "Messages", new { action = "create" }) %></span>
    <h2>Received Messages</h2>

    <ul class="list">
    <% if (!Model.Any()) { %><li class="empty">No messages</li><% } %>
    <% Model.ToList().ForEach(f => { %>
        <li <%= !f.IsRead ? "class=\"unread\"" : String.Empty %>>
            <div class="col">
                <%: Html.RouteLink(f.Message.Content.Truncate(50), "Messages", new { action = "view", messageId = f.MessageId }) %>
                <small>
                    from <%= Html.RouteLink(f.Message.Sender.UserName, "user", new { action = "profile", username = f.Message.Sender.UserName.ToLower() })%>
                     / Received on <%: f.Message.Date.PrettyDateTime() %>
                </small>
            </div>
            <div class="col right">
                <a class="delete" class="confirm delete" data-id="<%: f.MessageId %>" href="#"></a>
            </div>
            <div class="clear"></div>
        </li>
    <% }); %>
    </ul>

</asp:Content>