<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Edit/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Models.UserModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <span class="list-actions"><%: Html.RouteLink("View My Profile", "user", new { action = "profile", username = Model.UserName.ToLower() }) %></span>
    <h1>My Profile</h1>


    <%-- Pending band member invites --%>
    <% if (Model.PendingBandInvites.Any()) { %>
    <h2 style="color:#009900"><%: Html.DisplayNameFor(m => m.PendingBandInvites)%></h2>
    <div class="callout">
        You have been asked to join the following bands.
    </div>
    <ul class="list">
    <% Model.PendingBandInvites.ToList().ForEach(b => { %>
        <li>
            <div class="col"><%: Html.RouteLink(b.Band.Name, "Band", new { action = "profile", name = b.Band.UrlName })%></div>
            <div class="col right">
                <% using (Html.BeginForm(new { controller = "mybands", action = "approve", bandUrl = b.Band.UrlName })) { %>
                    <%: Html.Hidden("relationId", b.RelationId) %>
                    <%: Html.AntiForgeryToken() %>
                    <a href="#" class="submit-button">Accept</a>
                <% } %>
                / 
                <% using (Html.BeginForm(new { controller = "mybands", action = "deleterelation", bandUrl = b.Band.UrlName })) { %>
                    <%: Html.Hidden("relationId", b.RelationId) %>
                    <%: Html.AntiForgeryToken() %>
                    <a href="#" class="submit-button confirm">No Thanks</a>
                <% } %>
            </div>
            <div class="clear"></div>
        </li>
    <% }); %>
    </ul>
    <% } %>

    <%-- Pending band member requests --%>
    <% if (Model.PendingBandRequests.Any()) { %>
    <h2 style="color:#009900"><%: Html.DisplayNameFor(m => m.PendingBandRequests)%></h2>
    <div class="callout">
        You have requested to join the following bands.
    </div>
    <ul class="list">
    <% Model.PendingBandRequests.ToList().ForEach(b => { %>
        <li>
            <div class="col"><%: Html.RouteLink(b.Band.Name, "Band", new { action = "profile", name = b.Band.UrlName })%></div>
            <div class="col right">
                <% using (Html.BeginForm(new { controller = "MyBands", action = "deleterelation" })) { %>
                    <%: Html.Hidden("relationId", b.RelationId) %>
                    <%: Html.AntiForgeryToken() %>
                    <a href="#" class="submit-button">Cancel Request</a>
                <% } %>
            </div>
            <div class="clear"></div>
        </li>
    <% }); %>
    </ul>
    <% } %>

    <% Html.EnableClientValidation(); %>
    <% using (Ajax.BeginForm(new AjaxOptions { OnComplete = "OpenGrooves.AjaxForms.OnComplete", OnBegin = "OpenGrooves.AjaxForms.OnBegin" })) { %>

        <h2>Required Info</h2>
        <%: Html.FormFieldFor(m => m.Email) %>
        <%: Html.FormFieldFor(m => m.City) %>
        <div>
            <%: Html.LabelFor(m => m.State, true) %>
            <%: Html.StateListFor(m => m.State) %>
        </div>

        <h2>Optional</h2>

        <%: Html.FormFieldFor(m => m.RealName) %>
        <%: Html.FormFieldFor(m => m.Facebook) %>
        <%: Html.FormFieldFor(m => m.Twitter) %>
        <%: Html.FormFieldFor(m => m.AIM) %>
        <%: Html.FormFieldFor(m => m.Zip) %>
        <%: Html.FormFieldFor(m => m.Interests) %>
        <%: Html.FormFieldFor(m => m.Bio) %>

        <div class="form-buttons">
            <input type="submit" value="Update Profile" />
        </div>
    <% } %>

    <h2>Upload Avatar</h2>
    <%: Html.AvatarImage(Model.AvatarUrl) %>
    <% using (Html.BeginForm("uploadavatar", "myuser", FormMethod.Post, new { enctype = "multipart/form-data" })) { %>
        <div>
            <input type="file" name="avatar" />
        </div>
        <div class="form-buttons"><input type="submit" value="Upload Avatar" /></div>
    <% } %>

    <br />

    

    <%-- Pending event requests --%>
    <% if (Model.MemberOfBands.Any(b => b.Band.PendingEvents.Any())) { %>
    <% 
        var pendingEvents = from b in Model.MemberOfBands
                            let events = b.Band.PendingEvents
                            from e in events
                            select new
                            {
                                Band = b.Band,
                                Event = e.Event,
                                Relation = e
                            };
    %>
    <h2>Event Requests</h2>
    <p class="callout">
        The following bands have requested that one of your bands be part of a show or event. 
    </p>
    <ul class="list">
    <% pendingEvents.ToList().ForEach(b => { %>
        <li>
            <div class="col">
                <%: Html.RouteLink(b.Event.Name, "Event", new { action = "index", name = b.Event.UrlName })%>
                <small>For <strong><%: b.Band.Name %></strong></small>
            </div>
            <div class="col right">
                <% using (Html.BeginForm(new { controller = "myevents", action = "approve" })) { %>
                    <%: Html.Hidden("relationId", b.Relation.RelationId) %>
                    <%: Html.AntiForgeryToken() %>
                    <a href="#" class="submit-button">Accept Invite</a>
                <% } %>
            </div>
            <div class="clear"></div>
        </li>
    <% }); %>
    </ul>
    <% } %>

    

</asp:Content>
