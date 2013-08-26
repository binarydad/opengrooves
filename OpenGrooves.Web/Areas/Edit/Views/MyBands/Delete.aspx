<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Edit/Views/Shared/BandProfile.master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Delete</h1>

    <p class="callout">
        This will remove the band from the system. It cannot be undone.
    </p>

    <% using(Html.BeginForm(new { action = "deleteband", bandUrl = ViewData["bandUrl"] as string })) {%>
        <%: Html.AntiForgeryToken() %>
        <div class="form-buttons">
            <input type="submit" class="confirm" value="Delete Band" />
        </div>
    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
