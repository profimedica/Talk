using System;

namespace TalkerLibrary
{
    public class Message
    {
        public bool IsReceived { get; set; }
        public Int32 Timestamp { get; internal set; }
        public string From { get; internal set; }
        public string Text { get; set; }
    }
}