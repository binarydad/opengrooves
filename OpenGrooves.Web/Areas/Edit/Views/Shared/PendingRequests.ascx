<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BandModel>" %>

<%-- Pending requests --%>
<% if (Model.PendingMemberRequests.Any()) { %>

    <h2><%: Html.DisplayNameFor(m => m.PendingMemberRequests)%></h2>
    <p class="callout">
        The following users have requested to be a member of this band. 
    </p>
    <ul class="list">
    <% Model.PendingMemberRequests.ToList().ForEach(f => { %>
        <li>
            <div class="col"><%: Html.RouteLink(f.User.RealName ?? f.User.UserName, "user", new { username = f.User.UserName })%></div>
            <div class="col right">
            <% using (Html.BeginForm(new { action = "approve", bandUrl = Model.UrlName })) { %>
                <%: Html.Hidden("relationId", f.RelationId) %>
                <%: Html.AntiForgeryToken() %>
                <a href="#" class="submit-button">Accept</a>
            <% } %>
            / 
            <% using (Html.BeginForm(new { action = "deleterelation", bandUrl = Model.UrlName })) { %>
                <%: Html.Hidden("relationId", f.RelationId) %>
                <%: Html.AntiForgeryToken() %>
                <a href="#" class="submit-button confirm">Deny</a>
            <% } %>
            </div>
            <div class="clear"></div>
        </li>
    <% }); %>
    </ul>
<% } %>