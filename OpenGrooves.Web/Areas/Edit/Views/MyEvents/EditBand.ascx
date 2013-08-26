<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BandEventRelation>" %>

<h1><%: Model.Band.Name%></h1>

<% using (Html.BeginRouteForm("MyEvents", new { action = "EditBand" })) { %>
<%: Html.HiddenFor(m => m.RelationId) %>
<%: Html.FormFieldFor(m => m.Order, new { @class = "single-digit" })%>

<p>OR, if you wish to specify a time, you may do so below.</p>
<%: Html.FormFieldFor(m => m.ShowTime)%>

<div class="form-buttons">
    <input type="submit" value="Save" />
</div>

<% } %>