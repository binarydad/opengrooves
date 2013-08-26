<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Edit/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<OpenGrooves.Web.Models.BandModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Join
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Join a Band</h1>

    <h2>Join Existing Band</h2>

    <p class="callout">
        If you are a member of a band listed here, click <strong>JOIN</strong> next to the name of the band. A request will be sent to the administrator of that band's profile to join that band.    
    </p>

    <%: Html.LetterBrowser() %>
    
    <ul class="list">
        <% if (Model == null || !Model.Any()) { %><li class="empty">No bands here</li><% } else { 
               Model.ToList().ForEach(b => {    
                   %>
        <li>
            <div class="col"><%: b.Name %></div>
            <div class="col right">
            <% using (Html.BeginForm()) { %>
                <%: Html.Hidden("band", b.BandId) %>
                <%: Html.AntiForgeryToken() %>
                <a href="#" class="submit-button">Send Request to Join</a>
            <% } %>
            </div>
            <div class="clear"></div>
        </li>
    <% }); %>
    </ul>
    <% } %>
    <br /><br />

    <h2>Create Band</h2>

    <p class="callout">
        If the band that you wish to join is not in the live above, please create that band by entering the name below.
    </p>
    <%: Html.ValidationSummary() %>
    <% using (Html.BeginForm(new { action = "createband" })) { %>
        <%: Html.AntiForgeryToken() %>
        <%: Html.TextBox("bandName", null, new { @class = "large" })%>
        <div class="form-buttons">
            <input type="submit" value="Create Band" />
        </div>
    <% } %>

</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="HeadContent">
<%: Html.Partial("AutoComplete", new AutoCompleteModel { DataRequestUrl = "/global/bands", JavaScriptClickAction = "OpenGrooves.AutoCompleteActions.DataRedirect", JqueryFieldSelector = "#bandName" })%>
</asp:Content>