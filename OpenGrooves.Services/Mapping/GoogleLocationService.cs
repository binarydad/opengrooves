using System;
using System.Net;
using System.IO;
using System.Xml;
using StructureMap;
using OpenGrooves.Core;

namespace OpenGrooves.Services.Mapping
{
    [Pluggable("Location")]
    public class GoogleLocationService : ILocationService
    {
        private const string ApiUrl = "http://maps.google.com/maps/api/geocode/xml?address={0}&sensor=true";

        private Location CacheLookup(string address)
        {
            // determine type? zip, city/state, etc
            // query database, SP?
            // build model from local

            return null;
        }

        private void CacheSave(Location coord)
        {
            return;
        }

        public Location GetLocation(string address)
        {
            if (String.IsNullOrWhiteSpace(address)) return null;

            // cached lookup
            var cachedCoord = CacheLookup(address);

            if (cachedCoord != null)
            {
                return cachedCoord;
            }

            try
            {
                var client = new WebClient();
                var stream = client.OpenRead(String.Format(ApiUrl, address));
                using (var reader = new StreamReader(stream))
                {
                    var result = reader.ReadToEnd();

                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(result);

                    var coordsXml = xmlDoc.SelectSingleNode("/GeocodeResponse/result/geometry/location");

                    var latXml = coordsXml.SelectSingleNode("lat").InnerText;
                    var lngXml = coordsXml.SelectSingleNode("lng").InnerText;

                    var addressInfo = xmlDoc.SelectNodes("/GeocodeResponse/result/address_component");

                    string city = String.Empty, state = String.Empty, zip = String.Empty;

                    foreach (XmlNode info in addressInfo)
                    {
                        var type = info.SelectSingleNode("type").InnerText;
                        var value = info.SelectSingleNode("short_name").InnerText;
                        if (type == "locality")
                        {
                            city = value;
                        }
                        else if (type == "postal_code")
                        {
                            zip = value;
                        }
                        else if (type == "administrative_area_level_1")
                        {
                            state = value;
                        }
                    }

                    float lat, lng;

                    float.TryParse(latXml, out lat);
                    float.TryParse(lngXml, out lng);

                    var location = new Location
                    {
                        City = city,
                        State = state,
                        Zip = zip,
                        Coordinate = new Coordinate
                        {
                            Latitude = lat,
                            Longitude = lng
                        }
                    };

                    CacheSave(location);

                    return location;
                }
            }
            catch
            {
                // the caller will handle null location
                return null;
            }
        }
    }
}
