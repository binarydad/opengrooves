<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/OpenGrooves.Master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Models.MessageModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	View
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>
        View Message
        <span class="list-actions">
            <%: Html.RouteLink("Compose Message", "Messages", new { action = "create" }) %>
        </span>
    </h1>

    From: 
    <%: Html.RouteLink(Model.Sender.UserName, "user", new { action = "profile", username = Model.Sender.UserName }) %>
    (<%: Html.RouteLink("Reply", "Messages", new { action = "create", to = Model.Sender.UserName }) %>)

    <br /><br />

    <%: Model.Content %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
