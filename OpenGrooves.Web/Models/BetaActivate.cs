using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OpenGrooves.Web.Models
{
    public class BetaActivate
    {
        [Required]
        [RegularExpression("[A-Za-z0-9]+", ErrorMessage = "Please enter a valid username.")]
        public string Username { get; set; }

        [Required(ErrorMessage="Please choose a password")]
        [DataType(DataType.Password)]
        [DisplayName("Create Password")]
        public string NewPassword { get; set; }

        public string Email { get; set; }
    }
}