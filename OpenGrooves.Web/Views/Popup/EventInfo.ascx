<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EventModel>" %>

<%: Html.AvatarImage(Model.AvatarUrl) %>
<h3><%: Model.Name %></h3>

<div><%: Model.Date.PrettyDateTime() %></div>
<div><%: Model.VenueName %></div>
<div><%: Model.City %>, <%: Model.State %> <%: Model.Zip %></div>

<h4>Lineup</h4>
<% Model.Bands.ToList().ForEach(b =>{ %>
    <div><%: b.Band.Name %></div>
<% }); %>