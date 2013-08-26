using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OpenGrooves.Web.Models
{
    public class UserModel : LocationAwareModel
    {
        [ScaffoldColumn(false)]
        public Guid UserId { get; set; }
        
        [ScaffoldColumn(false)]
        public string UserName { get; set; }

        [DisplayName("Real Name")]
        [RegularExpression(@"^([a-zA-Z- ']+)$", ErrorMessage = "Invalid name")]
        public string RealName { get; set; }
        
        [DisplayName("Email Address")]
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid email adddress")]
        public string Email { get; set; }

        public string AvatarUrl { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string AIM { get; set; }

        [StringLength(200)]
        public string Interests { get; set; }

        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }

        [DisplayName("My Connections")]
        public IEnumerable<UserBandRelation> FollowingBands { get; set; }

        [DisplayName("My Bands")]
        public IEnumerable<UserBandRelation> MemberOfBands { get; set; }

        [DisplayName("Pending Band Invites")]
        public IEnumerable<UserBandRelation> PendingBandInvites { get; set; }

        [DisplayName("Pending Band Requests")]
        public IEnumerable<UserBandRelation> PendingBandRequests { get; set; }

        public IEnumerable<UserBandRelation> AllBands { get; set; }

        public IEnumerable<EventModel> Events { get; set; }
        public IEnumerable<VenueModel> Venues { get; set; }
        public IEnumerable<ImageModel> Images { get; set; }
        public IEnumerable<GalleryModel> Galleries { get; set; }
        public IEnumerable<Setting> Settings { get; set; }
    }

    public class Setting
    {
        public Guid UserId { get; set; }
        public string Key { get; set;  }
        public string Value { get; set; }
    }
}