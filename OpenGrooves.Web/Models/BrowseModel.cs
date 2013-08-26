using OpenGrooves.Core;
using System.Collections.Generic;

namespace OpenGrooves.Web.Models
{
    public class BrowseModel
    {
        public IEnumerable<BandModel> Bands { get; set; }
        public IEnumerable<GenreModel> Genres { get; set; }
        public IEnumerable<EventModel> Events { get; set; }
        public int Radius { get; set; }
        public Location Location { get; set; }
        public string Address { get; set; }
        public string PostUrl { get; set; }
    }
}