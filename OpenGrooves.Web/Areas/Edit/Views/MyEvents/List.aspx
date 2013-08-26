<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Edit/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenGrooves.Web.Models.EventModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	View
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Manage Events</h1>

    <h2>Create An Event</h2>

    <p class="callout">
        Enter the name of the event below. You will be an owner of this event. 
    </p>

    <%: Html.ValidationSummary() %>

    <% using (Html.BeginForm(new { action = "create" })) { %>
    <%: Html.TextBox("eventName", null, new { @class = "large" })%>
    <%: Html.AntiForgeryToken() %>
    <div class="form-buttons">
        <input type="submit" value="Create Event" />
    </div>
    <% } %>

    <h2>My Events</h2>

    <%: Html.Partial("EventsList", Model.OrderBy(e => e.Date)) %>

</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="HeadContent">
<%: Html.Partial("AutoComplete", new AutoCompleteModel { DataRequestUrl = "/global/events", JavaScriptClickAction = "action", JqueryFieldSelector = "#eventName" })%>
<script type="text/javascript">
    var action = function (value, data) {
        document.location.href = '/event/' + data;
    }
</script>
</asp:Content>