<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Edit/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Areas.Edit.Models.UserSettings>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	User Settings
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>User Settings</h1>

    <% using (Ajax.BeginForm(null, null, new AjaxOptions { OnComplete = "OpenGrooves.AjaxForms.OnComplete", OnBegin = "OpenGrooves.AjaxForms.OnBegin" }, new { @class = "checklist" })) { %>
        <%: Html.AntiForgeryToken() %>
        <%: Html.FormFieldFor(m => m.NotifyNewMessage)%>

        <h2>Notify me whenever a band...</h2>

        <%: Html.FormFieldFor(m => m.NotifyBandPhotos)%>
        <%: Html.FormFieldFor(m => m.NotifyEventUpdated)%>
        <%: Html.FormFieldFor(m => m.NotifyBandEvent)%>
        <%: Html.FormFieldFor(m => m.NotifyBandProfileUpdate)%>
        <%: Html.FormFieldFor(m => m.NotifyEventBandsAdded) %>

        <div class="form-buttons">
            <input type="submit" value="Save Settings" />
        </div>
    <% } %>

</asp:Content>