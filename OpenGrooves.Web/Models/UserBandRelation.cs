using System;
using System.ComponentModel;

namespace OpenGrooves.Web.Models
{
    public class UserBandRelation
    {
        public Guid RelationId { get; set; }
        public bool IsActive { get; set; }
        public int RelationTypeId { get; set; }
        public Guid UserId { get; set; }
        public Guid BandId { get; set; }
        public Guid MemberTypeId { get; set; }
        public UserModel User { get; set; }
        public BandModel Band { get; set; }
        public MemberTypeRelation MemberType { get; set; }
    }

    public class BandEventRelation
    {
        public Guid RelationId { get; set; }
        public bool IsActive { get; set; }
        public EventModel Event { get; set; }
        public Guid EventId { get; set; }
        public BandModel Band { get; set; }
        public Guid BandId { get; set; }
        public DateTime? ShowTime { get; set; }

        [DisplayName("Lineup Order")]
        public int? Order { get; set; }
    }

    public class MemberTypeRelation
    {
        public Guid MemberTypeId { get; set; }
        public string Name { get; set; }
    }
}