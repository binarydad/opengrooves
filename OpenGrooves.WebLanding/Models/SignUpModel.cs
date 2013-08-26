using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OpenGrooves.WebLanding.Models
{
    public class SignUpModel
    {
        [DisplayName("Your Name")]
        [RegularExpression("[A-Za-z- ']+", ErrorMessage="Please enter a valid first name.")]
        public string RealName { get; set; }

        [Required(ErrorMessage="No email address entered.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid email adddress.")]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [DisplayName("Your Band's Name")]
        [RegularExpression("[A-Za-z0-9-_ ']+", ErrorMessage = "Please enter a valid band name.")]
        public string BandName { get; set; }

        [RegularExpression(@"^([a-zA-Z- ']+)$", ErrorMessage = "Invalid city.")]
        public string City { get; set; }

        public State State { get; set; }
    }

    public class State
    {
        [RegularExpression("^[a-zA-Z]{2}$", ErrorMessage = "Invalid state.")]
        [DisplayName("State")]
        public string Name { get; set; }
    }
}