using System.Threading.Tasks;
using Discord.WebSocket;

namespace LackBot.Discord.Services
{
    public interface IAutoReactService
    {
        Task HandleMessageAsync(SocketUserMessage message);
    }
}