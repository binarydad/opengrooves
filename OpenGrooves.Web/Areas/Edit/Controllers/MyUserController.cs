using OpenGrooves.Web.Controllers;
using OpenGrooves.Web.Extensions;
using OpenGrooves.Web.Models;
using System.Web;
using System.Web.Mvc;

namespace OpenGrooves.Web.Areas.Edit.Controllers
{
    [Authorize]
    public class MyUserController : BaseController
    {
        public ActionResult Edit()
        {
            var model = DataRepository.GetUser(loggedUserGuid);
            model.PendingBandInvites = DataRepository.GetBandRelationsByUser(loggedUserGuid, 2);
            model.PendingBandRequests = DataRepository.GetBandRelationsByUser(loggedUserGuid, 3);
            model.MemberOfBands = DataRepository.GetBandRelationsByUser(loggedUserGuid, 1);
            return View(model);
        }

        [ActionName("Edit")]
        [HttpPost]
        public ActionResult EditPost(UserModel m)
        {
            if (ModelState.IsValid)
            {
                var userModel = m;

                userModel.UserName = loggedUser.UserName;
                userModel.UserId = loggedUserGuid;
                
                DataRepository.UpdateUser(userModel);
            }

            return JsonValidationResult(ModelState);            
        }

        [HttpPost]
        public ActionResult UploadAvatar()
        {
            if (Request.Files.Count > 0 && Request.Files["avatar"] != null)
            {
                var file = Request.Files["avatar"];

                if (file.ContentLength > 0)
                {
                    var filename = HttpContext.SaveImage(file, true, true);
                    DataRepository.SetUserAvatar(filename, loggedUserGuid);
                }
            }

            return RedirectToRoute("myuser", new { action = "edit" });
        }
    }
}
