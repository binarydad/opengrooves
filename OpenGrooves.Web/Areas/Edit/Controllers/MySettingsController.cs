using OpenGrooves.Web.Areas.Edit.Models;
using OpenGrooves.Web.Controllers;
using System.Web.Mvc;

namespace OpenGrooves.Web.Areas.Edit.Controllers
{
    public class MySettingsController : BaseController
    {
        public ActionResult Index()
        {
            var settings = new UserSettings();

            settings.NotifyNewMessage = UserSettings.Get<bool>("NotifyNewMessage");
            settings.NotifyBandPhotos = UserSettings.Get<bool>("NotifyBandPhotos");
            settings.NotifyEventUpdated = UserSettings.Get<bool>("NotifyEventUpdated");
            settings.NotifyBandEvent = UserSettings.Get<bool>("NotifyBandEvent");
            settings.NotifyBandProfileUpdate = UserSettings.Get<bool>("NotifyBandProfileUpdate");
            settings.NotifyEventBandsAdded = UserSettings.Get<bool>("NotifyEventBandsAdded");

            return View(settings);
        }

        [HttpPost]
        [ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult IndexPost(UserSettings settings)
        {
            UserSettings.Save("NotifyNewMessage", settings.NotifyNewMessage.ToString());
            UserSettings.Save("NotifyBandPhotos", settings.NotifyBandPhotos.ToString());
            UserSettings.Save("NotifyEventUpdated", settings.NotifyEventUpdated.ToString());
            UserSettings.Save("NotifyBandEvent", settings.NotifyBandEvent.ToString());
            UserSettings.Save("NotifyBandProfileUpdate", settings.NotifyBandProfileUpdate.ToString());
            UserSettings.Save("NotifyEventBandsAdded", settings.NotifyEventBandsAdded.ToString());
            UserSettings.SaveToProvider();

            return JsonValidationResult(ModelState);
        }
    }
}
