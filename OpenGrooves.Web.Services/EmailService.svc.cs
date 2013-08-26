using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Net.Mail;

namespace OpenGrooves.Web.Services
{
    public class EmailService : IEmailService
    {
        public void SendMail(string fromEmail, string fromName, string toEmail, string toName, string subject, string body)
        {
            var to = new MailAddress(toEmail, toName);
            var from = new MailAddress(fromEmail, fromName);

            var msg = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

            using(var client = new SmtpClient())
            {
                client.Send(msg);
            }
        }
    }
}
