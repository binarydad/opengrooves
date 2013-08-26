<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Edit/Views/Shared/BandProfile.master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Models.BandModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Members
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% var loggedUserGuid = (Guid)ViewData["loggedUserGuid"]; %>

    <%-- Members --%>
    <h2><%: Html.DisplayNameFor(m => m.ActiveMembers) %></h2>
    <ul class="list">
    <% if (!Model.ActiveMembers.Any()) { %><li class="empty">No members</li><% } %>
    <%
        var memberTypes = ((IEnumerable<OpenGrooves.Web.Models.MemberTypeRelation>)ViewData["memberTypes"]).Select(m => new SelectListItem { Value = m.MemberTypeId.ToString(), Text = m.Name, Selected = Model.ActiveMembers.Single(u => u.UserId == loggedUserGuid).MemberTypeId == m.MemberTypeId });
    %>
    <% Model.ActiveMembers.ToList().ForEach(f => { %>
        <li>
            <div class="col"><%: Html.RouteLink(f.User.RealName ?? f.User.UserName, "user", new { username = f.User.UserName })%></div>
            <div class="col right">
                <% if(f.User.UserId == loggedUserGuid) { %>
                    <%: Html.DropDownList("MemberTypeId", memberTypes, new { @class = "member-type" })%> / 
                    <% using (Html.BeginForm(new { action = "deleterelation" })) { %>
                    <%: Html.Hidden("relationId", f.RelationId) %>
                    <%: Html.AntiForgeryToken() %>
                    <a href="#" class="submit-button">I QUIT!</a>
                <% } %>
                <% } else { %>
                <% using (Html.BeginForm(new { action = "deleterelation" })) { %>
                    <%: Html.Hidden("relationId", f.RelationId) %>
                    <%: Html.AntiForgeryToken() %>
                    <a href="#" class="submit-button confirm">Kick From Band</a>
                <% } }%>
            </div>
            <div class="clear"></div>
        </li>
    <% }); %>
    </ul>

    <%-- Invite a member --%>
    <h2>Invite a Member</h2>

    <div class="callout">
        Enter the username of the member you wish to invite.
    </div>

    <% using(Html.BeginRouteForm("mybands", new { action = "invite", bandUrl = Model.UrlName })) { %>
        <input type="text" name="username" class="large" id="username" />
        <div class="form-buttons">
            <input type="submit" value="Send Invite" />
        </div>
    <% } %>

    <%-- Pending invites --%>
    <% if (Model.PendingMemberInvites.Any()) { %>

        <h2><%: Html.DisplayNameFor(m => m.PendingMemberInvites)%></h2>
        <p class="callout">
            The following users have been sent an invitation to join this band. 
        </p>
        <ul class="list">
        <% Model.PendingMemberInvites.ToList().ForEach(f =>
           { %>
            <li>
                <div class="col"><%: Html.RouteLink(f.User.RealName ?? f.User.UserName, "user", new { username = f.User.UserName })%></div>
                <div class="col right">
                <% using (Html.BeginForm(new { action = "deleterelation", bandUrl = Model.UrlName })) { %>
                    <%: Html.Hidden("relationId", f.RelationId) %>
                    <%: Html.AntiForgeryToken() %>
                    <a href="#" class="submit-button confirm">Cancel Invite</a>
                <% } %>
                </div>
                <div class="clear"></div>
            </li>
        <% }); %>
        </ul>
    <% } %>

    <%-- Pending requests --%>
    <%: Html.Partial("PendingRequests", Model) %>

    <script type="text/javascript">
        (function () {
            $('select.member-type').change(function () {
                var memberTypeId = $(this).val();
                $.ajax({
                    type: 'POST',
                    url: '<%: Url.RouteUrl("mybands", new { action = "setmembertype", bandUrl = Model.UrlName }) %>',
                    data: 'memberTypeId=' + memberTypeId,
                    success: function (d) {
                        alert(d.success ? 'Updated saved!' : 'Update failed');
                    }
                });
            });
        })();
    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <%: Html.Partial("AutoComplete", new AutoCompleteModel { DataRequestUrl = "/global/users", JavaScriptClickAction = "window.memberAction", JqueryFieldSelector = "#username" })%>
    <script type="text/javascript">
        var memberAction = function (value, data) {
            $('input#username').val(data);
            return false;
        }
    </script>
</asp:Content>
