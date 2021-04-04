using MongoDB.Bson.Serialization.Attributes;

namespace LackBot.Common.Models.AutoReacts
{
    /// <summary>
    /// A reaction that will be sent whenever the message contains a substring and is sent by the specified author.
    /// This is a special case where the substring is allowed to be empty, meaning that it should react to every message
    /// that this author sends.
    /// </summary>
    /// <remarks><seealso cref="AutoReactTypes.Author"/></remarks>
    [BsonDiscriminator(AutoReactTypes.Author)]
    public class AuthorAutoReact : AutoReact
    {
        [BsonElement("author")]
        public ulong Author { get; set; }

        public AuthorAutoReact(string phrase, string emoji, ulong author) : base(phrase, emoji, true)
        {
            Author = author;
        }
        
        public override bool Matches(MessageDetails msg)
        {
            Phrase ??= string.Empty;
            return base.Matches(msg) && msg.AuthorId == Author;
        }
    }
}