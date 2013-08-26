using System;
using System.Collections.Generic;

namespace OpenGrooves.Web.Models
{
    public class BandsListModel
    {
        public IEnumerable<BandModel> Bands { get; set; }
        public Func<BandModel, string> RightColumnData { get; set; }
    }
}