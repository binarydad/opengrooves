using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;

namespace OpenGrooves.Services.Email
{
    [Pluggable("EmailService")]
    public class AsyncWcfEmailService : IEmailService
    {
        public void Send(System.Net.Mail.MailMessage msg)
        {
            var service = new WcfServices.EmailServiceClient();
            
            if (msg.To.Count == 1)
            {
                var to = msg.To[0];
                service.BeginSendMail(msg.From.Address, msg.From.DisplayName, to.Address, to.DisplayName, msg.Subject, msg.Body, null, null);
            }
        }
    }
}
