using System.Threading.Tasks;
using Discord.WebSocket;

namespace LackBot.Discord.Services
{
    /// <summary>
    /// A service that handles automatic responses.
    /// </summary>
    public interface IAutoResponseService
    {
        /// <summary>
        /// Handle the message asynchronously.
        /// </summary>
        /// <param name="message">The message.</param>
        Task HandleMessageAsync(SocketUserMessage message);
    }
}