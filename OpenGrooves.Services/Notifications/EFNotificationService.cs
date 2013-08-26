using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Objects;
using System.Data.Linq;
using System.Web;
using OpenGrooves.Data;
using System.Net.Mail;
using OpenGrooves.Core;
using StructureMap;

namespace OpenGrooves.Services.Notifications
{
    [Pluggable("Notifications")]
    public class EFNotificationService : EmailNotificationServiceBase
    {
        public override void SendNotifications(NotificationType type, IEnumerable<Guid> userIds, MessageData data, bool ignoreSettings = false)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                string typeStr = type.ToString();

                var emails = (from s in ctx.UserSettings
                            join u in userIds on s.UserId equals u
                            where ignoreSettings ? true : (s.Value == "True" && s.Key.Equals(typeStr, StringComparison.OrdinalIgnoreCase))
                            select s.User.Email).Distinct().ToList();

                var replacements = BuildReplacements(data);
                var msg = TemplateBuilder.LoadTemplate(type, replacements);

                SendMessage(msg, emails);
            }
        }

        public override void SendBandNotifications(NotificationType type, Guid bandId, BandMessageData data)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                string typeStr = type.ToString();

                // Get list of users who are following this band and have notifications on to receive
                ctx.ContextOptions.LazyLoadingEnabled = true;
                var emails = (from r in ctx.UsersBands
                            join s in ctx.UserSettings on r.UserId equals s.UserId
                            where r.BandId == bandId && s.Value == "True" && s.Key.Equals(typeStr, StringComparison.OrdinalIgnoreCase)
                            select s.User.Email).Distinct().AsEnumerable();

                var replacements = BuildReplacements(data);
                var msg = TemplateBuilder.LoadTemplate(type, replacements);

                SendMessage(msg, emails);
            }
        }

        public override void SendEventNotifications(NotificationType type, Guid eventId, EventMessageData data)
        {
            using (var ctx = new OpenGroovesEntities())
            {
                string typeStr = type.ToString();

                // Get list of users who are following this band and have notifications on to receive
                ctx.ContextOptions.LazyLoadingEnabled = true;
                var emails = (from e in ctx.BandsEvents
                            join r in ctx.UsersBands on e.BandId equals r.BandId
                            join s in ctx.UserSettings on r.UserId equals s.UserId
                            where e.EventId == eventId && s.Value == "True" && s.Key.Equals(typeStr, StringComparison.OrdinalIgnoreCase)
                            select s.User.Email).Distinct().AsEnumerable();

                var replacements = BuildReplacements(data);
                var msg = TemplateBuilder.LoadTemplate(type, replacements);

                SendMessage(msg, emails);
            }
        }
    }
}