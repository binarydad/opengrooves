<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/OpenGrooves.Master" Inherits="System.Web.Mvc.ViewPage<OpenGrooves.Web.Models.SignupModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Signup
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1>You're getting close!</h1>

    <p class="info">To create an account, please complete the fields below. Once you're complete, you will receive an email containing a link to activate your account.</p>
    <p>Already have an account? <%: Html.RouteLink("Log in!", "login")%></p>

    <%: Html.ValidationSummary() %>
    
    <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm("signup", "authentication")) { %>
        
        <%: Html.FormFieldFor(m => m.Username) %>
        <%: Html.FormFieldFor(m => m.Password) %>
        <%: Html.FormFieldFor(m => m.PasswordConfirm) %>
        <%: Html.FormFieldFor(m => m.Email) %>
        <%: Html.FormFieldFor(m => m.RealName) %>
        <%: Html.FormFieldFor(m => m.City) %>
        <div>
            <%: Html.LabelFor(m => m.State) %>
            <%: Html.StateListFor(m => m.State) %>
        </div>
        
        <div class="form-buttons">
            <input type="submit" value="Sign Up" />
        </div>
    <% } %>

</asp:Content>
