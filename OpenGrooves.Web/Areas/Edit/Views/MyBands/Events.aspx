<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Edit/Views/Shared/BandProfile.master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<BandEventRelation>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Events
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <span class="list-actions">
        <%: Html.RouteLink("Create Event", "MyEvents", new { action = "list" }) %>
    </span>
    <h2>Events</h2>

    <p class="callout">
        The following events are associated with this band. You may only edit events that you have created.
    </p>

    <ul class="list">
    <% var events = Model.Where(e => e.Event.Date >= DateTime.Now); %>
    <% if (!events.Any()) { %><li class="empty">No events</li><% } %>
    <% events.ToList().ForEach(f => { %>
        <li>
            <div class="col"><%: Html.RouteLink(f.Event.Name, "myevents", new { action = "edit", eventName = f.Event.UrlName })%></div>
            <div class="col right"><%: f.Event.Date.PrettyDateTime() %></div>
            <div class="clear"></div>
        </li>
    <% }); %>
    </ul>

    <% if (Model.Any(r => !r.IsActive)) { %>

    <h2>Pending Events</h2>
    <p class="callout">
        Your band has been requested to play at these events. Please confirm or ignore the events below.
    </p>
    <ul class="list">
    <% Model.Where(r => !r.IsActive).ToList().ForEach(f => { %>
        <li>
            <div class="col">
                <% 
                    var requestedBy = String.Join(", ", f.Event.Users.ToList().Select(u => Html.RouteLink(u.RealName ?? u.UserName, "user", new { action = "profile", username = u.UserName.ToLower() })));
                %>
                <%: Html.RouteLink(f.Event.Name, "event", new { name = f.Event.UrlName })%>
                <small>Requested by <%= requestedBy %></small>
            </div>
            <div class="col right">
            <% using (Html.BeginForm(new { controller = "MyEvents", action = "approve" })) { %>
                <%: Html.Hidden("relationId", f.RelationId) %>
                <%: Html.AntiForgeryToken() %>
                <a href="#" class="submit-button">Accept Event Invite</a>
            <% } %>
            </div>
            <div class="clear"></div>
        </li>
    <% }); %>
    </ul>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
