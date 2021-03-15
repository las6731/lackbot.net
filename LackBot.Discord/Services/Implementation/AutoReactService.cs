using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Discord;
using Discord.WebSocket;
using LackBot.Common.Models.AutoReacts;
using LackBot.Discord.Config;
using LackBot.Discord.Extensions;
using Newtonsoft.Json;

namespace LackBot.Discord.Services.Implementation
{
    public class AutoReactService : IAutoReactService
    {
        private readonly HttpClient httpClient;
        private readonly DiscordSocketClient client;
        private readonly IConfigProvider configProvider;

        public AutoReactService(HttpClient httpClient, DiscordSocketClient client, IConfigProvider configProvider)
        {
            this.httpClient = httpClient;
            this.client = client;
            this.configProvider = configProvider;
        }
        
        public async Task HandleMessageAsync(SocketUserMessage message)
        {
            var reacts = await GetMatchingReacts(message);

            if (reacts.Count == 0) return;

            var emojis = reacts.Select(react => react.Emoji);

            foreach (var emoji in emojis)
            {
                var emote = client.GetEmote(emoji);

                if (!emote.IsSuccess)
                {
                    var basicEmoji = new Emoji(emoji);
                    await message.AddReactionAsync(basicEmoji);
                    continue;
                }

                await message.AddReactionAsync(emote.Value);
            }
        }
        
        private async Task<IList<AutoReact>> GetMatchingReacts(SocketUserMessage message)
        {
            var configResult = await configProvider.Get();
            if (!configResult.IsSuccess) return null;
            var config = configResult.Value;

            var queryBuilder = HttpUtility.ParseQueryString(string.Empty);

            queryBuilder["authorId"] = message.Author.Id.ToString();
            queryBuilder["channelId"] = message.Channel.Id.ToString();
            queryBuilder["timestamp"] = message.Timestamp.ToString();

            var result = await httpClient.GetAsync($"{config.ApiUrl}/auto-react/{message.Content}?{queryBuilder}");

            if (!result.IsSuccessStatusCode) return null;

            var reacts = JsonConvert.DeserializeObject<IList<AutoReact>>(await result.Content.ReadAsStringAsync());

            return reacts;
        }
    }
}