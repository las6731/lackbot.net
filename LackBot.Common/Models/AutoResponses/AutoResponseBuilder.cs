using System.Collections.Generic;
using System.Text;

namespace LackBot.Common.Models.AutoResponses
{
    public class AutoResponseBuilder : IBuilder<AutoResponse>
    {
        public string Phrase { get; }
        public IList<string> Responses { get; }
        public string Type { get; }
        
        public string TimeSchedule { get; }

        public AutoResponseBuilder(string phrase, IList<string> responses, string type = AutoResponseTypes.Naive, string timeSchedule = null)
        {
            Phrase = phrase;
            Responses = responses;
            Type = type;
            TimeSchedule = timeSchedule;
        }

        public AutoResponse Build()
        {
            return Type switch
            {
                AutoResponseTypes.TimeBasedYesNo => new TimeBasedYesNoAutoResponse(Phrase, Responses, TimeSchedule),
                AutoResponseTypes.TimeBased => new TimeBasedAutoResponse(Phrase, Responses, TimeSchedule),
                AutoResponseTypes.Strong => new StrongAutoResponse(Phrase, Responses),
                _ => new AutoResponse(Phrase, Responses)
            };
        }
    }
}