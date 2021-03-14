using System;

namespace LackBot.Common.Models
{
    public class MessageDetails
    {
        public string Content { get; set; }
        public ulong AuthorId { get; set; }
        public ulong ChannelId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}