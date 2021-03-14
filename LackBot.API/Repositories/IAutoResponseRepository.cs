using System.Threading.Tasks;
using Discord.WebSocket;
using LackBot.Common.Models.AutoResponses;
using LShort.Common.Database;

namespace LackBot.API.Repositories
{
    public interface IAutoResponseRepository : IRepository<AutoResponse>
    {
        public Task<AutoResponse> FindMatchingResponse(SocketMessage message);
    }
}