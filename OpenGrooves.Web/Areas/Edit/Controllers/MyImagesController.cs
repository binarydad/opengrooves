using OpenGrooves.Core.Extensions;
using OpenGrooves.Core.Helpers;
using OpenGrooves.Web.Areas.Edit.Models;
using OpenGrooves.Web.Controllers;
using OpenGrooves.Web.Models;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace OpenGrooves.Web.Areas.Edit.Controllers
{
    [Authorize]
    public class MyImagesController : BaseController
    {
        private EditImagesModel CreateModel()
        {
            var model = new EditImagesModel();

            model.Images = DataRepository.GetImagesByUser(loggedUserGuid, true);
            model.Galleries = DataRepository.GetGalleriesByUser(loggedUserGuid);

            ViewData["galleryList"] = model.Galleries;

            return model;
        }

        public ActionResult List()
        {
            var model = CreateModel();
            return View(model);
        }

        public ActionResult Gallery(string galleryName)
        {
            var gallery = DataRepository.GetGallery(galleryName);
            ViewData["galleryList"] = DataRepository.GetGalleriesByUser(loggedUserGuid);
            ViewData["myBands"] = DataRepository.GetBandRelationsByUser(loggedUserGuid, 1).Select(b => b.Band);
            return View(gallery);
        }

        [HttpPost]
        [ActionName("Gallery")]
        public ActionResult GalleryPost(string galleryName, GalleryModel m)
        {
            var model = DataRepository.GetGallery(galleryName);

            if (ModelState.IsValid)
            {
                var oldUrl = model.UrlName;
                var newUrl = EntityNameHelper.CreateUrl(m.Name);

                model.UrlName = newUrl;
                model.Description = m.Description;
                model.Name = m.Name;
                model.BandId = m.BandId;

                newUrl = DataRepository.UpdateGallery(model);

                if (oldUrl != newUrl)
                {
                    return Json(new { success = true, redirect = Url.RouteUrl("myimages", new { action = "gallery", galleryName = newUrl }) });
                }
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult MoveImage(Guid imageId, Guid? galleryId)
        {
            if (galleryId != null)
            {
                DataRepository.UpdateImageGallery(imageId, (Guid)galleryId);
            }
            else
            {
                DataRepository.DeleteImageGallery(imageId);
            }
            
            return JsonValidationResult(ModelState);
        }

        [HttpPost]
        public ActionResult DeleteImage(Guid imageId)
        {
            var image = DataRepository.GetImage(imageId);

            if (image != null)
            {
                var myBands = DataRepository.GetBandRelationsByUser(loggedUserGuid, 1);

                if (myBands.Any())
                {
                    bool belongsToMyBands = myBands.Select(b => b.BandId).Intersect(image.Bands.Select(b => b.BandId)).Any();

                    if (image.UserId == loggedUserGuid || belongsToMyBands)
                    {
                        System.IO.File.Delete(Server.MapPath(Path.Combine(Config.GetSetting<string>("LargeImagePath"), image.Url)));
                        System.IO.File.Delete(Server.MapPath(Path.Combine(Config.GetSetting<string>("ThumbPath"), image.Url)));

                        DataRepository.DeleteImage(imageId);

                        return Json(new { success = true });
                    }
                }
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult CreateGallery(string newGalleryName, string bandUrl)
        {
            if (newGalleryName.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("NoGalleryName", "Please enter a gallery name");
                var model = CreateModel();
                
                return View("List", CreateModel());
            }

            Guid? bandId = null;
            if (!bandUrl.IsNullOrWhiteSpace())
            {
                bandId = DataRepository.GetBandId(bandUrl);
            }

            var url = Core.Helpers.EntityNameHelper.CreateUrl(newGalleryName);
            var redirect = DataRepository.CreateGallery(newGalleryName, url, loggedUserGuid, bandId);

            return RedirectToRoute("myimages", new { action = "gallery", galleryName = redirect });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGallery(Guid galleryId)
        {
            DataRepository.DeleteGallery(galleryId);
            return RedirectToRoute("myimages", new { action = "list" });
        }
    }
}
