<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Edit/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Areas.Edit.Models.EditEventModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<span class="list-actions"><%: Html.RouteLink("View Event", "event", new { action = "index", name = Model.Event.UrlName }) %></span>
	<h1><%: Model.Event.Name %></h1>

	<%: Html.ValidationSummary() %>

	<% Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm(new AjaxOptions { OnComplete = "OpenGrooves.AjaxForms.OnComplete", OnBegin = "OpenGrooves.AjaxForms.OnBegin" })) { %>

		<h2>Event Info</h2>

		<%: Html.FormFieldFor(m => m.Event.Name) %>
		<%: Html.FormFieldFor(m => m.Event.Date) %>
        <%: Html.FormFieldFor(m => m.Event.Description) %>

		<h2>Venue Location</h2>

		<%: Html.FormFieldFor(m => m.Event.VenueName) %>
        <%: Html.FormFieldFor(m => m.Event.Address) %>
		<%: Html.FormFieldFor(m => m.Event.City) %>
		<div>
			<%: Html.LabelFor(m => m.Event.State, true) %>
			<%: Html.StateListFor(m => m.Event.State) %>
		</div>
		<%: Html.FormFieldFor(m => m.Event.Zip) %>
		

		<h2>Optional</h2>

        <p>Other bands playing that are not part of OpenGrooves. </p>

		<%: Html.FormFieldFor(m => m.Event.OtherBands) %>
		

		<div class="form-buttons">
			<input type="submit" value="Update Event" />
		</div>
	<% } %>

	<h2>Upload Avatar</h2>
	<%: Html.AvatarImage(Model.Event.AvatarUrl) %>
	<% using (Html.BeginForm("uploadavatar", "myevents", new { eventName = Model.Event.UrlName }, FormMethod.Post, new { enctype = "multipart/form-data" })) { %>
		<div>
			<input type="file" name="avatar" />
		</div>
		<div class="form-buttons"><input type="submit" value="Upload Avatar" /></div>
	<% } %>

	<h2>Bands Playing</h2>

	<ul class="list">
	<% if (!Model.Event.Bands.Any()) { %><li class="empty">No bands here</li><% } %>
	<% Model.Event.Bands.OrderBy(b => !b.IsActive).OrderBy(b => b.ShowTime).OrderBy(b => b.Order).ToList().ForEach(f => { %>
		<li>
			<div class="col">
				<%: Html.RouteLink(f.Band.Name, "Band", new { action = "profile", name = f.Band.UrlName }) %>
				<small>
					<% if (f.ShowTime.HasValue || f.Order.HasValue) { %>
						Going on <%: f.ShowTime.HasValue ? String.Format("at {0}", ((DateTime)f.ShowTime).PrettyTime()) : ((int)f.Order).AddOrdinal()%> - 
					<% } %>
					<strong><%: Html.RouteLink("Edit Showtime", "MyEvents", new { action = "editband", relationId = f.RelationId, eventName = Model.Event.UrlName }, new { @class = "fancy" })%></strong>
				</small>
			</div>
			<div class="col right">
				<% using (Html.BeginForm(new { action = "removeband", eventName = Model.Event.UrlName })) { %>
				<%: Html.Hidden("relationId", f.RelationId) %>
				<%: Html.AntiForgeryToken() %>
				<a href="#" class="submit-button"><%: f.IsActive ? "Remove" : "Cancel Request" %></a>
				<% }%>  
			</div>
			<div class="clear"></div>
		</li>
	<% }); %>
	</ul>

	<h2><%: Html.DisplayNameFor(m => m.MyBands) %></h2>

	<ul class="list">
	<% if (!Model.MyBands.Any()) { %><li class="empty">No bands here</li><% } %>
	<% Model.MyBands.ToList().ForEach(f => { %>
		<li>
			<div class="col"><%: Html.RouteLink(f.Name, "Band", new { action = "profile", name = f.UrlName }) %></div>
			<div class="col right">
			<% using (Html.BeginForm(new { action = "addband", eventName = Model.Event.UrlName })) { %>
				<%: Html.Hidden("bandId", f.BandId)%>
				<%: Html.AntiForgeryToken() %>
				<a href="#" class="submit-button">Add To Event</a>
			<% } %>
			</div>
			<div class="clear"></div>
		</li>
	<% }); %>
	</ul>

	<h2>Add Other Bands</h2>
	
	<p class="callout">
		You may add bands that you don't belong to. However, they would need to approve the action before being added to your event.
	</p>

	<%: Html.LetterBrowser() %>

	<ul class="list">
	<% if (!Model.OtherBands.Any()) { %><li class="empty">No bands here</li><% } %>
	<% Model.OtherBands.ToList().ForEach(f => { %>
		<li>
			<div class="col"><%: Html.RouteLink(f.Name, "Band", new { action = "profile", name = f.UrlName }) %></div>
			<div class="col right">
			<% using (Html.BeginForm(new { action = "requestband", eventName = Model.Event.UrlName })) { %>
				<%: Html.Hidden("bandid", f.BandId)%>
				<a href="#" class="submit-button">Sent Invite</a>
			<% } %>
			</div>
			<div class="clear"></div>
		</li>
	<% }); %>
	</ul>

	<h2>Delete Event</h2>
	<div class="callout">
		This will remove the event from the system. It cannot be undone.
	</div>

	<% using(Html.BeginForm(new { action = "deleteevent", eventName = Model.Event.UrlName })) {%>
		<%: Html.AntiForgeryToken() %>
		<div class="form-buttons">
			<input type="submit" class="confirm" value="Delete Event" />
		</div>
	<% } %>

</asp:Content>
