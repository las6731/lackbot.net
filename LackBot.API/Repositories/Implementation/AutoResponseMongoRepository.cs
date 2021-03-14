using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
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

        public async Task<AutoResponse> FindMatchingResponse(SocketMessage message)
        {
            var responses = await GetAll();

            return responses.FirstOrDefault(response => response.Matches(message));
        }
    }
}