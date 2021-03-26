using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cronos;
using LShort.Common.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace LackBot.Common.Models.ScheduledMessage
{
    public class ScheduledMessage : ModelBase, IDisposable
    {
        [BsonElement("channelId")]
        public ulong ChannelId { get; set; }
        
        [BsonElement("timeSchedule")]
        public string TimeSchedule { get; set; }
        
        [BsonElement("messages")]
        public IList<string> Messages { get; set; }

        private System.Timers.Timer timer;

        public ScheduledMessage(ulong channelId, string timeSchedule, IList<string> messages)
        {
            if (channelId == 0) throw new ArgumentException("A valid channelId is required!");
            if (messages.Any(m => m == string.Empty)) throw new ArgumentException("No message may be empty!");

            var cron = CronExpression.Parse(timeSchedule);
            if (cron.GetNextOccurrence(DateTime.UtcNow) is null) throw new ArgumentException("Invalid cron schedule!");

            ChannelId = channelId;
            TimeSchedule = timeSchedule;
            Messages = messages;
        }

        public async Task BeginTimer(Func<Guid, Task> task, Action<Guid> handleOutOfRange, CancellationToken cancellationToken)
        {
            var cron = CronExpression.Parse(TimeSchedule);
            var next = cron.GetNextOccurrence(DateTimeOffset.Now, TimeZoneInfo.Local);

            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                switch (delay.TotalMilliseconds)
                {
                    case <= 0:
                        await BeginTimer(task, handleOutOfRange, cancellationToken);
                        break;
                    case > int.MaxValue: // cannot create timer with interval > int.MaxValue
                        handleOutOfRange(Id);
                        return;
                }

                timer = new System.Timers.Timer(delay.TotalMilliseconds);
                timer.Elapsed += async (_, _) =>
                {
                    timer.Dispose();
                    timer = null;

                    if (!cancellationToken.IsCancellationRequested)
                        await task.Invoke(Id);

                    if (!cancellationToken.IsCancellationRequested)
                        await BeginTimer(task, handleOutOfRange, cancellationToken);
                };
                timer.Start();
            }
        }

        public void StopTimer()
        {
            timer?.Stop();
        }

        public string GetMessage()
        {
            var random = new Random();

            var index = random.Next(Messages.Count);

            return Messages[index];
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

        public override bool Equals(object obj)
        {
            return obj switch
            {
                ScheduledMessage m => m.Id == Id &&
                                      m.ChannelId == ChannelId &&
                                      m.TimeSchedule == TimeSchedule &&
                                      m.Messages.Count == Messages.Count &&
                                      m.Messages.All(Messages.Contains),
                _ => false
            };
        }
    }
}