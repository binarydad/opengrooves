using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenGrooves.Web.Admin.Models;

namespace OpenGrooves.Web.Admin.Services.Email
{
    interface IEmailService
    {
        void SendEmail(ConsoleModel model);
    }
}
