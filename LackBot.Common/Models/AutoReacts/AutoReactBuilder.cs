using System.Text.Json.Serialization;

namespace LackBot.Common.Models.AutoReacts
{
    public class AutoReactBuilder : IBuilder<AutoReact>
    {
        public string Phrase { get; }
        public string Emoji { get; }
        public string Type { get; }
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