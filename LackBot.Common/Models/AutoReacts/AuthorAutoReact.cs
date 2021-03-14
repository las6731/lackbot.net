using Discord.WebSocket;
using MongoDB.Bson.Serialization.Attributes;

namespace LackBot.Common.Models.AutoReacts
{
    [BsonDiscriminator("Author")]
    public class AuthorAutoReact : AutoReact
    {
        [BsonElement("author")]
        public ulong Author { get; set; }

        public AuthorAutoReact(string phrase, string emoji, ulong author) : base(phrase, emoji)
        {
            Author = author;
        }
        
        public override bool Matches(SocketMessage msg)
        {
            return base.Matches(msg) && msg.Author.Id == Author;
        }
    }
}