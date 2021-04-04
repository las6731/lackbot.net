using System;

namespace LackBot.Common.Models
{
    /// <summary>
    /// Object containing the elements of a Discord message that might be matched against.
    /// </summary>
    public class MessageDetails
    {
        /// <summary>
        /// The body of the message.
        /// </summary>
        public string Content { get; set; }
        
        /// <summary>
        /// The id of the author who sent the message.
        /// </summary>
        public ulong AuthorId { get; set; }
        
        /// <summary>
        /// The id of the channel where the message was sent.
        /// </summary>
        public ulong ChannelId { get; set; }
        
        /// <summary>
        /// The timestamp of when the message was sent (in UTC).
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }
    }
}