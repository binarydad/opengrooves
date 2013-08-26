using System;

namespace OpenGrooves.Web.Models
{
    public class AudioModel
    {
        public Guid AudioID { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime Date { get; set; }
        public BandModel Band { get; set; }
        public Guid BandID { get; set; }
    }
}