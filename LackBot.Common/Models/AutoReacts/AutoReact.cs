using System;
using Discord.WebSocket;
using LShort.Common.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace LackBot.Common.Models.AutoReacts
{
    [BsonKnownTypes(typeof(StrongAutoReact), typeof(AuthorAutoReact))]
    [BsonDiscriminator("Naive", RootClass = true)]
    public class AutoReact : ModelBase
    {
        [BsonElement("phrase")]
        public string Phrase { get; set; }
        
        [BsonElement("emoji")]
        public string Emoji { get; set; }

        public AutoReact(string phrase, string emoji)
        {
            if (phrase == string.Empty) throw new ArgumentException("Phrase must not be empty");
            if (emoji == string.Empty) throw new ArgumentException("Emoji must not be empty");
            
            Phrase = phrase;
            Emoji = emoji;
        }

        public virtual bool Matches(SocketMessage msg)
        {
            return msg.Content.Contains(Phrase);
        }
    }
}