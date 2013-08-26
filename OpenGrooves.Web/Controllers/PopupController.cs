using System.Web.Mvc;

namespace OpenGrooves.Web.Controllers
{
    public class PopupController : BaseController
    {
        [HttpPost]
        public ActionResult UserInfo(string data)
        {
            var user = DataRepository.GetUser(data);
            user.MemberOfBands = DataRepository.GetBandRelationsByUser(user.UserId, 1, 3);
            return View(user);
        }

        [HttpPost]
        public ActionResult BandInfo(string data)
        {
            var band = DataRepository.GetBand(data);
            band.NumFans = DataRepository.GetTotalFans(band.BandId);
            band.ActiveMembers = DataRepository.GetUserRelationsByBand(band.BandId, 1, 10);
            return View(band);
        }

        [HttpPost]
        public ActionResult EventInfo(string data)
        {
            var ev = DataRepository.GetEvent(data);
            return View(ev);
        }
    }
}
