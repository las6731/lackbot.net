﻿using System;
using LShort.Common.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace LackBot.Common.Models.AutoReacts
{
    [BsonKnownTypes(typeof(StrongAutoReact), typeof(AuthorAutoReact))]
    [BsonDiscriminator(AutoReactTypes.Naive, RootClass = true)]
    public class AutoReact : ModelBase
    {
        [BsonElement("phrase")]
        public string Phrase { get; set; }
        
        [BsonElement("emoji")]
        public string Emoji { get; set; }
        
        [BsonElement("type")]
        public string Type { get; set; }

        public AutoReact(string phrase, string emoji)
        {
            if (phrase == string.Empty) throw new ArgumentException("Phrase must not be empty");
            if (emoji == string.Empty) throw new ArgumentException("Emoji must not be empty");
            
            Phrase = phrase;
            Emoji = emoji;
            Type = AutoReactTypes.Naive;
        }

        public virtual bool Matches(MessageDetails msg)
        {
            return msg.Content.Contains(Phrase);
        }
    }

    public static class AutoReactTypes
    {
        public const string Naive = "Naive";
        public const string Strong = "Strong";
        public const string Author = "Author";
    }
}