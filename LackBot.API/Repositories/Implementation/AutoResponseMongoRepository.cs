using System.Linq;
using System.Threading.Tasks;
using LackBot.Common.Models;
using LackBot.Common.Models.AutoResponses;
using LShort.Common.Database.Attributes;
using LShort.Common.Database.Implementation;
using LShort.Common.Logging;
using MongoDB.Driver;

namespace LackBot.API.Repositories.Implementation
{
    [Collection("AutoResponses")]
    public class AutoResponseMongoRepository : MongoRepository<AutoResponse>, IAutoResponseRepository
    {
        public AutoResponseMongoRepository(IMongoDatabase db, IAppLogger logger) : base(db, logger)
        {
            this.logger = logger.FromSource(GetType());
        }

        protected override void EnsureIndexes()
        {
            var indexKeys = Builders<AutoResponse>.IndexKeys.Ascending(response => response.Phrase);
            var indexOptions = new CreateIndexOptions()
            {
                Unique = true
            };
            
            Collection.Indexes.CreateOne(new CreateIndexModel<AutoResponse>(indexKeys, indexOptions));
        }

        public async Task<AutoResponse> FindMatchingResponse(MessageDetails message)
        {
            var responses = await GetAll();

            return responses.FirstOrDefault(response => response.Matches(message));
        }
    }
}