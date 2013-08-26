<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OpenGrooves.Web.Models.LoginModel>" %>

<%: Html.ValidationSummary() %>
<% Html.EnableClientValidation(); %>
<% using (Html.BeginForm(new { controller = "authentication", action = "login" })) { %>
    <%: Html.Hidden("ReturnUrl", Request.QueryString["ReturnUrl"]) %>
    <%: Html.FormFieldFor(m => m.Username) %>
    <%: Html.FormFieldFor(m => m.Password) %>
    <div class="form-buttons">
        <input type="submit" value="Login" />
    </div>
<% } %>

<script type="text/javascript">

    (function () {
        $('#Username').focus();
    })();
        
</script>