using System.Text.RegularExpressions;
using MongoDB.Bson.Serialization.Attributes;

namespace LackBot.Common.Models.AutoReacts
{
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