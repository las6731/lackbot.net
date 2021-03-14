using System.Collections.Generic;

namespace LackBot.Common.Models.AutoResponses
{
    public class AutoResponseBuilder : IBuilder<AutoResponse>
    {
        public string Phrase { get; }
        public IList<string> Responses { get; }
        public string Type { get; }

        public AutoResponseBuilder(string phrase, IList<string> responses, string type = AutoResponseTypes.Naive)
        {
            Phrase = phrase;
            Responses = responses;
            Type = type;
        }

        public AutoResponse Build()
        {
            return Type switch
            {
                AutoResponseTypes.Strong => new StrongAutoResponse(Phrase, Responses),
                _ => new AutoResponse(Phrase, Responses)
            };
        }
    }
}