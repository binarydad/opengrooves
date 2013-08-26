using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using StructureMap;

namespace OpenGrooves.Services.Email
{
    [PluginFamily("EmailService")]
    public interface IEmailService
    {
        void Send(MailMessage msg);
    }
}
