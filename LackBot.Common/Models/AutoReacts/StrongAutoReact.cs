using System.Text.RegularExpressions;
using Discord.WebSocket;
using MongoDB.Bson.Serialization.Attributes;

namespace LackBot.Common.Models.AutoReacts
{
    [BsonDiscriminator("Strong")]
    public class StrongAutoReact : AutoReact
    {
        public StrongAutoReact(string phrase, string emoji): base(phrase, emoji) {}
        
        public override bool Matches(SocketMessage msg)
        {
            return Regex.IsMatch(msg.Content, $"\b{Phrase}\b");
        }
    }
}