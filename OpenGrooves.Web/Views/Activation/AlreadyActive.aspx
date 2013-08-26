<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/OpenGrooves.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Account Already Activated
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>This account has already been activated</h1>

    <p>It appears that this account has already been activated. For security reasons, we cannot display your username here. </p>
    <p>If you are unable to log in with your username, please contact <a href="mailto:support@opengrooves.com">Support</a>. Thank you.</p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
