using System.Collections.Generic;
using System.Text.RegularExpressions;
using MongoDB.Bson.Serialization.Attributes;

namespace LackBot.Common.Models.AutoResponses
{
    [BsonDiscriminator(AutoResponseTypes.Strong)]
    public class StrongAutoResponse : AutoResponse
    {
        public StrongAutoResponse(string phrase, string response) : base(phrase, response)
        {
            Type = AutoResponseTypes.Strong;
        }

        public StrongAutoResponse(string phrase, IList<string> responses) : base(phrase, responses)
        {
            Type = AutoResponseTypes.Strong;
        }

        public override bool Matches(MessageDetails msg)
        {
            return Regex.IsMatch(msg.Content, $"\\b{Phrase}\\b");
        }
    }
}