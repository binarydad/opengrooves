using OpenGrooves.Web.Models;
using System.Web.Mvc;

namespace OpenGrooves.Web.Controllers
{
    public class DefaultController : BaseController
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToRoute("home", new { action = "home" });
            }

            var model = CreateModel();

            return View(model);
        }

        private DefaultModel CreateModel()
        {
            var model = new DefaultModel();

            model.NewBands = DataRepository.GetNewestBands(12);

            return model;
        }
    }
}
