using System.Collections.Generic;
using System.Threading.Tasks;
using LackBot.Common.Models;
using LackBot.Common.Models.AutoReacts;
using LShort.Common.Database;

namespace LackBot.API.Repositories
{
    public interface IAutoReactRepository : IRepository<AutoReact>
    {
        public Task<IList<AutoReact>> FindMatchingReactions(MessageDetails message);
    }
}