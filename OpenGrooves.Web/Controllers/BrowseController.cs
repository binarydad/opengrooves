using OpenGrooves.Core;
using OpenGrooves.Core.Extensions;
using OpenGrooves.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OpenGrooves.Web.Controllers
{
    public class BrowseController : BaseController
    {
        public ActionResult Genre(string filter)
        {
            var model = new BrowseModel();

            model.Bands = DataRepository.GetBandsByGenre(filter);
            model.Genres = DataRepository.GetGenres();

            ViewData["genre-name"] = DataRepository.GetGenre(filter).Name;

            return View(model);
        }

        public ActionResult Newest()
        {
            var model = new BrowseModel();
            model.Bands = DataRepository.GetNewestBands();

            return View(model);
        }

        public ActionResult Letter(string letter)
        {
            var model = new BrowseModel();
            var myBands = DataRepository.GetBandRelationsByUser(loggedUserGuid, 2).Select(b => b.Band);
            model.Bands = DataRepository.GetBandsByLetter(letter).Except(myBands);

            return View(model);
        }

        public ActionResult Search(string query)
        {
            if (query.IsNullOrWhiteSpace())
            {
                return View(new List<BandModel>()); // meh...
            }
                
            var bands = DataRepository.SearchBands(query);

            return View(bands);
        }

        public ActionResult Nearby(string address)
        {
            return View(new BrowseModel { Address = address });
        }

        public ActionResult Events(string address)
        {
            return View(new BrowseModel { Address = address });
        }

        public ActionResult LocalBands(int? radius, string address, SearchSort? sort, int max = 20)
        {
            Location loc = null;

            if (String.IsNullOrWhiteSpace(address))
            {
                if (Request.IsAuthenticated)
                {
                    loc = DataRepository.GetUserLocation(loggedUserGuid);
                    if (loc != null)
                    {
                        address = Core.Helpers.LocationHelper.CityStateOrZip(loc.City, loc.State, loc.Zip);
                    }
                }
            }
            else
            {
                loc = Location.GetLocation(address);
            }

            radius = radius ?? 25;
            address = address ?? String.Empty;

            var model = new BrowseModel();

            if (loc != null)
            {
                model.Bands = DataRepository.GetBandsByLocation(loc.Coordinate, (int)radius, max: max, sort: sort);
                model.Location = loc;
            }
            
            model.Radius = (int)radius;
            model.Address = address;

            return View(model);
        }

        public ActionResult LocalEvents(int? radius, string address, SearchSort? sort, int max = 20)
        {
            Location loc = null;

            if (String.IsNullOrWhiteSpace(address))
            {
                if (Request.IsAuthenticated)
                {
                    loc = DataRepository.GetUserLocation(loggedUserGuid);
                    if (loc != null)
                    {
                        address = Core.Helpers.LocationHelper.CityStateOrZip(loc.City, loc.State, loc.Zip);
                    }
                }
            }
            else
            {
                loc = Location.GetLocation(address);
            }

            radius = radius ?? 25;
            address = address ?? String.Empty;

            var model = new BrowseModel();

            if (loc != null)
            {
                // getting closest events, but then sorting the result by date
                model.Events = DataRepository.GetEventsByLocation(loc.Coordinate, (int)radius, max: max, sort: sort).OrderBy(e => e.Date);
                model.Location = loc;
            }

            model.Radius = (int)radius;
            model.Address = address;

            return View(model);
        }

        [OutputCache(Duration = 10, VaryByParam = "none")]
        public ActionResult UpcomingEvents(int max = 10)
        {
            var browseModel = new BrowseModel();
            browseModel.Events = DataRepository.GetUpcomingEventsByUser(loggedUserGuid, max).OrderBy(e => e.Date);
            return View(browseModel);
        }
    }
}
