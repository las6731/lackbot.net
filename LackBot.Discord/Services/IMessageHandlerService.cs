using System.Threading.Tasks;
using Discord.WebSocket;

namespace LackBot.Discord.Services
{
    /// <summary>
    /// The service that handles all messages received by the bot.
    /// </summary>
    public interface IMessageHandlerService
    {
        /// <summary>
        /// Initialize the service asynchronously.
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// Handles a message received by the bot.
        /// </summary>
        /// <param name="msg">The message.</param>
        Task HandleMessageAsync(SocketMessage msg);
    }
}