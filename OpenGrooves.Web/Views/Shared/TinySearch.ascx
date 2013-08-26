<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BrowseModel>" %>

<form class="tiny-search" method="post">

    <% var radii = new int[] { 1, 2, 3, 5, 10, 15, 25, 50, 100, 200 }; %>
    <select name="radius">
        <% radii.ToList().ForEach(r =>
           { %><option value="<%: r %>" <%: r == Model.Radius ? "selected=\"selected\"" : "" %>><%: r%> miles</option><% }); %>
    </select>

    <strong>from</strong>

    <input type="text" name="address" value="<%: Model.Address %>" class="watermark" data-watermark="City and state, or zip" />

    <input type="submit" value="Search" />
</form>

<div class="clear"></div>