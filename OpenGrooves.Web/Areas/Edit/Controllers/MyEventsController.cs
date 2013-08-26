using OpenGrooves.Core;
using OpenGrooves.Core.Extensions;
using OpenGrooves.Core.Helpers;
using OpenGrooves.Web.Areas.Edit.Models;
using OpenGrooves.Web.Controllers;
using OpenGrooves.Web.Extensions;
using OpenGrooves.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace OpenGrooves.Web.Areas.Edit.Controllers
{
    [Authorize]
    public class MyEventsController : BaseController
    {
        #region Events
        public ActionResult List()
        {
            var events = DataRepository.GetEventsByUser(loggedUserGuid).Where(e => e.Date > DateTime.Now.AddDays(-1));
            return View(events);
        }

        public ActionResult Edit(string eventName, string letter)
        {
            var model = CreateModel(eventName, letter);

            // this user does not own this event, redirect to front page
            if (!model.Event.Users.Any(e => e.UserId == loggedUserGuid))
            {
                return RedirectToRoute("event", new { action = "index", name = eventName });
            }

            return View(model);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditPost(EditEventModel m, string eventName, string letter)
        {
            var model = CreateModel(eventName, letter);

            if (ModelState.IsValid)
            {
                var oldUrl = model.Event.UrlName;
                var newUrl = EntityNameHelper.CreateUrl(m.Event.Name);

                var eventModel = m.Event;
                eventModel.EventId = model.Event.EventId;
                //eventModel.UrlName = newUrl;

                DataRepository.UpdateEvent(eventModel);

                if (model.Event.Date >= DateTime.Now && model.Event.Bands.Any())
                {
                    // for each band on the event
                    model.Event.Bands.ToList().ForEach(b =>
                    {
                        DataRepository.AddFeedItem("Event page for {0} has been updated.", 3, b.Band.BandId, eventId: model.Event.EventId);
                    });
                }

                // send notification, adding the list of bands for the event
                Notifications.SendEventNotifications(NotificationType.NotifyEventUpdated, model.Event.EventId, new EventMessageData { Event = m.Event.Name, Link = model.Event.UrlName, Bands = model.Event.Bands.Select(b => b.Band.Name).ToString("<br />") });

                //if (oldUrl != newUrl)
                //{
                //    return JsonValidationResult(ModelState, Url.RouteUrl("myevents", new { action = "edit", eventName = newUrl }));
                //}
            }

            return JsonValidationResult(ModelState);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteEvent(string eventName)
        {
            var ev = DataRepository.GetEvent(eventName);

            if (ev.Users.Any(u => u.UserId == loggedUserGuid))
            {
                DataRepository.DeleteEvent(ev.EventId);
            }

            return RedirectToRoute("myuser", new { action = "edit" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string eventName)
        {
            var url = EntityNameHelper.CreateUrl(eventName, true);

            var existing = DataRepository.GetEvent(url, loadExtended: false);

            if (eventName.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("NoName", "No event name was entered");
            }
            // no event exists, create a new one
            else if (existing == null)
            {
                DataRepository.CreateEvent(new EventModel { Name = eventName, UrlName = url }, loggedUserGuid);
                return RedirectToRoute("myevents", new { action = "edit", eventName = url });
            }
            else
            {
                ModelState.AddModelError("EventExists", "This event already exists");
            }

            var events = DataRepository.GetEventsByUser(loggedUserGuid);
            return View("list", events);
        }

        [HttpPost]
        public ActionResult UploadAvatar(string eventName)
        {
            if (Request.Files.Count > 0 && Request.Files["avatar"] != null)
            {
                var file = Request.Files["avatar"];

                if (file.ContentLength > 0)
                {
                    var ev = DataRepository.GetEvent(eventName, loadExtended: false);
                    if (ev != null && ev.Users.Any(u => u.UserId == loggedUserGuid))
                    {
                        var filename = HttpContext.SaveImage(file, true, true);
                        DataRepository.SetEventAvatar(filename, ev.EventId);
                    }
                }
            }

            return RedirectToRoute("myevents", new { action = "edit", eventName = eventName });
        }
        #endregion

        #region Bands
        public ActionResult EditBand(Guid relationId)
        {
            var relation = DataRepository.GetBandEventRelation(relationId);

            if (!relation.Order.HasValue)
            {
                relation.Order = 1;
            }

            return View(relation);
        }

        [HttpPost]
        [ActionName("EditBand")]
        public ActionResult EditBandPost(Guid relationId, DateTime? showTime, int? order, string eventName)
        {
            var relation = DataRepository.GetBandEventRelation(relationId);

            DataRepository.SetBandEventLineup(relationId, showTime: showTime, order: order);
            return RedirectToRoute("MyEvents", new { action = "edit", eventName = eventName });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddBand(Guid bandId, string eventName)
        {
            var ev = DataRepository.GetEvent(eventName, loadExtended: false);

            if (DataRepository.UserIsMemberOfBand(loggedUserGuid, bandId))
            {
                // set active relation
                DataRepository.SetBandEventRelation(bandId, ev.EventId);

                // add feed alert
                DataRepository.AddFeedItem("{0} was added to the event {1}.", 5, bandId, eventId: ev.EventId);

                // send notifications
                var band = DataRepository.GetBand(bandId, false);
                Notifications.SendEventNotifications(NotificationType.NotifyEventBandsAdded, ev.EventId, new EventMessageData { Event = ev.Name, Link = ev.UrlName, Bands = band.Name });
            }

            return RedirectToRoute("myevents", new { action = "edit", eventName = eventName });
        }

        [HttpPost]
        public ActionResult RequestBand(Guid bandId, string eventName)
        {
            var ev = DataRepository.GetEvent(eventName, loadExtended: false);

            DataRepository.SetBandEventRelation(bandId, ev.EventId, false);

            return RedirectToRoute("myevents", new { action = "edit", eventName = ev.UrlName });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RemoveBand(Guid relationId, string eventName)
        {
            var relation = DataRepository.GetBandEventRelation(relationId);

            if (relation != null)
            {
                DataRepository.DeleteBandEventRelation(relationId);
            }

            return RedirectToRoute("myevents", new { action = "edit", eventName = eventName });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Approve(Guid relationId, string eventName)
        {
            var relation = DataRepository.GetBandEventRelation(relationId);
            var ev = relation.Event;

            if (DataRepository.UserIsMemberOfBand(loggedUserGuid, relation.BandId))
            {
                DataRepository.SetBandEventRelation(relationId, true);
                DataRepository.AddFeedItem("{0} was added to the event {1}.", 5, relation.BandId, eventId: ev.EventId);

                var band = DataRepository.GetBand(relation.BandId, false);
                Notifications.SendEventNotifications(NotificationType.NotifyEventBandsAdded, ev.EventId, new EventMessageData { Event = ev.Name, Link = ev.UrlName, Bands = band.Name });
            }

            return RedirectToRoute("MyUser", new { action = "edit" });
        }

        
        #endregion

        #region Private Methods
        private EditEventModel CreateModel(string name, string letter)
        {
            var model = new EditEventModel();

            model.Event = DataRepository.GetEvent(name);

            // exclude bands on the list
            model.MyBands = DataRepository.GetBandRelationsByUser(loggedUserGuid, 1).Where(b => !model.Event.Bands.Select(m => m.Band.BandId).Contains(b.Band.BandId)).Select(b => b.Band);

            // exclude my bands
            model.OtherBands = DataRepository.GetBandsByLetter(letter).Where(b => !model.MyBands.Select(m => m.BandId).Contains(b.BandId));

            return model;
        } 
        #endregion
    }
}
