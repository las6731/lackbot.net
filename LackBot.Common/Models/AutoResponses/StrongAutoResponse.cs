using System.Collections.Generic;
using System.Text.RegularExpressions;
using MongoDB.Bson.Serialization.Attributes;

namespace LackBot.Common.Models.AutoResponses
{
    /// <summary>
    /// A response that will be sent automatically whenever the message contains a substring surrounded by
    /// word boundaries - such as whitespace, beginning or end of line, etc.
    /// </summary>
    /// <remarks><seealso cref="AutoResponseTypes.Strong"/></remarks>
    [BsonDiscriminator(AutoResponseTypes.Strong)]
    public class StrongAutoResponse : AutoResponse
    {
        public StrongAutoResponse(string phrase, IList<string> responses) : base(phrase, responses) {}
        
        public override bool Matches(MessageDetails msg)
        {
            return Regex.IsMatch(msg.Content.ToLower(), $"\\b{Phrase}\\b");
        }
    }
}