<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/OpenGrooves.Master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Models.SignupModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Login
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Awesome...</h1>

    Your account has been created! <strong>Check your email for activation link.</strong> This link was sent to: <strong><%: Model.Email %></strong>.

</asp:Content>