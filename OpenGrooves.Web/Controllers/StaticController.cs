using System.Web.Mvc;

namespace OpenGrooves.Web.Controllers
{
    public class StaticController : Controller
    {
        public ActionResult StaticPage(string page)
        {
            return View(page);
        }

    }
}
