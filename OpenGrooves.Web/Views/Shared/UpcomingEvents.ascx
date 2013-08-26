<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OpenGrooves.Web.Models.BrowseModel>" %>

<% if (!Model.Events.Any()) { %>
<p class="info">
    Get started by clicking <strong><%: Html.RouteLink("Browse", "browse", new { action = "newest" })%></strong> for bands or find local <%: Html.RouteLink("Events", "browse", new { action = "events" }) %> in your area. Once you begin following bands, any upcoming shows that they're playing at will display in this area.
</p>
<% } else { %>
<p class="callout">
    Keep track of events and upcoming shows from the bands that <%: Html.RouteLink("you are following", "home", new { action = "bands" }) %>. 
</p>
<% } %>

<% if (Model.Events != null) { %>
<%: Html.Partial("EventsList", Model.Events) %>
<% } %>