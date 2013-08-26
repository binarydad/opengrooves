using OpenGrooves.Core;
using StructureMap;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("robots.txt");

            routes.MapRoute("User", "user/{username}", new { controller = "User", action = "profile" });
            routes.MapRoute("UserAjax", "userajax/{action}", new { controller = "User", action = "profile" });
            routes.MapRoute("Browse", "browse/{action}/{filter}", new { controller = "Browse", action = "nearby", filter = UrlParameter.Optional });
            routes.MapRoute("Event", "event/{name}", new { controller = "Event", action = "index" });
            routes.MapRoute("EventAjax", "eventajax/{action}", new { controller = "Event", action = "index" });
            routes.MapRoute("Genre", "genre/{name}", new { controller = "Genre", action = "index" });
            routes.MapRoute("Activation", "activate/{keyEncoded}/{action}", new { controller = "Activation", action = "index" });
            routes.MapRoute("Login", "login", new { controller = "Authentication", action = "login" });
            routes.MapRoute("Logout", "logout", new { controller = "Authentication", action = "logout" });
            routes.MapRoute("Signup", "signup", new { controller = "Authentication", action = "signup" });
            routes.MapRoute("Gallery", "gallery/{galleryName}", new { controller = "Gallery", action = "view", galleryName = UrlParameter.Optional });
            routes.MapRoute("GalleryAjax", "galleryajax/{action}", new { controller = "Gallery", action = "recentphotos" });
            routes.MapRoute("Messages", "messages/{action}/{messageId}", new { controller = "Messages", action = "received", messageId = UrlParameter.Optional });
            routes.MapRoute("Home", "home/{action}", new { controller = "Home", action = "home" });
            routes.MapRoute("Global", "global/{action}/{query}", new { controller = "Global", query = UrlParameter.Optional });
            routes.MapRoute("Popup", "popup/{action}", new { controller = "Popup", action = "userinfo" });
            routes.MapRoute("Static", "about/{page}", new { controller = "Static", action = "staticpage", page = "about" });
            routes.MapRoute("Docs", "docs/{page}", new { controller = "Static", action = "staticpage", page = "about" });
            routes.MapRoute("Band", "{name}/{action}", new { controller = "Band", action = "profile" });
            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Default", action = "index", id = UrlParameter.Optional });
            
        }

        protected void Application_BeginRequest()
        {
            // for uploadify, as flash doesn't pass in cookies for requests
            string token = Request["token"];

            if (token != null && !String.IsNullOrWhiteSpace(token))
            {
                var cookie = new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName) { Value = token };
                Request.Cookies.Add(cookie);
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);

            ObjectFactory.Initialize(x => x.Scan(y => y.AssembliesFromApplicationBaseDirectory()));
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());
            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());
        }
    }
}
