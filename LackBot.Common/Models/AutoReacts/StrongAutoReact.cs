using System.Text.RegularExpressions;
using MongoDB.Bson.Serialization.Attributes;

namespace LackBot.Common.Models.AutoReacts
{
    /// <summary>
    /// A reaction that will be sent automatically whenever the message contains a substring surrounded by
    /// word boundaries - such as whitespace, beginning or end of line, etc.
    /// </summary>
    /// <remarks><seealso cref="AutoReactTypes.Strong"/></remarks>
    [BsonDiscriminator(AutoReactTypes.Strong)]
    public class StrongAutoReact : AutoReact
    {
        public StrongAutoReact(string phrase, string emoji) : base(phrase, emoji) {}
        
        public override bool Matches(MessageDetails msg)
        {
            return Regex.IsMatch(msg.Content, $"\\b{Phrase}\\b");
        }
    }
}