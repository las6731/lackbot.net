using System;
using System.Collections.Generic;
using System.Linq;
using LShort.Common.Models;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace LackBot.Common.Models.AutoResponses
{
    [BsonKnownTypes(typeof(StrongAutoResponse), typeof(TimeBasedAutoResponse), typeof(TimeBasedYesNoAutoResponse))]
    [BsonDiscriminator(AutoResponseTypes.Naive, RootClass = true)]
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
        
        [JsonConstructor]
        public AutoResponse(string phrase, IList<string> responses)
        {
            if (phrase == string.Empty) throw new ArgumentException("Phrase must not be empty");
            if (responses.Any(r => r == string.Empty)) throw new ArgumentException("No response may be empty");
            
            Phrase = phrase;
            Responses = responses;
        }

        public virtual bool Matches(MessageDetails msg)
        {
            return msg.Content.ToLower().Contains(Phrase);
        }

        public virtual string GetResponse(MessageDetails msg)
        {
            var random = new Random();

            var index = random.Next(Responses.Count);

            return Responses[index];
        }
    }

    public static class AutoResponseTypes
    {
        public const string Naive = "Naive";
        public const string Strong = "Strong";
        public const string TimeBased = "TimeBased";
        public const string TimeBasedYesNo = "TimeBasedYesNo";
    }
}