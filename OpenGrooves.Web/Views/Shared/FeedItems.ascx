<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<FeedItemModel>>" %>

<ul class="list feed">
<% Model.ToList().ForEach(f => { %>
    <%
        string data = String.Empty;
        string icon = String.Empty;
           
        // BAND
        if (f.FeedItemTypeId == 2)
        {
            icon = "group-edit.png";
            data = String.Format(f.Content, Html.BandProfileUrl(f.Band.UrlName, f.Band.Name));   
        }
        // EVENT
        else if (f.FeedItemTypeId == 3)
        {
            icon = "event.png";
            var link = Html.EventUrl(f.Event.UrlName, f.Event.Name);
            data = String.Format(f.Content, link);
        }
        // FOLLOW
        else if (f.FeedItemTypeId == 4)
        {
            icon = "follow.png";
            var userLink = Html.UserProfileUrl(f.User.UserName, f.User.RealName ?? f.User.UserName);
            var bandLink = Html.BandProfileUrl(f.Band.UrlName, f.Band.Name);
            data = String.Format(f.Content, userLink, bandLink);
        }
        // EVENT_BAND
        else if (f.FeedItemTypeId == 5)
        {
            icon = "event-add.png";
            var eventLink = Html.EventUrl(f.Event.UrlName, f.Event.Name);
            var bandLink = Html.BandProfileUrl(f.Band.UrlName, f.Band.Name);
            data = String.Format(f.Content, bandLink, eventLink);
        }
        // ANNOUNCEMENT
        else if (f.FeedItemTypeId == 6)
        {
            icon = "comment.png";
            data = String.Format(f.Content, Html.BandProfileUrl(f.Band.UrlName, f.Band.Name));   
        }
        // IMAGES
        else if (f.FeedItemTypeId == 7)
        {
            icon = "images.png";
            data = String.Format(f.Content, Html.BandProfileUrl(f.Band.UrlName, f.Band.Name));

            if (f.Images != null && f.Images.Any())
            {
                data += String.Format("<div class=\"gallery\">{0}</div>", String.Join("", f.Images.ToList().Select(i => "<a rel=\"gallery\" href=\"/uploads/images/band/" + Html.AttributeEncode(i.Url) + "\"><img src=\"/uploads/images/thumbs/" + Html.AttributeEncode(i.Url) + "\" alt=\"" + Html.AttributeEncode(i.Caption) + "\" /></a>")));
            }
        }
        else
        {
            data = f.Content;
        }
    %>
    <li>
        <div class="col">
            <% if (!icon.IsNullOrWhiteSpace()) { %>
                <img class="icon" src="/content/images/icons/<%: icon %>" />
            <% } %>
            <%= data %>
        </div>
        <div class="col right"><%: Html.FeedAge(f.Date)%></div>
        <div class="clear"></div>
    </li>
<% }); %>
</ul>