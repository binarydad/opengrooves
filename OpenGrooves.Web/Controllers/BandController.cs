using OpenGrooves.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace OpenGrooves.Web.Controllers
{
    public class BandController : BaseController
    {
        public ActionResult Profile(string name)
        {
            var model = CreateModel(name);

            if (model == null)
            {
                // since this sits at the route, redirect to the homepage if not found
                return RedirectToRoute("Default", new { controller = "Default", action = "index" });
            }

            return View(model);
        }

        public ActionResult Images(string name)
        {
            var images = DataRepository.GetImagesByBand(name, true);

            return View("Images", images);
        }

        public ActionResult BandFeed(Guid bandId, int max = 5)
        {
            return View("FeedItems", DataRepository.GetFeedByBand(bandId, max));
        }

        public ActionResult BandImages(Guid bandId)
        {
            return View("ImageGallery", DataRepository.GetImagesByBand(bandId, true));
        }

        public ActionResult BandGalleries(Guid bandId)
        {
            return View("GalleryList", DataRepository.GetGalleriesByBand(bandId));
        }

        public ActionResult BandFans(Guid bandId)
        {
            return View("UsersList", DataRepository.GetUserRelationsByBand(bandId, 4, 12).Select(u => u.User));
        }

        public ActionResult BandEvents(Guid bandId, int max = 20)
        {
            return View("EventsList", DataRepository.GetEventsByBand(bandId, max, true).Select(e => e.Event).OrderBy(e => e.Date));
        }

        public ActionResult BandAudio(Guid bandId)
        {
            return View("AudioList", DataRepository.GetAudioByBand(bandId));
        }

        #region Connections (Follow/unfollow)
        public ActionResult Follow(string name)
        {
            var bandId = (Guid)DataRepository.GetBandId(name);
            DataRepository.SetUserBandRelation(bandId, loggedUserGuid, 4);

            DataRepository.AddFeedItem("{0} is now following {1}.", 4, bandId, userId: loggedUserGuid);

            return RedirectToRoute("band", new { action = "profile" });
        }

        public ActionResult UnFollow(string name)
        {
            var bandId = (Guid)DataRepository.GetBandId(name);
            DataRepository.DeleteUserBandRelation(bandId, loggedUserGuid);

            return RedirectToRoute("band", new { action = "profile" });
        } 
        #endregion

        #region Private methods
        private BandProfileModel CreateModel(string name)
        {
            var model = new BandProfileModel();
            var band = DataRepository.GetBand(name);

            if (band != null)
            {
                band.ActiveMembers = DataRepository.GetUserRelationsByBand(band.BandId, 1);

                model.Band = band;
                model.Status = Request.IsAuthenticated ? GetStatus(band.BandId, loggedUserGuid) : BandProfileModel.FollowStatus.NoRelation;

                return model;
            }

            return null;
        }

        private BandProfileModel.FollowStatus GetStatus(Guid bandId, Guid userId)
        {
            var relation = DataRepository.GetUserBandRelation(userId, bandId);

            if (relation != null)
            {
                if (relation.RelationTypeId == 1)
                {
                    return BandProfileModel.FollowStatus.Member;
                }
                else if (relation.RelationTypeId == 4)
                {
                    return BandProfileModel.FollowStatus.Following;
                }
                else
                {
                    return BandProfileModel.FollowStatus.Pending;
                }
            }
            else
            {
                return BandProfileModel.FollowStatus.NoRelation;
            }
        } 
        #endregion
    }
}
