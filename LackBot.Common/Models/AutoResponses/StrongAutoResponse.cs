using System.Collections.Generic;
using System.Text.RegularExpressions;
using Discord.WebSocket;
using MongoDB.Bson.Serialization.Attributes;

namespace LackBot.Common.Models.AutoResponses
{
    [BsonDiscriminator("Strong")]
    public class StrongAutoResponse : AutoResponse
    {
        public StrongAutoResponse(string phrase, string response) : base(phrase, response) {}

        public StrongAutoResponse(string phrase, IList<string> responses) : base(phrase, responses) {}

        public override bool Matches(SocketMessage msg)
        {
            return Regex.IsMatch(msg.Content, $"\b{Phrase}\b");
        }
    }
}