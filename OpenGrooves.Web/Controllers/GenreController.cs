using System.Web.Mvc;

namespace OpenGrooves.Web.Controllers
{
    public class GenreController : BaseController
    {
        public ActionResult Index(string name)
        {
            ViewData["genre-name"] = name;
            var bands = DataRepository.GetBandsByGenre(name);

            return View(bands);
        }

    }
}
