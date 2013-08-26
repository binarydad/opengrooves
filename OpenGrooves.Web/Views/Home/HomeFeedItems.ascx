<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<FeedItemModel>>" %>

<% if (!Model.Any()) { %>
    <p class="callout">
        <strong>It seems like you're either a new member of you're not following any bands (which can become pretty boring).</strong>
        <br />
        <strong>To get started,</strong> <%: Html.RouteLink("browse for bands in your area or by interest", "browse", new { action = "nearby" })%>. From there, you'll see all their activity in this Feed section.
        <br /><br />
        
        <a href="<%: Url.RouteUrl("browse", new { action = "nearby" }) %>">
            <img src="/Content/Images/feed-get-started.gif" />
        </a>
        
    </p>
<% } else { %>
    <%: Html.Partial("FeedItems", Model) %>
<% } %>

