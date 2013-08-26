<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BandProfileModel>" %>

<div class="profile-page-action">
<% 
    if (Request.IsAuthenticated) { %>
    <% if (Model.Status == OpenGrooves.Web.Models.BandProfileModel.FollowStatus.Member) { %>
            <%: Html.RouteLink("Edit Band", "mybands", new { action = "edit", bandId = Model.Band.BandId })%>
	<% }
        else if (Model.Status == OpenGrooves.Web.Models.BandProfileModel.FollowStatus.Pending)
        { %>
        <span class="request-status pending">Membership request pending</span>
    <% }
        else if (Model.Status == OpenGrooves.Web.Models.BandProfileModel.FollowStatus.Following)
        { %>
	<%: Html.RouteLink("Unfollow", "band", new { action = "unfollow" }, new { @class = "unfollow", title = "Disconnect and no longer follow this band." })%>
	<% }
        else
        { %>
	<%: Html.RouteLink("Follow", "band", new { action = "follow" }, new { @class = "follow", title = "Connect to follow this band." })%>
	<% } %>

<% } else { %>
        <%: Html.RouteLink("Sign up to connect!", "Signup", new { action = "signup" }) %>
    <% } %>
</div>