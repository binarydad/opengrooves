using System;
using System.Linq;
using System.Web.Mvc;

namespace OpenGrooves.Web.Controllers
{
    public class UserController : BaseController
    {
        public ActionResult Profile(string username)
        {
            var user = DataRepository.GetUser(username);
            return View(user);
        }

        public ActionResult BandsList(Guid userId)
        {
            return View("BandsList", DataRepository.GetBandRelationsByUser(userId, 1).Select(u => u.Band));
        }

        public ActionResult Following(Guid userId)
        {
            return View("BandsList", DataRepository.GetBandRelationsByUser(userId, 4).Select(u => u.Band));
        }

        public ActionResult Images(Guid userId)
        {
            return View("ImageGallery", DataRepository.GetImagesByUser(userId, true));
        }

        public ActionResult Galleries(Guid userId)
        {
            return View("GalleryList", DataRepository.GetGalleriesByUser(userId));
        }
    }
}
