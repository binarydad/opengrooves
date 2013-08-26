using OpenGrooves.Core;
using System.ComponentModel.DataAnnotations;

namespace OpenGrooves.Web.Models
{
    public class LocationAwareModel
    {
        public Location Location
        {
            get
            {
                return new Location
                {
                    Address = this.Address,
                    City = this.City,
                    State = this.State,
                    Zip = this.Zip,
                    Coordinate = new Coordinate
                    {
                        Latitude = this.Latitude,
                        Longitude = this.Longitude
                    }
                };
            }
        }

        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public string Address { get; set; }

        [Required]
        [RegularExpression(@"^([a-zA-Z- ']+)$", ErrorMessage = "Invalid city")]
        public string City { get; set; }
        
        [Required]
        [RegularExpression("^[a-zA-Z]{2}$", ErrorMessage="Invalid state")]
        public string State { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid zip code")]
        public string Zip { get; set; }
    }
}
