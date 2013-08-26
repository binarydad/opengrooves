using OpenGrooves.Core;
using OpenGrooves.Core.Extensions;
using OpenGrooves.Web.Areas.Edit.Models;
using OpenGrooves.Web.Controllers;
using OpenGrooves.Web.Extensions;
using OpenGrooves.Web.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace OpenGrooves.Web.Areas.Edit.Controllers
{
    public class UploadController : BaseController
    {
        [ChildActionOnly]
        public ActionResult Uploadify(string uploadUrl, string callbackJs)
        {
            var model = new UploadifyModel
            {
                UploadPath = uploadUrl,
                CallbackJS = callbackJs
            };

            return View(model);
        }

        #region Upload Images
        [HttpPost]
        public ActionResult UploadImage(string bandUrl, string galleryName, Guid batchId)
        {
            var file = Request.Files[0];
            var filename = HttpContext.SaveImage(file);

            var image = new ImageModel
            {
                ImageId = Guid.NewGuid(),
                Url = filename,
                Caption = file.FileName,
                BatchId = batchId
            };

            // if for a gallery, add that image to the gallery
            if (!String.IsNullOrEmpty(galleryName))
            {
                Guid galleryId = DataRepository.GetGallery(galleryName).GalleryId;
                image.GalleryId = galleryId;
            }

            DataRepository.AddImage(image, loggedUserGuid);

            // if for a band, associate image with band
            if (!bandUrl.IsNullOrWhiteSpace())
            {
                var bandId = DataRepository.GetBandId(bandUrl);
                if (bandId != null && DataRepository.UserIsMemberOfBand(loggedUserGuid, (Guid)bandId))
                {
                    DataRepository.AddBandImage(image.ImageId, (Guid)bandId);
                }
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult UploadImageBatchComplete(Guid batchId, Guid bandId)
        {
            var band = DataRepository.GetBand(bandId, loadExtended: false);
            DataRepository.AddFeedItem("{0} has uploaded new photos.", 7, band.BandId, batchId: batchId);
            Notifications.SendBandNotifications(NotificationType.NotifyBandPhotos, band.BandId, new BandMessageData { Band = band.Name, Link = band.UrlName });

            return Json(new { success = true });
        } 
        #endregion

        #region Upload Audio
        
        public ActionResult UploadAudio(Guid bandId)
        {
            var file = Request.Files[0];
            var filename = HttpContext.SaveAudio(file);

            var audio = new AudioModel
            {
                AudioID = Guid.NewGuid(),
                Title = filename,
                BandID = bandId,
                Url = filename
            };

            DataRepository.AddAudio(audio);

            return Json(new { success = true });
        }

        #endregion
    }
}
