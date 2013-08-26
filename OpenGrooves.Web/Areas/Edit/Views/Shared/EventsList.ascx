<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<OpenGrooves.Web.Models.EventModel>>" %>

<ul class="list">
<% var events = Model.Where(e => !String.IsNullOrWhiteSpace(e.UrlName)).OrderBy(e => e.Name).ToList(); %>
<% events.ForEach(b => { %>
    <li>
        <div class="col">
            <%: Html.RouteLink(b.Name, "MyEvents", new { action = "edit", eventName = b.UrlName })%>
            <small><%: b.Description.Truncate(50) %></small>
        </div>
        <div class="col right"><%: b.Date.PrettyDateTime() %></div>
        <div class="clear"></div>
    </li>
<% }); %>
</ul>
