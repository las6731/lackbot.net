using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LackBot.Common.Models;
using LackBot.Discord.Config;
using LackBot.Discord.Config.Implementation;
using LackBot.Discord.Services;
using LackBot.Discord.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LackBot.Discord
{
    class Program
    {
        public static Version AppVersion => typeof(Program).Assembly.GetName().Version;
        
        static void Main(string[] args)
        {
            Console.WriteLine($"Initializing LackBot {AppVersion}");

            var configFile = Environment.GetEnvironmentVariable("BOT_CONFIG");
            configFile ??= "config.json";

            MainAsync(configFile).Wait();
        }

        private static async Task MainAsync(string configFile)
        {
            // initialize dependency injection
            await using var services = BuildServiceProvider();

            var configProvider = services.GetRequiredService<IConfigProvider>();
            var configResult = await configProvider.Get(configFile);

            if (!configResult.IsSuccess)
            {
                Console.WriteLine("Config file not found. Attempting to create default config...");
                var result = await configProvider.Update(configFile, new ConfigData());

                Console.WriteLine(!result.IsSuccess()
                    ? "Failed to create a new configuration file. Please ensure you have write permissions for the current directory."
                    : "A config file has been created. Please add your bot token to the config file and relaunch the application.");
                return;
            }

            var config = configResult.Value;

            var client = services.GetRequiredService<DiscordSocketClient>();
            var messageHandlerService = services.GetRequiredService<IMessageHandlerService>();
            
            // initialize command handler
            await messageHandlerService.InitializeAsync();

            client.Log += Log;
            services.GetRequiredService<CommandService>().Log += Log;

            client.MessageReceived += messageHandlerService.HandleMessageAsync;
            
            // connect to the server
            await client.LoginAsync(TokenType.Bot, config.Token);
            await client.StartAsync();
            
            await client.SetGameAsync("Slackbot but worse");

            var scheduledMessageService = services.GetServices<IHostedService>().OfType<ScheduledMessageService>().First();

            await scheduledMessageService.StartAsync(new CancellationToken());
            
            // block until the program exits
            await Task.Delay(-1);
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        /// <summary>
        /// Configures dependency injection.
        /// </summary>
        /// <returns>The ServiceProvider for dependency injection.</returns>
        private static ServiceProvider BuildServiceProvider() => new ServiceCollection()
            .AddSingleton<IConfigProvider, ConfigProvider>()
            .AddSingleton(new HttpClient())
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton<CommandService>()
            .AddSingleton<IAutoResponseService, AutoResponseService>()
            .AddSingleton<IAutoReactService, AutoReactService>()
            .AddSingleton<IMessageHandlerService, MessageHandlerService>()
            .AddHostedService<ScheduledMessageService>()
            .BuildServiceProvider();
    }
}