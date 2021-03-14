using System;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace LackBot.Discord.Services
{
    public interface IMessageHandlerService
    {
        Task InitializeAsync();

        Task HandleMessageAsync(SocketMessage msg);
    }
}