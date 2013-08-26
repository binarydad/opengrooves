<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BandModel>" %>

<%: Html.AvatarImage(Model.AvatarUrl) %>
<h3><%: Model.Name %></h3>

<% if (!Model.Description.IsNullOrWhiteSpace()) { %>
<p><%: Model.Description.Truncate(200) %></p>
<% } %>

<div><strong><%: OpenGrooves.Core.Helpers.LocationHelper.CityStateOrZip(Model.City, Model.State, Model.Zip) %></strong></div>
<div><%: Model.NumFans %> fans</div>
<div>
    <% var numUpcoming = Model.ActiveEvents.Where(e => e.Event.Date >= DateTime.Now).Count(); %>
    <%: numUpcoming %> upcoming events
    <% if (numUpcoming > 0) { %>
        <br />at
        <%: String.Join(", ", Model.ActiveEvents.Where(e => e.Event.Date >= DateTime.Now).Select(e => String.Format("{0} ({1})", e.Event.Name, e.Event.Date.PrettyDate()))) %>        
    <% } %>
</div> 

<h4>Members</h4>
<% Model.ActiveMembers.ToList().ForEach(u => { %>
    <div><%: u.User.RealName ?? u.User.UserName %></div>
<% }); %>

<h4>Genres</h4>
<%: String.Join(", ", Model.Genres.Select(g => g.Name)) %>