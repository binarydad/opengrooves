using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OpenGrooves.Web.Models
{
    public class EventModel : LocationAwareModel
    {
        [ScaffoldColumn(false)]
        public Guid EventId { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(500)]
        public string Description { get; set; }

        public string AvatarUrl { get; set; }

        [DisplayName("Venue Name")]
        public string VenueName { get; set; }

        [DisplayName("Other Bands")]
        public string OtherBands { get; set; }

        [ScaffoldColumn(false)]
        public string UrlName { get; set; }
        public IEnumerable<BandEventRelation> Bands { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [ScaffoldColumn(false)]
        public IEnumerable<UserModel> Users { get; set; }

        public VenueModel Venue { get; set; }
    }
}