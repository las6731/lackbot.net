using System;
using LShort.Common.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace LackBot.Common.Models.AutoReacts
{
    /// <summary>
    /// A reaction that will automatically be added to any message that matches the defined criteria.
    /// By default, this matches any message containing <see cref="Phrase"/> as a substring.
    /// </summary>
    /// <remarks><seealso cref="AutoReactTypes.Naive"/></remarks>
    [BsonKnownTypes(typeof(StrongAutoReact), typeof(AuthorAutoReact))]
    [BsonDiscriminator(AutoReactTypes.Naive, RootClass = true)]
    public class AutoReact : ModelBase
    {
        /// <summary>
        /// The phrase to match.
        /// </summary>
        [BsonElement("phrase")]
        public string Phrase { get; set; }
        
        /// <summary>
        /// The emoji to react with.
        /// </summary>
        [BsonElement("emoji")]
        public string Emoji { get; set; }

        public AutoReact(string phrase, string emoji, bool allowEmptyPhrase = false)
        {
            if (phrase == string.Empty && !allowEmptyPhrase) throw new ArgumentException("Phrase must not be empty");
            if (emoji == string.Empty) throw new ArgumentException("Emoji must not be empty");
            
            Phrase = phrase;
            Emoji = emoji;
        }

        /// <summary>
        /// Determines whether the message is a match for this reaction.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>A bool indicating whether the message matches.</returns>
        public virtual bool Matches(MessageDetails msg)
        {
            return msg.Content.Contains(Phrase);
        }
    }

    public static class AutoReactTypes
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
        /// Matches based on a substring, and must be sent by a specific author. This is a special case where the
        /// substring is allowed to be empty (to react to every message sent by that author).
        /// </summary>
        public const string Author = "Author";
    }
}