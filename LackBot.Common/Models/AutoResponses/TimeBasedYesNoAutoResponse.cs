using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace LackBot.Common.Models.AutoResponses
{
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