<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/OpenGrooves.Master" Inherits="System.Web.Mvc.ViewPage<string>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Account Active!
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>You're all set!</h1>

    Your account, <strong><%: Model %></strong>, has been activated!

    <br /><br />

    <%: Html.RouteLink("Continue...", "home", new { action = "home" }, new { @class = "bold" })%>

</asp:Content>
