using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using LackBot.Common.Models.ScheduledMessage;
using LackBot.Discord.Config;
using LackBot.Discord.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace LackBot.Discord.Services.Implementation
{
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

            Console.WriteLine("ScheduledMessageService updated messages successfully.");
        }

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
                try
                {
                    await m.BeginTimer(SendMessage, cancellationToken);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine($"Interval for '{m.TimeSchedule}' is too far in the future to schedule yet.");
                    continue;
                }
                messages.Add(m);
            }
        }

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

        private string ReplaceEmojis(string msg)
        {
            var matches = Regex.Matches(msg, ":(.+?):");

            foreach (Match match in matches)
            {
                var emoteName = match.Captures[0].Value.Trim(':');
                var emote = client.GetEmote(emoteName);

                if (!emote.IsSuccess) continue;

                msg = msg.Replace(match.Value, emote.Value.ToString());
            }

            return msg;
        }

        private async Task SendMessage(Guid id)
        {
            var msg = messages.FirstOrDefault(m => m.Id == id);

            if (msg is null) return;

            var message = msg.GetMessage();

            message = ReplaceEmojis(message);

            var channel = client.GetChannel(msg.ChannelId) as IMessageChannel;

            await channel.SendMessageAsync(message);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Stop();

            foreach (var message in messages)
            {
                message.StopTimer();
            }

            await Task.CompletedTask;
        }

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