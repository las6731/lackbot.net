using MongoDB.Bson.Serialization.Attributes;

namespace LackBot.Common.Models.AutoReacts
{
    [BsonDiscriminator(AutoReactTypes.Author)]
    public class AuthorAutoReact : AutoReact
    {
        [BsonElement("author")]
        public ulong Author { get; set; }

        public AuthorAutoReact()
        {
            Author = 0;
        }

        public AuthorAutoReact(string phrase, string emoji, ulong author) : base(phrase, emoji, true)
        {
            Author = author;
            Type = AutoReactTypes.Author;
        }
        
        public override bool Matches(MessageDetails msg)
        {
            Phrase ??= string.Empty;
            return base.Matches(msg) && msg.AuthorId == Author;
        }
    }
}