using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace LackBot.Common.Models.AutoResponses
{
    /// <summary>
    /// A response that will be sent automatically whenever the message contains the defined substring.
    /// Must have exactly two responses. The first will be sent if the cron expression matches the message timestamp
    /// (rounded to the minute), the second will be sent if it does not.
    /// </summary>
    /// <remarks><seealso cref="AutoResponseTypes.TimeBasedYesNo"/></remarks>
    [BsonDiscriminator(AutoResponseTypes.TimeBasedYesNo)]
    public class TimeBasedYesNoAutoResponse : TimeBasedAutoResponse
    {
        public TimeBasedYesNoAutoResponse(string phrase, IList<string> responses, string timeSchedule) : base(phrase,
            responses, timeSchedule)
        {
            if (responses.Count != 2) throw new ArgumentException("Yes/no response must have exactly 2 responses!");
        }

        public override bool Matches(MessageDetails msg)
        {
            return msg.Content.ToLower().Contains(Phrase);
        }

        public override string GetResponse(MessageDetails msg) => base.Matches(msg) ? Responses[0] : Responses[1];
    }
}