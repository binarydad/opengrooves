using System;
using System.ComponentModel.DataAnnotations;

namespace OpenGrooves.Web.Models
{
    public class VenueModel : LocationAwareModel
    {
        [ScaffoldColumn(false)]
        public Guid VenueId { get; set; }

        [ScaffoldColumn(false)]
        public string UrlName { get; set; }

        [Required]
        public string Name { get; set; }
    }
}