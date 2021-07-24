using System.Collections.Generic;

namespace LackBot.Common.Models.AutoResponses
{
    /// <summary>
    /// Builds a new <see cref="AutoResponse"/> based on the parameters included in an API request. 
    /// </summary>
    public class AutoResponseBuilder : IBuilder<AutoResponse>
    {
        /// <summary>
        /// The phrase to match.
        /// </summary>
        public string Phrase { get; }
        
        /// <summary>
        /// The response options.
        /// </summary>
        public IList<string> Responses { get; }
        
        /// <summary>
        /// The type of <see cref="AutoResponse"/> to build. Must be a type defined by <see cref="AutoResponseTypes"/>
        /// </summary>
        public string Type { get; }
        
        /// <summary>
        /// The cron expression to match with (if provided).
        /// </summary>
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
                AutoResponseTypes.Regex => new RegexAutoResponse(Phrase, Responses),
                AutoResponseTypes.TimeBasedYesNo => new TimeBasedYesNoAutoResponse(Phrase, Responses, TimeSchedule),
                AutoResponseTypes.TimeBased => new TimeBasedAutoResponse(Phrase, Responses, TimeSchedule),
                AutoResponseTypes.Strong => new StrongAutoResponse(Phrase, Responses),
                _ => new AutoResponse(Phrase, Responses)
            };
        }
    }
}