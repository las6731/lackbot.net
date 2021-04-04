using Newtonsoft.Json;

namespace LackBot.Common.Models.AutoReacts
{
    /// <summary>
    /// Builds a new <see cref="AutoReact"/> based on the parameters included in an API request. 
    /// </summary>
    public class AutoReactBuilder : IBuilder<AutoReact>
    {
        /// <summary>
        /// The phrase to match.
        /// </summary>
        public string Phrase { get; }
        
        /// <summary>
        /// The emoji to react with.
        /// </summary>
        public string Emoji { get; }
        
        /// <summary>
        /// The type of <see cref="AutoReact"/> to build. Must be a type defined by <see cref="AutoReactTypes"/>
        /// </summary>
        public string Type { get; }
        
        /// <summary>
        /// The id of an author to match with (if provided).
        /// </summary>
        public ulong Author { get; }

        [JsonConstructor]
        public AutoReactBuilder(string phrase, string emoji, ulong author = 0, string type = AutoReactTypes.Naive)
        {
            Phrase = phrase;
            Phrase ??= string.Empty;
            Emoji = emoji;
            Author = author;
            Type = type;
        }

        public AutoReactBuilder(ulong author, string emoji)
        {
            Phrase = string.Empty;
            Author = author;
            Emoji = emoji;
            Type = AutoReactTypes.Author;
        }

        public AutoReactBuilder(ulong author, string phrase, string emoji)
        {
            Phrase = phrase;
            Author = author;
            Emoji = emoji;
            Type = AutoReactTypes.Author;
        }

        public AutoReact Build()
        {
            return Type switch
            {
                AutoReactTypes.Strong => new StrongAutoReact(Phrase, Emoji),
                AutoReactTypes.Author => new AuthorAutoReact(Phrase, Emoji, Author),
                _ => new AutoReact(Phrase, Emoji)
            };
        }
    }
}