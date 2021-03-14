using System.Collections.Generic;

namespace LackBot.Common.Models.AutoResponses
{
    public class AutoResponseBuilder : IBuilder<AutoResponse>
    {
        public string Phrase { get; }
        public IList<string> Responses { get; }
        public string Type { get; }

        public AutoResponseBuilder(string phrase, IList<string> responses)
        {
            Phrase = phrase;
            Responses = responses;
            Type = AutoResponseTypes.Naive;
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