using System;
using System.Collections.Generic;
using System.Linq;
using LShort.Common.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace LackBot.Common.Models.AutoResponses
{
    /// <summary>
    /// A response that will be sent automatically whenever the defined criteria are matched.
    /// By default, this matches any message containing <see cref="Phrase"/> as a substring.
    /// </summary>
    /// <remarks><seealso cref="AutoResponseTypes.Naive"/></remarks>
    [BsonKnownTypes(typeof(StrongAutoResponse), typeof(TimeBasedAutoResponse), typeof(TimeBasedYesNoAutoResponse), typeof(RegexAutoResponse))]
    [BsonDiscriminator(AutoResponseTypes.Naive, RootClass = true)]
    public class AutoResponse : ModelBase
    {
        /// <summary>
        /// The phrase to match.
        /// </summary>
        [BsonElement("phrase")]
        public string Phrase { get; set; }
        
        /// <summary>
        /// The response options.
        /// </summary>
        [BsonElement("responses")]
        public IList<string> Responses { get; set; }
        
        public AutoResponse(string phrase, IList<string> responses)
        {
            if (phrase == string.Empty) throw new ArgumentException("Phrase must not be empty");
            if (responses.Any(r => r == string.Empty)) throw new ArgumentException("No response may be empty");
            
            Phrase = phrase;
            Responses = responses;
        }

        /// <summary>
        /// Determine whether the message is a match for this response.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>A bool indicating whether the message matches.</returns>
        public virtual bool Matches(MessageDetails msg)
        {
            return msg.Content.Contains(Phrase, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Get the response for this message.
        /// </summary>
        /// <param name="msg">The message being responded to.</param>
        /// <returns>The message to send as a response.</returns>
        public virtual string GetResponse(MessageDetails msg)
        {
            var random = new Random();

            var index = random.Next(Responses.Count);

            return Responses[index];
        }
    }

    public static class AutoResponseTypes
    {
        /// <summary>
        /// Matches based on a substring.
        /// </summary>
        public const string Naive = "Naive";
        
        /// <summary>
        /// Matches based on a word, meaning the substring must be surrounded by word boundaries (whitespace)
        /// </summary>
        public const string Strong = "Strong";
        
        /// <summary>
        /// Matches based on a substring and a cron expression.
        /// </summary>
        public const string TimeBased = "TimeBased";
        
        /// <summary>
        /// Matches based on a substring; response will be one of two options, depending on whether the message
        /// also matches a cron expression or not.
        /// </summary>
        public const string TimeBasedYesNo = "TimeBasedYesNo";

        /// <summary>
        /// Matches based on a regex pattern. Capturing groups are made available to the response in order of appearance.
        /// </summary>
        public const string Regex = "Regex";
    }
}