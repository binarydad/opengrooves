using System;
using System.Linq;
using System.Web.Mvc;

namespace OpenGrooves.Web.Controllers
{
    public class GlobalController : BaseController
    {
        public ActionResult Users(string query)
        {
            var users = DataRepository.FindUsersStartingWith(query);

            return Json(new
            {
                query = query,
                suggestions = users.Select(u => String.Format("{0} ({1})", u.UserName, u.RealName)),
                data = users.Select(u => u.UserName.ToLower())
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Bands(string query)
        {
            var bands = DataRepository.FindBandsStartingWith(query);

            return Json(new
            {
                query = query,
                suggestions = bands.Select(b => 
                    {
                        if (b.Location.HasLocation)
                        {
                            return String.Format("{0} ({1}, {2})", b.Name, b.City, b.State);
                        }

                        return b.Name;
                    }),
                data = bands.Select(b => b.UrlName)
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Events(string query)
        {
            var events = DataRepository.FindEventsStartingWith(query);

            return Json(new
            {
                query = query,
                suggestions = events.Select(e => String.Format("{0} ({1:dddd, M/d} @ {1:h:mm tt})", e.Name, e.Date)),
                data = events.Select(e => e.UrlName)
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
