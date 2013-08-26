<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UserModel>" %>

<%: Html.AvatarImage(Model.AvatarUrl)%>
<h3><%: Model.RealName%></h3>
<%: OpenGrooves.Core.Helpers.LocationHelper.CityStateOrZip(Model.City, Model.State, Model.Zip) %>

<% if (Model.MemberOfBands.Any()) { %>
<h4>Member Of:</h4>
<% Model.MemberOfBands.ToList().ForEach(b => { %>
    <%: b.Band.Name%><br />
<% }); }%>