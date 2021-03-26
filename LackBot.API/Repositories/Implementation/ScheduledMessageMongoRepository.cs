using LackBot.Common.Models.ScheduledMessage;
using LShort.Common.Database.Attributes;
using LShort.Common.Database.Implementation;
using LShort.Common.Logging;
using MongoDB.Driver;

namespace LackBot.API.Repositories.Implementation
{
    [Collection("ScheduledMessages")]
    public class ScheduledMessageMongoRepository : MongoRepository<ScheduledMessage>, IScheduledMessageRepository
    {
        public ScheduledMessageMongoRepository(IMongoDatabase db, IAppLogger logger) : base(db, logger)
        {
            this.logger = logger.FromSource(GetType());
        }
    }
}