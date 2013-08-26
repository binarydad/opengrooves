using System.Web.Mvc;

namespace OpenGrooves.Web.Controllers
{
    public class GalleryController : BaseController
    {
        [ActionName("View")]
        public ActionResult ViewGallery(string galleryName)
        {
            var gallery = DataRepository.GetGallery(galleryName);
            return base.View(gallery);
        }

        public ActionResult RecentPhotos()
        {
            var images = DataRepository.GetRecentBandImages(loggedUserGuid, 10);
            return View(images);
        }
    }
}
