using System.Web.Mvc;

namespace OpenGrooves.Web.Areas.Edit
{
    public class EditAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Edit";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("MyUser", "settings/profile/{action}", new { controller = "MyUser", action = "edit" });
            context.MapRoute("MyEvents", "settings/events/{action}/{eventName}", new { controller = "MyEvents", action = "list", eventName = UrlParameter.Optional });
            context.MapRoute("MyVenues", "settings/venues/{action}/{venueName}", new { controller = "MyVenues", action = "list", venueName = UrlParameter.Optional });
            context.MapRoute("MyImages", "settings/images/{action}/{galleryName}", new { controller = "MyImages", action = "list", galleryName = UrlParameter.Optional });
            context.MapRoute("MySettings", "settings/account", new { controller = "MySettings", action = "index" });
            context.MapRoute("Join", "settings/bands/join", new { controller = "MyBands", action = "join" });
            context.MapRoute("MyBands", "settings/bands/{action}/{bandId}", new { controller = "MyBands", action = "list", bandId = UrlParameter.Optional });
            context.MapRoute("Upload", "settings/upload/{action}", new { controller = "Upload", action = "Uploadify" });
        }
    }
}
