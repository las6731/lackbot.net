﻿using System;
using System.Collections.Generic;
using Cronos;
using LackBot.Common.Extensions;
using MongoDB.Bson.Serialization.Attributes;

namespace LackBot.Common.Models.AutoResponses
{
    [BsonDiscriminator(AutoResponseTypes.TimeBased)]
    public class TimeBasedAutoResponse : AutoResponse
    {
        [BsonElement("timeSchedule")]
        public string TimeSchedule { get; set; }

        public TimeBasedAutoResponse(string phrase, string response, string timeSchedule) : base(phrase, response)
        {
            var cron = CronExpression.Parse(timeSchedule);
            if (cron.GetNextOccurrence(DateTime.Now) is null) throw new ArgumentException("Invalid cron schedule!");

                TimeSchedule = timeSchedule;
        }
        
        public TimeBasedAutoResponse(string phrase, IList<string> responses, string timeSchedule) : base(phrase, responses)
        {
            var cron = CronExpression.Parse(timeSchedule);
            if (cron.GetNextOccurrence(DateTime.UtcNow) is null)
            {
                throw new ArgumentException("Invalid cron schedule!");
            }
            
            TimeSchedule = timeSchedule;
        }

        public override bool Matches(MessageDetails msg)
        {
            if (!base.Matches(msg)) return false;

            var time = msg.Timestamp.Truncate(TimeSpan.TicksPerMinute);
            var cron = CronExpression.Parse(TimeSchedule);
            var tz = TimeZoneInfo.Local;

            return time == cron.GetNextOccurrence(time.AddMinutes(-1), tz);
        }
    }
}