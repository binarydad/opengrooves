using OpenGrooves.Core;
using OpenGrooves.Core.Extensions;
using OpenGrooves.Core.Helpers;
using OpenGrooves.Services.Notifications;
using OpenGrooves.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace OpenGrooves.Web.Controllers
{
    public class ActivationController : BaseController
    {
        #region Regular Activation
        public ActionResult Index(string keyEncoded)
        {
            var user = GetUser(keyEncoded);

            if (user != null)
            {
                if (!user.IsApproved)
                {
                    user.IsApproved = true;

                    Membership.UpdateUser(user);
                    FormsAuthentication.SetAuthCookie(user.UserName, true);

                    SendWelcomeEmail(user.UserName, user.Email);

                    return View((object)user.UserName);
                }

                return View("AlreadyActive");
            }

            return View("Error");
        } 
        #endregion

        #region Beta User Activation
        public ActionResult SetUpAccount(string keyEncoded)
        {
            var user = GetUser(keyEncoded);

            if (user != null)
            {
                if (user.IsApproved)
                {
                    return View("AlreadyActive");
                }

                var defaultPass = user.ResetPassword();

                var model = new BetaActivate
                {
                    Username = user.UserName,
                    Email = user.Email
                };

                Membership.UpdateUser(user);

                return View("BetaUsers", model);
            }

            return View("Error");
        }

        [HttpPost]
        [ActionName("SetUpAccount")]
        public ActionResult SetUpAccountPost(string keyEncoded, BetaActivate m)
        {
            if (ModelState.IsValid)
            {
                var activationKey = UrlGuidHelper.GetGuid(keyEncoded);

                // using EF directly, as this is only temporary
                using (var ctx = new OpenGrooves.Data.OpenGroovesEntities())
                {
                    var usernameExists = ctx.Users.Where(u => u.UserName == m.Username && u.UserId != activationKey).Any();

                    if (usernameExists)
                    {
                        ModelState.AddModelError("UsernameExists", "The username you have chosen is taken. Please choose another.");
                        return View("BetaUsers", m);
                    }

                    var user = ctx.Users.SingleOrDefault(u => u.UserId == activationKey);
                    user.UserName = m.Username;
                    user.LoweredUserName = m.Username.ToLower();
                    user.SetupRequired = false;
                    ctx.SaveChanges();
                }

                var newPass = m.NewPassword;

                var newUser = Membership.GetUser(activationKey);

                if (!newPass.IsNullOrWhiteSpace())
                {
                    var tempPass = newUser.ResetPassword();
                    var changed = newUser.ChangePassword(tempPass, newPass);
                    newUser.IsApproved = true;

                    Membership.UpdateUser(newUser);

                    SendBetaWelcomeEmail(newUser.UserName, newUser.Email, newPass);
                    FormsAuthentication.SetAuthCookie(newUser.UserName, true);
                }

                return View("Index", (object)newUser.UserName);
            }

            return View("BetaUsers", m);
        } 
        #endregion

        #region Private Methods
        [NonAction]
        private static MembershipUser GetUser(string keyEncoded)
        {
            var guid = UrlGuidHelper.GetGuid(keyEncoded);
            var user = Membership.GetUser(guid);
            return user;
        }

        [NonAction]
        private void SendWelcomeEmail(string username, string email)
        {
            var replacements = new Dictionary<string, string>();
            replacements.Add("USERNAME", username);

            var template = TemplateBuilder.LoadTemplate(NotificationType.Welcome, replacements);

            template.To.Add(email);

            EmailHelper.SendEmail(template);
        }

        [NonAction]
        private void SendBetaWelcomeEmail(string username, string email, string password)
        {
            var replacements = new Dictionary<string, string>();
            replacements.Add("USERNAME", username);
            replacements.Add("PASSWORD", password);

            var template = TemplateBuilder.LoadTemplate(NotificationType.BetaTestWelcome, replacements);

            template.To.Add(email);

            EmailHelper.SendEmail(template);
        } 
        #endregion
    }
}
