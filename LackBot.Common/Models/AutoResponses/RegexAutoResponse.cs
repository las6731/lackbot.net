using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MongoDB.Bson.Serialization.Attributes;

namespace LackBot.Common.Models.AutoResponses
{
    /// <summary>
    /// A response that will be sent automatically whenever the message matches a regular expression.
    /// Capturing groups in the expression are made available to the response to use.
    /// </summary>
    [BsonDiscriminator(AutoResponseTypes.Regex)]
    public class RegexAutoResponse : AutoResponse
    {
        public RegexAutoResponse(string phrase, IList<string> responses) : base(phrase, responses) { }

        public override bool Matches(MessageDetails msg) => Regex.IsMatch(msg.Content, Phrase);

        public override string GetResponse(MessageDetails msg)
        {
            var template = base.GetResponse(msg);

            var regex = new Regex(Phrase);
            var matches = regex.Match(msg.Content).Groups.Values
                .Skip(1)
                .Select(g => g.Value);

            return string.Format(template, matches.ToArray());
        }
    }
}