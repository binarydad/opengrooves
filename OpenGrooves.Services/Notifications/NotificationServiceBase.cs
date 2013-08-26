using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using OpenGrooves.Core;
using StructureMap;
using StructureMap.Attributes;
using OpenGrooves.Services.Email;

namespace OpenGrooves.Services.Notifications
{
    [PluginFamily("Notifications")]
    public abstract class EmailNotificationServiceBase
    {
        public abstract void SendNotifications(NotificationType type, IEnumerable<Guid> userIds, MessageData data, bool ignoreSettings = false);
        public abstract void SendBandNotifications(NotificationType type, Guid bandId, BandMessageData data);
        public abstract void SendEventNotifications(NotificationType type, Guid eventId, EventMessageData data);

        [SetterProperty]
        public IEmailService EmailService { get; set; }

        protected void SendMessage(MailMessage msg, IEnumerable<string> emails)
        {
            // async this: 
            // SEE: http://www.csharp-examples.net/create-asynchronous-method/

            emails.Distinct().ToList().ForEach(e => 
            {
                msg.To.Add(e);
                EmailService.Send(msg);
                msg.To.Clear();
            });
        }

        protected IDictionary<string, string> BuildReplacements<T>(T data)
        {
            var replacements = new Dictionary<string, string>();

            data.GetType().GetProperties().ToList().ForEach(p =>
            {
                var value = p.GetValue(data, null);
                if (value != null)
                {
                    replacements.Add(p.Name.ToUpper(), value as string);
                }
            });

            return replacements;
        }

        private readonly object _sync = new object();
    }
}
