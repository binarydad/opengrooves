using System;
using System.Collections.Generic;

namespace OpenGrooves.Web.Models
{
    public class HomeModel
    {
        public IEnumerable<EventModel> Events { get; set; }
        public IEnumerable<EventModel> LocalEvents { get; set; }
        public IEnumerable<BandModel> Bands { get; set; }
        public IEnumerable<BandModel> MyBands { get; set; }
        public IEnumerable<FeedItemModel> Feed { get; set; }
        public IEnumerable<ImageModel> Images { get; set; }
        public IEnumerable<GalleryModel> Galleries { get; set; }
    }

    public class FeedItemModel
    {
        public Guid FeedItemId { get; set; }
        public Guid BandId { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public BandModel Band { get; set; }
        public EventModel Event { get; set; }
        public UserModel User { get; set; }
        public Guid BatchId { get; set; }
        public IEnumerable<ImageModel> Images { get; set; }
        public int FeedItemTypeId { get; set; }
    }
}