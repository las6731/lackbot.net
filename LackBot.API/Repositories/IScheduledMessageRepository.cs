using LackBot.Common.Models.ScheduledMessage;
using LShort.Common.Database;

namespace LackBot.API.Repositories
{
    public interface IScheduledMessageRepository : IRepository<ScheduledMessage>
    {
        // no extra methods needed
    }
}