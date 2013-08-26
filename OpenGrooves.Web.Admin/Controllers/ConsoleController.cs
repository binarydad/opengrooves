using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using StructureMap.Attributes;
using OpenGrooves.Services.Notifications;
using OpenGrooves.Web.Admin.Models;
using OpenGrooves.Data;
using OpenGrooves.Core;
using OpenGrooves.Core.Helpers;

namespace OpenGrooves.Web.Admin.Controllers
{
    public class ConsoleController : Controller
    {
        [SetterProperty]
        public EmailNotificationServiceBase Notifications { get; set; }

        public ActionResult BetaTesters()
        {
            var model = new ConsoleModel();

            using (var ctx = new OpenGrooves.Data.OpenGroovesEntities())
            {
                var users = ctx.Users.Where(u => u.SetupRequired).AsEnumerable().Select(u => new BetaUser
                    {
                        ActivationUrl = String.Format("http://preview.opengrooves.com/activate/{0}/setupaccount", Core.Helpers.UrlGuidHelper.GetUrlString(u.UserId)),
                        Username = u.UserName,
                        Email = u.Email,
                        Name = u.RealName,
                        UserId = u.UserId
                    });

                model.BetaUsers = users.ToList();
            }

            return View(model);
        }

        public ActionResult SendBetaActivation(Guid id)
        {
            using (var ctx = new OpenGrooves.Data.OpenGroovesEntities())
            {
                var user = ctx.Users.SingleOrDefault(u => u.UserId == id);

                if (user != null)
                {
                    var replacements = new Dictionary<string, string>();
                    var url = String.Format("http://preview.opengrooves.com/activate/{0}/setupaccount", Core.Helpers.UrlGuidHelper.GetUrlString(user.UserId));
                    replacements.Add("ACTIVATION", url);

                    var template = TemplateBuilder.LoadTemplate(NotificationType.BetaTestActivate, replacements);

                    template.To.Add(user.Email);
                    EmailHelper.SendEmail(template);

                    template.To.Clear();
                    template.To.Add("beta@marriedgeek.com");
                    EmailHelper.SendEmail(template);
                }

                return RedirectToRoute("Default", new { action = "BetaTesters" });
            }
        }

        [HttpPost]
        [ActionName("Email")]
        public ActionResult SendEmail(ConsoleModel model)
        {
            var usernameList = model.Usernames.Split(new char[] { ',' });
            var guids = GetGuidsFromUsers(usernameList);
            var data = new Core.MessageData { Subject = "test", Message = "Adasdsaa" };

            Notifications.SendNotifications(Core.NotificationType.BetaTestWelcome, guids, data, true);

            return RedirectToRoute("Default", new { action = "BetaTesters" });
        }

        #region Private Methods
        private IEnumerable<Guid> GetGuidsFromUsers(IEnumerable<string> users)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                var userIds = from u in ctx.Users
                              join i in users on u.UserName equals i
                              select u.UserId;

                return userIds.AsEnumerable();
            }
        } 
        #endregion
    }
}
