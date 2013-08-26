<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/OpenGrooves.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Credits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Credits</h1>

    <p>We don't have fancy titles. We're just a couple o' boys from Charm City with an idea and some code.</p>

    <p>
        <a href="/user/rpeters">Ryan Peters</a> - Developer<br />
        <a href="/user/jcrawford">Jason Crawford</a> - Marketing
    </p>

    <h2>Special Thanks</h2>
    <ul>
        <li><strong>Kirk Lutz</strong> of <a target="_blank" href="http://redheadcompanies.com">Redhead Companies</a> for the logo design.</li>
    </ul>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
