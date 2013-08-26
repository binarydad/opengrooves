using OpenGrooves.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OpenGrooves.Web.Models
{
    public class BandLocationModel
    {
        public BandModel Band { get; set; }
        public int Radius { get; set; }
        public Coordinate Coordinate { get; set; }
    }

    [System.Web.Mvc.Bind(Exclude = "Genres")]
    public class BandModel : LocationAwareModel
    {
        [ScaffoldColumn(false)]
        public Guid BandId { get; set; }

        [Required]
        public string Name { get; set; }

        [ScaffoldColumn(false)]
        public string UrlName { get; set; }

        [ScaffoldColumn(false)]
        public string AvatarUrl { get; set; }

        [ScaffoldColumn(false)]
        public DateTime Date { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Website { get; set; }

        [DisplayName("Shows and Events")]
        public IEnumerable<BandEventRelation> ActiveEvents { get; set; }

        [DisplayName("Pending Event Requests")]
        public IEnumerable<BandEventRelation> PendingEvents { get; set; }

        public IEnumerable<BandEventRelation> AllEvents { get; set; }

        public IEnumerable<GalleryModel> Galleries { get; set; }
        public IEnumerable<ImageModel> Images { get; set; }

        [DisplayName("Genres")]
        public IEnumerable<GenreModel> Genres { get; set; }

        [DisplayName("Band Buds")]
        public IEnumerable<UserBandRelation> Fans { get; set; }

        [DisplayName("Members")]
        public IEnumerable<UserBandRelation> ActiveMembers { get; set; }

        [DisplayName("Pending Member Requests")]
        public IEnumerable<UserBandRelation> PendingMemberRequests { get; set; }

        [DisplayName("Pending Member Invites")]
        public IEnumerable<UserBandRelation> PendingMemberInvites { get; set; }

        public IEnumerable<UserBandRelation> AllUsers { get; set; }

        public int NumMembers { get; set; }
        public int NumFans { get; set; }
    }

    public class BandProfileModel
    {
        public enum FollowStatus
        {
            Following,
            Member,
            Pending,
            NoRelation
        }

        public BandModel Band { get; set; }
        [DisplayName("Recent Activity")]
        public IEnumerable<FeedItemModel> Feed { get; set; }
        public FollowStatus Status { get; set; } 
    }

    public class GenreModel
    {
        public Guid GenreId { get; set; }
        public string Name { get; set; }
        public IEnumerable<BandModel> Bands { get; set; }
    }
}