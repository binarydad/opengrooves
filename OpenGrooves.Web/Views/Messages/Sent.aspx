<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Messages.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenGrooves.Web.Models.MessageModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Sent
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <span class="list-actions"><%: Html.RouteLink("Compose Message", "Messages", new { action = "create" }) %></span>
    <h2>Sent Messages</h2>

    <ul class="list">
    <% if (!Model.Any()) { %><li class="empty">No messages</li><% } %>
    <% Model.ToList().ForEach(f => { %>
        <li>
            <div class="col">
                <%: Html.RouteLink(f.Content.Truncate(50), "Messages", new { action = "view", messageId = f.MessageId }) %>
                <small>
                 to <%= String.Join(", ", f.Recipients.Select(g => Html.RouteLink(g.User.UserName, "user", new { action = "profile", username = g.User.UserName.ToLower() }).ToString()).DefaultIfEmpty("Unavailable"))%>
                 / Sent on <%: f.Date.PrettyDateTime() %>
                </small>
            </div>
            <div class="col right">
                <%--<a class="delete" data-id="<%: f.MessageId %>" href="#">Delete</a>--%>
            </div>
            <div class="clear"></div>
        </li>
    <% }); %>
    </ul>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
