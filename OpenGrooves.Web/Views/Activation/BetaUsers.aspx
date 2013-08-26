<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/OpenGrooves.Master" Inherits="System.Web.Mvc.ViewPage<BetaActivate>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Welcome Beta Testers!
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>One last step...</h1>

    <p>
        Thank you again for being part of our crack team of beta testers. 
        Before proceeding, your account for <strong><%: Model.Email %></strong> still needs a couple things.
    </p>

    <p>Please <strong>choose your username</strong> and <strong>create a password</strong> below to activate your acount.</p>
    
    <%: Html.ValidationSummary() %>

    <% using (Html.BeginRouteForm("activation", new { action = "setupaccount" })) { %>

        <%: Html.FormFieldFor(m => m.Username)%>
        <%: Html.FormFieldFor(m => m.NewPassword) %>
        
        <div class="form-buttons">
            <input type="submit" class="confirm" value="Activate My Account" />
        </div>

    <% } %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
