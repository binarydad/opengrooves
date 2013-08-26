using System;

namespace OpenGrooves.Core
{
    public class MessageData
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string CustomData { get; set; }
        public string Link { get; set; }
    }

    public class BandMessageData : MessageData
    {
        public string Band { get; set; }
    }

    public class EventMessageData : MessageData
    {
        public string Event { get; set; }
        public DateTime Date { get; set; }
        public string Bands { get; set; }
    }
}