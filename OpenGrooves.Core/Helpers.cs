using OpenGrooves.Core.Extensions;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace OpenGrooves.Core.Helpers
{
    public static class UrlGuidHelper
    {
        public static string GetUrlString(Guid g)
        {
            return Convert.ToBase64String(g.ToByteArray()).Replace("/", "-").Replace("+", "_").Replace("=", String.Empty);
        }

        public static Guid GetGuid(string g)
        {
            g = g.Replace("-", "/").Replace("_", "+") + "==";

            var guid = new Guid(Convert.FromBase64String(g));

            return guid;
        }
    }

    public static class EntityNameHelper
    {
        public static string CreateUrl(string data, bool makeUnique = false)
        {
            if (!data.IsNullOrWhiteSpace())
            {
                var formatted = Regex.Replace(data, "[^a-zA-Z0-9 ]", "").ToLower().Replace(" ", "-");

                if (makeUnique)
                {
                    return String.Format("{0}-{1}", Guid.NewGuid(), formatted);
                }

                return formatted;
            }

            return String.Empty;
        }

        public static string StripUselessWords(string data)
        {
            if (!data.IsNullOrWhiteSpace())
            {
                if (data.StartsWith("the", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = data.Substring(4);
                }

                if (data.StartsWith("a", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = data.Substring(2);
                }
            }

            return data;
        }
    }

    public static class EmailHelper
    {
        private static string fromEmail = "noreply@opengrooves.com";

        public static void SendEmail(string to, string subject, string body)
        {
            using (var msg = new MailMessage(fromEmail, to, subject, body) { IsBodyHtml = true })
            {
                SendEmail(msg);
            }
        }

        public static void SendEmail(MailMessage msg)
        {
            if (msg.To.Count > 0)
            {
                var smtp = new SmtpClient();
                smtp.Send(msg);
            }
        }
    }

    public static class LocationHelper
    {
        public static int Radius(float lat1, float long1, float lat2, float long2)
        {
            return Radius(new Coordinate { Latitude = lat1, Longitude = long1 }, new Coordinate { Latitude = lat2, Longitude = long2 });
        }

        public static int Radius(Coordinate p1, Coordinate p2)
        {
            if (p1 == null) throw new ArgumentNullException("p1");
            if (p2 == null) throw new ArgumentNullException("p2");

            return (int)(Math.Acos(Math.Cos(DegreeToRadian(p1.Latitude)) * Math.Cos(DegreeToRadian(p2.Latitude)) * Math.Cos(DegreeToRadian(p2.Longitude) - DegreeToRadian(p1.Longitude)) + Math.Sin(DegreeToRadian(p1.Latitude)) * Math.Sin(DegreeToRadian(p2.Latitude))) * 3959);
        }

        private static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static string CityStateOrZip(Location loc)
        {
            return CityStateOrZip(loc.City, loc.State, loc.Zip);
        }

        public static string CityStateOrZip(string city = null, string state = null, string zip = null)
        {
            var validZip = !String.IsNullOrWhiteSpace(zip);
            var validCity = !String.IsNullOrWhiteSpace(city);
            var validState = !String.IsNullOrWhiteSpace(state);

            if (validCity && validState && !validZip)
            {
                return String.Format("{0}, {1}", city, state);
            }
            else if (validZip && validCity && validState)
            {
                return String.Format("{0}, {1} {2}", city, state, zip);
            }
            else if (validZip)
            {
                return zip;
            }
            else
            {
                return String.Empty;
            }
        }
    }
}