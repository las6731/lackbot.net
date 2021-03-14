using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LackBot.Common.Models;
using LackBot.Common.Models.AutoReacts;
using LShort.Common.Database.Attributes;
using LShort.Common.Database.Implementation;
using LShort.Common.Logging;
using MongoDB.Driver;

namespace LackBot.API.Repositories.Implementation
{
    [Collection("AutoReacts")]
    public class AutoReactMongoRepository : MongoRepository<AutoReact>, IAutoReactRepository
    {
        public AutoReactMongoRepository(IMongoDatabase db, IAppLogger logger) : base(db, logger)
        {
            this.logger = logger.FromSource(GetType());
        }

        public async Task<IList<AutoReact>> FindMatchingReactions(MessageDetails message)
        {
            var reacts = await GetAll();

            return reacts.Where(react => react.Matches(message)).ToList();
        }
    }
}