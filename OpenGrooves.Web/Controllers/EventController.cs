using System.Linq;
using System.Web.Mvc;

namespace OpenGrooves.Web.Controllers
{
    public class EventController : BaseController
    {
        public ActionResult Index(string name)
        {
            ViewData["userLocation"] = DataRepository.GetUserLocation(loggedUserGuid);
            var ev = DataRepository.GetEvent(name, loadExtended: false);
            ev.Bands = DataRepository.GetBandRelationsByEvent(ev.EventId).Where(e => e.IsActive == true);
            return View(ev);
        }
    }
}
