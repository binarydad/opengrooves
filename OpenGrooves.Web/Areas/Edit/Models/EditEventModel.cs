using OpenGrooves.Web.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace OpenGrooves.Web.Areas.Edit.Models
{
    public class EditEventModel
    {
        public EventModel Event { get; set; }

        [DisplayName("My Bands")]
        public IEnumerable<BandModel> MyBands { get; set; }
        public IEnumerable<BandModel> OtherBands { get; set; }
    }
}