﻿using System;
using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;
using LShort.Common.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace LackBot.Common.Models.AutoResponses
{
    [BsonKnownTypes(typeof(StrongAutoResponse))]
    [BsonDiscriminator("Naive", RootClass = true)]
    public class AutoResponse : ModelBase
    {
        [BsonElement("phrase")]
        public string Phrase { get; set; }
        
        [BsonElement("responses")]
        public IList<string> Responses { get; set; }

        public AutoResponse(string phrase, string response)
        {
            if (phrase == string.Empty) throw new ArgumentException("Phrase must not be empty");
            if (response == string.Empty) throw new ArgumentException("Response must not be empty");
            
            Phrase = phrase;
            Responses = new List<string>
            {
                response
            };
        }

        public AutoResponse(string phrase, IList<string> responses)
        {
            if (phrase == string.Empty) throw new ArgumentException("Phrase must not be empty");
            if (responses.Any(r => r == string.Empty)) throw new ArgumentException("No response may be empty");
            
            Phrase = phrase;
            Responses = responses;
        }

        public virtual bool Matches(SocketMessage msg)
        {
            return msg.Content.Contains(Phrase);
        }

        public virtual string GetResponse()
        {
            var random = new Random();

            var index = random.Next(Responses.Count);

            return Responses[index];
        }
    }
}