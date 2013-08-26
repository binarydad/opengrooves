<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/OpenGrooves.Master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Models.UserModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <% if (!Model.RealName.IsNullOrWhiteSpace()) {%>
        <%: Model.RealName%> (<%: Model.UserName%>)
    <% } else { %>
       <%: Model.UserName %>
    <% } %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1><%: Model.RealName ?? Model.UserName %></h1>

    <%-- avatar --%>
    <div class="user-profile-info">
        <%: Html.AvatarImage(Model.AvatarUrl, false) %>
    </div>

    <%-- basic profile info --%>
    <fieldset class="user-profile-info">
        <%-- Location --%>
        <div>
            <%: Html.Label("Location") %>
            <% if (Model.Location.HasLocation) { %>
            <%: Html.LocationSearchUrl(Model.City, Model.State, Model.Zip) %>
            <% } else { %>Unknown<% } %>
        </div>
        <% if (!Model.Facebook.IsNullOrWhiteSpace()) { %>
        <div>
            <%: Html.LabelFor(m => m.Facebook) %>
            <%: Html.FacebookUrl(Model.Facebook) %>
        </div>
        <% } %>

        <% if (!Model.Twitter.IsNullOrWhiteSpace()) { %>
        <div>
            <%: Html.LabelFor(m => m.Twitter)%>
            <%: Html.TwitterUrl(Model.Twitter)%>
        </div>
        <% } %>
        <%: Html.ReadOnlyFieldFor(m => m.AIM) %>
        <%: Html.ReadOnlyFieldFor(m => m.Interests) %>
    </fieldset>

    <div class="clear"></div>

    <% if (!Model.Bio.IsNullOrWhiteSpace()) { %>
    <h2>About <%: Model.RealName ?? Model.UserName %></h2>
    <%= Model.Bio.NL2BR() %>
    <% } %>

    <h2>Member Of</h2>
    <div id="bands-list"></div>
    <script type="text/javascript">
        OpenGrooves.AjaxAction.Render('bandslist', 'userajax', { userId: '<%: Model.UserId %>' }, '#bands-list');
    </script>
    
    <h2>Following Bands</h2>
    <div id="following"></div>
    <script type="text/javascript">
        OpenGrooves.AjaxAction.Render('following', 'userajax', { userId: '<%: Model.UserId %>' }, '#following');
    </script>

    <h2>User Photos</h2>
    <div id="images"></div>
    <script type="text/javascript">
        OpenGrooves.AjaxAction.Render('images', 'userajax', { userId: '<%: Model.UserId %>' }, '#images');
    </script>

    <h2>Galleries</h2>
    <div id="galleries"></div>
    <script type="text/javascript">
        OpenGrooves.AjaxAction.Render('galleries', 'userajax', { userId: '<%: Model.UserId %>' }, '#galleries');
    </script>

</asp:Content>
