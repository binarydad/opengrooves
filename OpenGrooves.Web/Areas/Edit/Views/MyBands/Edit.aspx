<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Edit/Views/Shared/BandProfile.Master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Models.BandModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="HeadContent">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%-- Pending requests --%>
    <%: Html.Partial("PendingRequests", Model) %>

    <% Html.EnableClientValidation(); %>
    <% using (Ajax.BeginForm(new AjaxOptions { OnComplete = "OpenGrooves.AjaxForms.OnComplete", OnBegin = "OpenGrooves.AjaxForms.OnBegin" })) { %>
        <%: Html.AntiForgeryToken()%>



        <h2>Required Info</h2>

        <%: Html.FormFieldFor(m => m.Name)%>
        <%: Html.FormFieldFor(m => m.City)%>
        <div>
            <%: Html.LabelFor(m => m.State, true)%>
            <%: Html.StateListFor(m => m.State)%>
        </div>

        <h2>Optional</h2>

        <%: Html.FormFieldFor(m => m.Description)%>
        <%: Html.FormFieldFor(m => m.Facebook)%>
        <%: Html.FormFieldFor(m => m.Twitter)%>
        <%: Html.FormFieldFor(m => m.Website)%>
        
        <%: Html.FormFieldFor(m => m.Zip)%>
        <% var allGenres = ViewData["allGenres"] as List<GenreModel>; %>
        <div>
            <%: Html.LabelFor(m => m.Genres)%>
            <% allGenres.ForEach(g =>
               { %>
                <% var isChecked = Model.Genres.Select(t => t.GenreId).Contains(g.GenreId); %>
                <%: Html.CheckBox("genres", isChecked, new { value = g.GenreId })%> <%: g.Name%>
            <% }); %>
        </div>

        <div class="form-buttons">
            <input type="submit" value="Save Band" />
        </div>
    <% } %>

    
    <h2>Upload Avatar</h2>
    <%: Html.AvatarImage(Model.AvatarUrl) %>
    <% using (Html.BeginForm("uploadavatar", "mybands", new { bandUrl = Model.UrlName }, FormMethod.Post, new { enctype = "multipart/form-data" })) { %>
        <div>
            <input type="file" name="avatar" />
        </div>
        <div class="form-buttons"><input type="submit" value="Upload Avatar" /></div>
    <% } %>

    <h2>Delete</h2>

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
