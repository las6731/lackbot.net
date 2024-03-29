﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace LackBot.Discord.Services.Implementation
{
    public class MessageHandlerService : IMessageHandlerService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly DiscordSocketClient client;
        private readonly CommandService commandService;
        private readonly IAutoResponseService autoResponseService;
        private readonly IAutoReactService autoReactService;

        public MessageHandlerService(IServiceProvider serviceProvider, CommandService commandService, IAutoResponseService autoResponseService, IAutoReactService autoReactService, DiscordSocketClient client)
        {
            this.serviceProvider = serviceProvider;
            this.client = client;
            this.commandService = commandService;
            this.autoResponseService = autoResponseService;
            this.autoReactService = autoReactService;
        }

        public async Task InitializeAsync()
        {
            await commandService.AddModulesAsync(Assembly.GetEntryAssembly(), serviceProvider);
        }

        public async Task HandleMessageAsync(SocketMessage msg)
        {
            if (msg.Author.IsBot) return;

            var message = msg as SocketUserMessage;
            if (message is null) return;
            
            var argPos = 0;
            if (message.HasCharPrefix('!', ref argPos))
            {
                var context = new SocketCommandContext(client, message);
                var result =
                    await commandService.ExecuteAsync(context, argPos, serviceProvider, MultiMatchHandling.Best);

                if (result.IsSuccess) return;

                if (result.Error == CommandError.UnknownCommand) return; // ignore unknown commands, they're probably meant for another bot
                
                Console.WriteLine($"{result.Error} when attempting to parse command: {result.ErrorReason}");
                await context.Channel.SendMessageAsync(
                    $":sweat_smile: Failed to execute command: {result.ErrorReason}");
            }
            
            // message was not a command; parse for AutoResponses and AutoReacts.
            await autoResponseService.HandleMessageAsync(message);
            await autoReactService.HandleMessageAsync(message);
        }
    }
}