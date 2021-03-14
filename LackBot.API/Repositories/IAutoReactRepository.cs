using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.WebSocket;
using LackBot.Common.Models.AutoReacts;
using LShort.Common.Database;

namespace LackBot.API.Repositories
{
    public interface IAutoReactRepository : IRepository<AutoReact>
    {
        public Task<IList<AutoReact>> FindMatchingReactions(SocketMessage message);
    }
}