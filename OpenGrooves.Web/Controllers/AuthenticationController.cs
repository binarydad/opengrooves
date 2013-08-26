using OpenGrooves.Core;
using OpenGrooves.Core.Extensions;
using OpenGrooves.Core.Helpers;
using OpenGrooves.Services.Notifications;
using OpenGrooves.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;

namespace OpenGrooves.Web.Controllers
{
    public class AuthenticationController : BaseController
    {
        static AuthenticationController()
        {
            AutoMapper.Mapper.CreateMap<SignupModel, UserModel>();
        }

        public ActionResult Login()
        {
            var login = new LoginModel();

            return View(login);
        }

        [HttpPost]
        [ActionName("Login")]
        public ActionResult LoginPost(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = Membership.GetUser(model.Username);

                if (user != null && !user.IsApproved)
                {
                    ModelState.AddModelError("Locked", "Your account has not been activated.");
                }
                else if (Membership.ValidateUser(model.Username, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Username, true);
                    var dbUser = DataRepository.GetUser((Guid)user.ProviderUserKey);

                    Session["RealName"] = dbUser.RealName;

                    if (returnUrl.IsNullOrWhiteSpace())
                    {
                        return RedirectToRoute("home", new { action = "home" });
                    }

                    return Redirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("InvalidLogin", "Your username/password combination was incorrect.");
                }
            }

            return View(model);
        }

        public ActionResult Signup ()
        {
            var signup = new SignupModel();

            return View(signup);
        }

        [HttpPost]
        [ActionName("Signup")]
        public ActionResult SignupPost(SignupModel model)
        {
            if (ModelState.IsValid)
            {
                if (Membership.GetUser(model.Username) != null)
                {
                    ModelState.AddModelError("UserExists", String.Format("This username, {0}, is taken. Please try another.", model.Username));
                }
                else if (model.Password != model.PasswordConfirm)
                {
                    ModelState.AddModelError("PasswordMatch", "Please confirm your password.");
                }
                else
                {
                    MembershipCreateStatus status;
                    var user = Membership.CreateUser(model.Username, model.Password, model.Email, null, null, false, out status);

                    if (status == MembershipCreateStatus.Success)
                    {
                        var guid = (Guid)user.ProviderUserKey;

                        var u = DataRepository.GetUser(guid);
                        u.RealName = model.RealName;
                        u.City = model.City;
                        u.State = model.State;
                        DataRepository.UpdateUser(u);

                        SendAccountEmail(user.UserName, user.Email, guid);

                        return View("AccountCreated", model);
                    }
                    else if (status == MembershipCreateStatus.DuplicateEmail)
                    {
                        ModelState.AddModelError("DupEmail", "This email address already exists.");
                    }
                    else
                    {
                        ModelState.AddModelError("OtherError", "Your account could not be created");
                    }
                }
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            return Redirect("~/");
        }

        [NonAction]
        private void SendAccountEmail (string username, string email, Guid key)
        {
            string keyEncoded = UrlGuidHelper.GetUrlString(key);
            var link = String.Format("http://{0}/activate/{1}", Request.ServerVariables["SERVER_NAME"], keyEncoded);

            var replacements = new Dictionary<string, string>();
            replacements.Add("LINK", link);
            replacements.Add("USERNAME", username);
            
            var template = TemplateBuilder.LoadTemplate(NotificationType.Activate, replacements);

            template.To.Add(email);

            EmailHelper.SendEmail(template);
        }
    }
}
