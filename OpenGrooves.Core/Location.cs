using OpenGrooves.Core.Extensions;

namespace OpenGrooves.Core
{
    public class Location
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public Coordinate Coordinate { get; set; }
        public bool HasLocation 
        { 
            get
            {
                return !this.City.IsNullOrWhiteSpace() && !this.State.IsNullOrWhiteSpace();
            }
        }
    }
}
