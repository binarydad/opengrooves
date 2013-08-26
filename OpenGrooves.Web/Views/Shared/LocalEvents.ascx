<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OpenGrooves.Web.Models.BrowseModel>" %>

<%: Html.Partial("EmptyLocation", Model) %>

<%: Html.Partial("TinySearch") %>

<% if (Model.Events != null) { %>
<%: Html.Partial("EventsList", Model.Events) %>
<% } %>