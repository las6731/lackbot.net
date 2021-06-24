using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using LackBot.Common.Extensions;
using LackBot.Common.Models.ScheduledMessage;
using LackBot.Discord.Config;
using LackBot.Discord.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace LackBot.Discord.Services.Implementation
{
    /// <summary>
    /// A service that handles scheduled messages.
    /// </summary>
    public class ScheduledMessageService : IHostedService, IDisposable
    {
        private readonly DiscordSocketClient client;
        private readonly HttpClient httpClient;
        private readonly IConfigProvider configProvider;
        private readonly IList<ScheduledMessage> messages;
        private System.Timers.Timer timer;

        public ScheduledMessageService(HttpClient httpClient, DiscordSocketClient client,
            IConfigProvider configProvider)
        {
            this.client = client;
            this.httpClient = httpClient;
            this.configProvider = configProvider;
            messages = new List<ScheduledMessage>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await LoadMessages(cancellationToken);

            timer = new System.Timers.Timer(1000 * 60 * 5); // 5 minutes
            timer.Elapsed += async (_, _) =>
            {
                timer.Dispose();
                timer = null;

                if (!cancellationToken.IsCancellationRequested)
                    await StartAsync(cancellationToken);
            };
            timer.Start();

            var time = DateTimeOffset.Now.Truncate(TimeSpan.TicksPerSecond).TimeOfDay;
            Console.WriteLine($"{time} ScheduledMessageService updated messages successfully.");
        }

        /// <summary>
        /// Loads scheduled messages from the API, replacing any messages that have been updated or removed.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task LoadMessages(CancellationToken cancellationToken)
        {
            var returnedMessages = await GetMessages();

            if (returnedMessages is null) return;

            if (!returnedMessages.Any()) return;

            var changedMessages = messages.Where(m => !returnedMessages.Contains(m)).ToList();
            foreach (var m in changedMessages)
            {
                messages.Remove(m);
                m.Dispose();
            }

            var newMessages = returnedMessages.Where(m => !messages.Contains(m));
            foreach (var m in newMessages)
            {
                messages.Add(m);
                await m.BeginTimer(SendMessage, HandleIntervalOutOfRange, cancellationToken);
            }
        }

        /// <summary>
        /// Query the API for all scheduled messages.
        /// </summary>
        /// <returns>The list of scheduled messages.</returns>
        private async Task<IList<ScheduledMessage>> GetMessages()
        {
            var configResult = await configProvider.Get();
            if (!configResult.IsSuccess) return null;
            var config = configResult.Value;
            
            var result = await httpClient.GetAsync($"{config.ApiUrl}/scheduled-messages");
        
            if (!result.IsSuccessStatusCode) return null;
            
            var returnedMessages =
                JsonConvert.DeserializeObject<IList<ScheduledMessage>>(await result.Content.ReadAsStringAsync());
        
            return returnedMessages;
        }

        private async Task SendMessage(Guid id)
        {
            var msg = messages.FirstOrDefault(m => m.Id == id);

            if (msg is null) return;

            var message = msg.GetMessage();

            message = client.ReplaceEmojis(message);

            if (client.GetChannel(msg.ChannelId) is not IMessageChannel channel) return;

            await channel.SendMessageAsync(message);
        }

        /// <summary>
        /// Handles when the next occurence of a scheduled message is too far away to be scheduled by a timer.
        /// </summary>
        /// <remarks>Is invoked by the <see cref="ScheduledMessage"/> when it determines the timer interval
        /// is out of range. The maximum value of a timer interval is int.MaxValue: 2147483647 milliseconds,
        /// or 24 days, 20 hours, 31 minutes, and 23.647 seconds.</remarks>
        /// <param name="id">The id of the scheduled message to be sent.</param>
        private void HandleIntervalOutOfRange(Guid id)
        {
            var msg = messages.FirstOrDefault(m => m.Id == id);

            if (msg is null) return;

            var time = DateTimeOffset.Now.Truncate(TimeSpan.TicksPerSecond).TimeOfDay;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{time} WARN: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Interval for '{msg.TimeSchedule}' is too far in the future to schedule yet.");
            messages.Remove(msg);
        }

        /// <summary>
        /// Stops all timers for all scheduled messages.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Stop();

            foreach (var message in messages)
            {
                message.StopTimer();
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Disposes of all timers for all scheduled messages.
        /// </summary>
        public void Dispose()
        {
            timer?.Dispose();

            foreach (var m in messages)
            {
                m.Dispose();
            }
        }
    }
}