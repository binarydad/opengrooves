using System;
using System.Linq;
using System.Web.Mvc;
using OpenGrooves.Data;
using OpenGrooves.Core.Helpers;
using OpenGrooves.WebLanding.Models;
using System.Web.Security;
using StructureMap;
using OpenGrooves.Services.Notifications;
using OpenGrooves.Services.Mapping;
using OpenGrooves.Core.Extensions;
using StructureMap.Attributes;

namespace OpenGrooves.WebLanding.Controllers
{
    public class LandingController : Controller
    {
        [SetterProperty]
        public EmailNotificationServiceBase notifications { get; set; }

        [OutputCache(Duration = 43200)]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ActionName("SignUp")]
        public ActionResult SignUpPost(SignUpModel m)
        {
            // check city/state
            if (!m.City.IsNullOrWhiteSpace() && m.State.Name.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("CityNoState", "If entering a city, please select your state.");
            }

            if (ModelState.IsValid)
            {
                using (var ctx = new OpenGroovesEntities())
                {
                    m.Email = m.Email.Trim();
                    bool hasBand = !m.BandName.IsNullOrWhiteSpace();
                    
                    MembershipCreateStatus status;
                    var username = EntityNameHelper.CreateUrl(m.Email);
                            
                    // check for email
                    if (Membership.FindUsersByEmail(m.Email).Count > 0 || Membership.FindUsersByName(username).Count > 0)
                    {
                        ModelState.AddModelError("EmailExists", "This email address is already registered");
                    }

                    // check for band name
                    if (hasBand && ctx.Bands.Any(b => b.Name == m.BandName.Trim()))
                    {
                        ModelState.AddModelError("BandExists", String.Format("The band, \"{0}\", has already registered. But don't worry, you can leave this field blank and request to join this band later.", m.BandName));
                        ModelState.AddModelError("BandExistsHelp", "If this is in error, please contact issues@opengrooves.com.");
                        m.BandName = String.Empty;
                    }
                           
                    // if we're here, we're good to go!
                    if (ModelState.IsValid)
                    {
                        var user = Membership.CreateUser(username, "thisisafakepassword", m.Email, null, null, false, out status);

                        // if membership create is good
                        if (status == MembershipCreateStatus.Success)
                        {
                            var guid = (Guid)user.ProviderUserKey;

                            var existingUser = ctx.Users.SingleOrDefault(u => u.Email == m.Email);

                            existingUser.RealName = m.RealName;
                            existingUser.Email = m.Email;
                            
                            existingUser.SetupRequired = true;

                            var relation = new UsersBand
                            {
                                RelationId = Guid.NewGuid(),
                                RelationTypeId = 1,
                                Date = DateTime.Now
                            };

                            if (!m.City.IsNullOrWhiteSpace() && !m.State.Name.IsNullOrWhiteSpace())
                            {
                                existingUser.City = m.City;
                                existingUser.State = m.State.Name;
                            }

                            var address = LocationHelper.CityStateOrZip(m.City, m.State.Name);
                            var location = ObjectFactory.GetInstance<ILocationService>();
                            var loc = location.GetLocation(address);

                            if (loc != null)
                            {
                                existingUser.Latitude = loc.Coordinate.Latitude;
                                existingUser.Longitude = loc.Coordinate.Longitude;
                                existingUser.State = loc.State;
                                existingUser.City = loc.City;
                            }

                            if (hasBand)
                            {
                                var band = new Band
                                {
                                    BandId = Guid.NewGuid(),
                                    Name = m.BandName,
                                    City = m.City,
                                    Searchable = EntityNameHelper.StripUselessWords(m.BandName),
                                    State = m.State.Name,
                                    UrlName = Core.Helpers.EntityNameHelper.CreateUrl(m.BandName),
                                    Date = DateTime.Now
                                };

                                if (loc != null)
                                {
                                    band.Latitude = loc.Coordinate.Latitude;
                                    band.Longitude = loc.Coordinate.Longitude;
                                    band.City = loc.City;
                                    band.State = loc.State;
                                }

                                relation.Band = band;

                                existingUser.UsersBands.Add(relation);
                            }

                            ctx.SaveChanges();

                            NotifyNewSignup(m);

                            return View("ThankYou", m);
                        }
                        else if (status == MembershipCreateStatus.DuplicateEmail)
                        {
                            ModelState.AddModelError("DupEmail", "This email address already exists.");
                        }
                        else
                        {
                            ModelState.AddModelError("OtherError", "Your account could not be created.");
                        }
                    }
                    
                }
            }

            return View(m);
            
        }

        private static void NotifyNewSignup(SignUpModel m)
        {
            // just for paranoia sake
            try
            {
                Core.Helpers.EmailHelper.SendEmail("ryan@marriedgeek.com", "New OG Signup", String.Format("{0} has signed up.", m.Email));
            }
            catch { }
        }

        [OutputCache(Duration = 43200)]
        public ActionResult About()
        {
            return View();
        }

        [OutputCache(Duration = 43200)]
        public ActionResult BetaTester()
        {
            return View();
        }

        [OutputCache(Duration = 43200)]
        public ActionResult Privacy()
        {
            return View();
        }
    }
}
