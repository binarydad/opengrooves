using System.ComponentModel;

namespace OpenGrooves.Web.Areas.Edit.Models
{
    public class UserSettings
    {
        [DisplayName("Notify Of New Messages")]
        public bool NotifyNewMessage { get; set; }

        [DisplayName("Posts a new event")]
        public bool NotifyBandEvent { get; set; }

        [DisplayName("Adds new photos")]
        public bool NotifyBandPhotos { get; set; }

        [DisplayName("Has an event that gets updated")]
        public bool NotifyEventUpdated { get; set; }

        [DisplayName("Updates their profile")]
        public bool NotifyBandProfileUpdate { get; set; }

        [DisplayName("Gets added to an event")]
        public bool NotifyEventBandsAdded { get; set; }
    }
}