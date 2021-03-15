using System.Threading.Tasks;
using Discord.WebSocket;

namespace LackBot.Discord.Services
{
    public interface IAutoResponseService
    {
        Task HandleMessageAsync(SocketUserMessage message);
    }
}