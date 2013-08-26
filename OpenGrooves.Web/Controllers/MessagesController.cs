using OpenGrooves.Core;
using OpenGrooves.Core.Extensions;
using OpenGrooves.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OpenGrooves.Web.Controllers
{
    [Authorize]
    public class MessagesController : BaseController
    {
        public ActionResult Received()
        {
            var model = DataRepository.GetUserReceivedMessages(loggedUserGuid).OrderByDescending(m => m.Message.Date);
            return View(model);
        }

        public ActionResult Sent()
        {
            var model = DataRepository.GetUserSentMessages(loggedUserGuid).OrderByDescending(m => m.Date);
            return View(model);
        }

        public ActionResult View(Guid messageId)
        {
            var message = DataRepository.GetMessage(messageId);
            if (CheckMessageOwnership(message))
            {
                DataRepository.MarkMessageRead(message.MessageId, loggedUserGuid);
                return View(message);
            }

            return RedirectToRoute("Messages", new { action = "list" });
        }

        public ActionResult Create(string to)
        {
            return View((object)to);
        }

        [HttpPost]
        [ActionName("Create")]
        public ActionResult Create(IEnumerable<string> recipient, string message)
        {
            if (recipient != null && recipient.Any() && !message.IsNullOrWhiteSpace())
            {
                var sender = DataRepository.GetUser(loggedUserGuid);
                var recipients = DataRepository.SendMessages(loggedUserGuid, recipient, message);

                // send notification
                Notifications.SendNotifications(NotificationType.NotifyNewMessage, recipients.Select(u => u.UserId).ToArray(), new MessageData { From = sender.UserName, Message = message });
            }

            return JsonValidationResult(ModelState, Url.RouteUrl("messages", new { action = "received" }));
        }
    
        [HttpPost]
        public ActionResult Delete(Guid messageId)
        {
            var message = DataRepository.GetMessage(messageId);
            if (CheckMessageOwnership(message))
            {
                DataRepository.DeleteMessage(messageId, loggedUserGuid);
            }

            return JsonValidationResult(ModelState);
            
        }

        private bool CheckMessageOwnership(MessageModel m)
        {
            return m.Recipients.Any(u => u.UserId == loggedUserGuid) || m.SenderUserId == loggedUserGuid;
        }
    }
}
