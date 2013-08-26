<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/OpenGrooves.Master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Models.LoginModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Login
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Login</h1>

    <p class="info">The section you are trying to acccess requires a membership to continue.</p>
    <p>Don't have an account? <strong><%: Html.RouteLink("Sign up!", "signup")%></strong>.</p>

    <%: Html.Partial("LoginControl") %>

    <p>
        If you have having trouble logging in, please contact <a href="mailto:support@opengrooves.com">Support</a> at this time.
    </p>

</asp:Content>
