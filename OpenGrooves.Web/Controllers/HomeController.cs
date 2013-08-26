using OpenGrooves.Web.Models;
using System.Linq;
using System.Web.Mvc;

namespace OpenGrooves.Web.Controllers
{
    
    public class HomeController : BaseController
    {
        private HomeModel model = new HomeModel();

        [Authorize]
        public ActionResult Home()
        {
            return View();
        }

        [Authorize]
        public ActionResult Feed()
        {
            return View();
        }

        public ActionResult Events()
        {
            return View();
        }

        [Authorize]
        public ActionResult Bands()
        {
            model.Bands = DataRepository.GetBandRelationsByUser(loggedUserGuid, 4).Select(b => b.Band).OrderBy(b => b.Name);
            model.MyBands = DataRepository.GetBandRelationsByUser(loggedUserGuid, 1).Select(b => b.Band).OrderBy(b => b.Name);

            return View(model);
        }

        public ActionResult Photos()
        {
            if (Request.IsAuthenticated)
            {
                model.Images = DataRepository.GetRecentBandImages(loggedUserGuid, 50, true);
                model.Galleries = DataRepository.GetBandGalleriesByUser(loggedUserGuid).OrderByDescending(g => g.Date).Take(10);
            }
            else
            {
                model.Images = DataRepository.GetRecentImages(50);
            }

            return View(model);
        }

        [Authorize]
        public ActionResult HomeFeedItems(int max = 20)
        {
            var feed = DataRepository.GetFeedByUser(loggedUserGuid, max);

            feed.ToList().ForEach(f =>
            {
                if (f.FeedItemTypeId == 7)
                {
                    f.Images = DataRepository.GetImagesByBatch(f.BatchId, 8);
                }
            });

            // if there are no images, dont display the feed item
            return View(feed.Where(f => f.FeedItemTypeId == 7 ? f.Images.Any() : true));
        } 
    }
}
