<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BrowseModel>" %>

<% if (Request.IsAuthenticated && (Model.Location == null || !Model.Location.HasLocation)) { %>
    <p class="info">
        It appears that you have not set your location for your user profile. 
        Setting your location allows OpenGrooves to automatically display bands and events in your area. 
        <%: Html.RouteLink("Edit your profile", "myuser", new { action = "edit" }) %> to set this up.
    </p>
<% } %>