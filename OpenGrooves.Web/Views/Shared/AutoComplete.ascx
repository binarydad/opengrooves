<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AutoCompleteModel>" %>

<script type="text/javascript">

    $(function () {

        $('<%: Model.JqueryFieldSelector %>').autocomplete({
            serviceUrl: '<%: Model.DataRequestUrl %>',
            minChars: 1,
            delimiter: /(,|;)\s*/,
            deferRequestBy: 0,
            onSelect: <%: Model.JavaScriptClickAction %>
        });
    });

</script>
