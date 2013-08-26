<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Edit/Views/Shared/BandProfile.master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<FeedItemModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Announcements
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Make Announcement</h2>
    
    <% using (Ajax.BeginForm(new AjaxOptions { OnComplete = "OpenGrooves.AjaxForms.OnComplete", OnBegin = "OpenGrooves.AjaxForms.OnBegin" })) { %>
        <%: Html.AntiForgeryToken() %>
        <%: Html.TextBox("announcement", null, new { @class = "large" })%>
        <div class="form-buttons">
            <input type="submit" value="Make Announcement" />
        </div>
    <% } %>

    <h2>Recent Announcements</h2>

    <ul class="list feed">
    <% Model.ToList().ForEach(f => { %>
        <li>
            <div class="col">
                <%= String.Format(f.Content, Html.BandProfileUrl(f.Band.UrlName, f.Band.Name)) %>
                <small><%: Html.FeedAge(f.Date)%></small>
            </div>
            <div class="col right">
                <% using(Html.BeginRouteForm("mybands", new { action = "deleteannouncement", bandUrl = ViewData["bandUrl"] as string })) { %>
                    <%: Html.AntiForgeryToken() %>
                    <%: Html.Hidden("feedItemId", f.FeedItemId) %>
                    <a href="#" class="submit-button delete confirm"></a>
                <% } %>
            </div>
            <div class="clear"></div>
        </li>
    <% }); %>
    </ul>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
