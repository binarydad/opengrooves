using OpenGrooves.Services.Caching;
using OpenGrooves.Services.Configuration;
using OpenGrooves.Services.Mapping;
using OpenGrooves.Services.Notifications;
using OpenGrooves.Web.Proxies;
using StructureMap.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace OpenGrooves.Web.Controllers
{
    public class BaseController : Controller
    {
        #region Services
        [SetterProperty]
        public IRepository DataRepository { get; set; }

        [SetterProperty]
        public ILocationService Location { get; set; }

        [SetterProperty]
        public IConfig Config { get; set; }

        [SetterProperty]
        public EmailNotificationServiceBase Notifications { get; set; }

        [SetterProperty]
        public ICacheService UserSettings { get; set; } 
        #endregion

        protected MembershipUser loggedUser;
        protected Guid loggedUserGuid;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            if (requestContext.HttpContext.Request.IsAuthenticated)
            {
                loggedUser = Membership.GetUser();

                if (loggedUser == null)
                {
                    FormsAuthentication.SignOut();
                    FormsAuthentication.RedirectToLoginPage();
                }

                loggedUserGuid = (Guid)loggedUser.ProviderUserKey;

                ViewData["loggedUserGuid"] = loggedUserGuid;

                // cache provider
                UserSettings.DataProvider = new UserSettingsCacheDataProvider(loggedUserGuid);
            }

            base.Initialize(requestContext);
        }

        protected JsonResult JsonValidationResult(ModelStateDictionary state, string redirectUrl = null)
        {
            if (state.IsValid)
            {
                return Json(new { success = true, redirect = redirectUrl });
            }
            else
            {
                var errors = new List<string>();

                ModelState.ToList().ForEach(s =>
                {
                    if (s.Value.Errors.Any())
                    {
                        s.Value.Errors.ToList().ForEach(e => errors.Add(e.ErrorMessage));
                    }
                });

                return Json(new { success = false, errors = errors });
            }
        }
    }
}
