using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OpenGrooves.Web.Admin.Models
{
    public class ConsoleModel
    {
        public string Usernames { get; set; }
        public string Subject { get; set; }

        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        public IEnumerable<BetaUser> BetaUsers { get; set; }
    }

    public class BetaUser
    {
        public string ActivationUrl { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }
}