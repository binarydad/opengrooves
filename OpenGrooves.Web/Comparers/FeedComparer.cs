using OpenGrooves.Data;
using System.Collections.Generic;

namespace OpenGrooves.Web.Comparers
{
    public class FeedComparer : IEqualityComparer<FeedItem>
    {
        public bool Equals(FeedItem x, FeedItem y)
        {
            bool isEqual;

            if (y.FeedItemTypeId == 3 && x.FeedItemTypeId == 3)
            {
                isEqual =
                    x.EventId == y.EventId &&
                    x.Date.Date == y.Date.Date &&
                    x.Date.Hour == y.Date.Hour &&
                    x.Content == y.Content &&
                    x.FeedItemTypeId == y.FeedItemTypeId;
            }
            else
            {
                isEqual =
                    x.FeedItemTypeId == y.FeedItemTypeId &&
                    x.Date.Date == y.Date.Date &&
                    x.BandId == y.BandId &&
                    x.Content == y.Content &&
                    x.Date.Hour == y.Date.Hour;
            }

            return isEqual;
        }

        public int GetHashCode(FeedItem obj)
        {
            var date = obj.Date.Date.GetHashCode();
            var hour = obj.Date.Hour.GetHashCode();
            var bandId = obj.BandId.GetHashCode();
            var eventId = obj.EventId.GetHashCode();
            var content = obj.Content.GetHashCode();
            var type = obj.FeedItemTypeId.GetHashCode();

            if (obj.FeedItemTypeId == 3)
            {
                return date * hour * type * eventId * content;
            }
            else
            {
                return date * hour * type * bandId * content;
            }
        }
    }
}