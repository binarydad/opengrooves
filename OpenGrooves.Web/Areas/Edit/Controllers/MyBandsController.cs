using OpenGrooves.Core;
using OpenGrooves.Core.Helpers;
using OpenGrooves.Web.Areas.Edit.Models;
using OpenGrooves.Web.Controllers;
using OpenGrooves.Web.Extensions;
using OpenGrooves.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenGrooves.Web.Areas.Edit.Controllers
{
    [Authorize]
    public class MyBandsController : BaseController
    {
        public ActionResult List()
        {
            var bands = DataRepository.GetBandRelationsByUser(loggedUserGuid, 1).Select(b => b.Band);

            return View(bands);
        }

        public ActionResult Edit(Guid bandId)
        {
            var model = DataRepository.GetBand(bandId);
            model.PendingMemberRequests = DataRepository.GetUserRelationsByBand(model.BandId, 3);
            ViewData["allGenres"] = DataRepository.GetGenres();

            if (!DataRepository.UserIsMemberOfBand(loggedUserGuid, model.BandId))
            {
                return RedirectToRoute("MyUser");
            }
            
            return View(model);
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(BandModel m, IEnumerable<string> genres, Guid bandId)
        {
            var model = DataRepository.GetBand(bandId);

            if (ModelState.IsValid)
            {
                var oldUrl = model.UrlName;
                var newUrl = EntityNameHelper.CreateUrl(m.Name);

                m.UrlName = newUrl;
                m.BandId = model.BandId;

                var selectedGenres = genres.Where(g => g != "false").Select(g => new Guid(g));

                DataRepository.UpdateBand(m);
                DataRepository.UpdateBandGenres(m.BandId, selectedGenres);
                DataRepository.AddFeedItem("{0} has updated their band profile.", 2, model.BandId);

                // notification
                Notifications.SendBandNotifications(NotificationType.NotifyBandProfileUpdate, model.BandId, new BandMessageData { Band = model.Name, Link = model.UrlName });
            }

            return JsonValidationResult(ModelState);
        }

        public ActionResult Images(Guid bandId)
        {
            var model = new EditImagesModel();

            model.Images = DataRepository.GetImagesByBand(bandId, true);
            model.Galleries = DataRepository.GetGalleriesByBand(bandId);

            ViewData["galleryList"] = model.Galleries;
            return View("Images", model);
        }

        public ActionResult Audio(Guid bandId)
        {
            var model = new EditAudioModel
            {
                Audio = DataRepository.GetAudioByBand(bandId)
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteAudio(Guid audioId)
        {
            DataRepository.DeleteAudio(audioId);

            return Json(new { success = false });
        }

        public ActionResult Videos(Guid bandId)
        {
            return View();
        }

        public ActionResult Members(Guid bandId)
        {
            ViewData["memberTypes"] = DataRepository.GetMemberTypes();
            var band = DataRepository.GetBand(bandId, loadExtended: false);
            band.PendingMemberInvites = DataRepository.GetUserRelationsByBand(band.BandId, 2);
            band.PendingMemberRequests = DataRepository.GetUserRelationsByBand(band.BandId, 3);
            band.ActiveMembers = DataRepository.GetUserRelationsByBand(band.BandId, 1);
            return View(band);
        }

        public ActionResult Events(Guid bandId)
        {
            var events = DataRepository.GetEventsByBand((Guid)bandId).OrderBy(r => r.Event.Date);
            return View(events);
        }

        public ActionResult Delete(Guid bandId)
        {
            return View();
        }

        public ActionResult Announcements(Guid bandId)
        {
            var feed = DataRepository.GetFeedByBand(bandId).Where(f => f.FeedItemTypeId == 6);

            return View(feed);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Announcements")]
        public ActionResult AnnouncementsPost(string announcement, Guid bandId)
        {
            DataRepository.AddFeedItem("{0} says: <strong>\"" + announcement + "\"</strong>", 6, bandId);

            return JsonValidationResult(ModelState);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAnnouncement(Guid feedItemId, Guid bandId)
        {
            var feed = DataRepository.GetFeedItem(feedItemId);
            
            if (feed.BandId == bandId)
            {
                DataRepository.DeleteFeedItem(feedItemId);
            }

            return RedirectToRoute("mybands", new { action = "announcements", bandId = bandId });
        }

        public ActionResult Join(string letter)
        {
            if (!String.IsNullOrWhiteSpace(letter))
            {
                var bands = DataRepository.GetBandsByLetter(letter);
                return View(bands);
            }
            else
            {
                return View();
            }
        }

        [ActionName("Join")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JoinPost(Guid bandId)
        {
            var relation = DataRepository.GetUserBandRelation(loggedUserGuid, bandId);

            if ((relation != null && relation.RelationTypeId == 4) || relation == null)
            {
                DataRepository.SetUserBandRelation(bandId, loggedUserGuid, 3, false);
            }

            return RedirectToRoute("MyUser", new { action = "edit" });
        }

        [HttpPost]
        public ActionResult Invite(Guid bandId, string username)
        {
            var userId = DataRepository.GetUserId(username);

            if (bandId != null && userId != null && loggedUser.UserName != username && DataRepository.GetUserBandRelation(loggedUserGuid, (Guid)bandId).RelationTypeId == 1)
            {
                DataRepository.SetUserBandRelation((Guid)bandId, (Guid)userId, 2, false);
            }
            else
            {
                //
            }

            return RedirectToRoute("mybands", new { action = "members", bandId = bandId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(Guid relationId)
        {
            // get the band that is in my bands
            var relation = DataRepository.GetUserBandRelation(relationId);
            // check to see if relation is one of my pending or active members
            if (DataRepository.UserIsMemberOfBand(loggedUserGuid, relation.BandId) || DataRepository.UserIsMemberOfBand(loggedUserGuid, relation.BandId, 2))
            {
                DataRepository.SetUserBandRelation(relationId, 1, true);
                DataRepository.AddFeedItem("{0} has joined the band {1}", 4, relation.BandId, userId: relation.UserId);
            }

            return RedirectToRoute("MyBands", new { action = "edit", bandUrl = relation.Band.UrlName });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRelation(Guid relationId)
        {
            var relation = DataRepository.GetUserBandRelation(relationId);
            var bandId = relation.BandId;

            // check to see if relation is one of my pending or active members
            if (DataRepository.UserIsMemberOfBand(loggedUserGuid, bandId) || DataRepository.UserIsMemberOfBand(loggedUserGuid, bandId, 2) || DataRepository.UserIsMemberOfBand(loggedUserGuid, bandId, 3))
            {
                DataRepository.DeleteUserBandRelation(relationId);

                if (relation.RelationTypeId == 1)
                {
                    DataRepository.AddFeedItem("{0} has left the band {1}", 4, relation.BandId, userId: relation.UserId);
                }
            }

            return RedirectToRoute("MyUser", new { action = "edit" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBand(string bandName)
        {
            if (!String.IsNullOrWhiteSpace(bandName))
            {
                var band = DataRepository.GetBand(bandName, false);

                if (band == null)
                {
                    var bandUrl = EntityNameHelper.CreateUrl(bandName);

                    var guid = Guid.NewGuid();
                    BandModel newBand = new BandModel() { BandId = guid, UrlName = bandUrl, Name = bandName, Date = DateTime.Now };

                    DataRepository.CreateBand(newBand, loggedUserGuid);

                    DataRepository.AddFeedItem("{0} has created their band profile!", 2, guid);

                    return RedirectToRoute("MyBands", new { action = "edit", bandUrl = bandUrl });
                }
                else
                {
                    ModelState.AddModelError("BandExists", "This band already exists.");
                    return View("Join");
                }
            }

            return RedirectToRoute("join");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBand(Guid bandId)
        {
            var relation = DataRepository.GetUserBandRelation(loggedUserGuid, (Guid)bandId);

            if (relation != null && relation.RelationTypeId == 1)
            {
                DataRepository.DeleteBand((Guid)bandId);
            }

            return RedirectToRoute("MyUser", new { action = "edit" });
        }

        [HttpPost]
        public ActionResult UploadAvatar(Guid bandId)
        {
            if (Request.Files.Count > 0 && Request.Files["avatar"] != null)
            {
                var file = Request.Files["avatar"];

                if (file.ContentLength > 0)
                {
                    if (bandId != null && DataRepository.UserIsMemberOfBand(loggedUserGuid, (Guid)bandId))
                    {
                        var filename = HttpContext.SaveImage(file, true, true);
                        DataRepository.SetBandAvatar(filename, (Guid)bandId);
                        DataRepository.AddFeedItem("{0} has changed their band avatar.", 2, (Guid)bandId);
                    }
                }
            }

            return RedirectToRoute("mybands", new { action = "edit", bandId = bandId });
        }

        [HttpPost]
        public ActionResult SetMemberType(Guid memberTypeId, Guid bandId)
        {
            var band = DataRepository.GetBand(bandId);
            var relation = DataRepository.GetUserBandRelation(loggedUserGuid, band.BandId);

            DataRepository.SetMemberType(relation.RelationId, memberTypeId);    

            return Json(new { success = true });
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewData["bandId"] = filterContext.RouteData.Values["bandId"];
            base.OnActionExecuting(filterContext);
        }
    }
}
