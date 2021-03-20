using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Discord.WebSocket;
using LackBot.Common.Models;
using LackBot.Common.Models.AutoResponses;
using LackBot.Discord.Config;
using LackBot.Discord.Extensions;
using Newtonsoft.Json;

namespace LackBot.Discord.Services.Implementation
{
    public class AutoResponseService : IAutoResponseService
    {
        private readonly HttpClient httpClient;
        private readonly DiscordSocketClient client;
        private readonly IConfigProvider configProvider;

        public AutoResponseService(HttpClient httpClient, DiscordSocketClient client, IConfigProvider configProvider)
        {
            this.httpClient = httpClient;
            this.client = client;
            this.configProvider = configProvider;
        }
        
        public async Task HandleMessageAsync(SocketUserMessage message)
        {
            var msgDetails = new MessageDetails
            {
                AuthorId = message.Author.Id,
                ChannelId = message.Channel.Id,
                Content = message.Content.ToLower(),
                Timestamp = message.Timestamp
            };
            var response = await GetMatchingResponse(msgDetails);

            if (response is null) return;

            var msg = response.GetResponse(msgDetails);
            
            msg = ReplaceEmojis(msg);

            await message.Channel.SendMessageAsync(msg);
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

        private async Task<AutoResponse> GetMatchingResponse(MessageDetails message)
        {
            var configResult = await configProvider.Get();
            if (!configResult.IsSuccess) return null;
            var config = configResult.Value;

            var queryBuilder = HttpUtility.ParseQueryString(string.Empty);

            queryBuilder["authorId"] = message.AuthorId.ToString();
            queryBuilder["channelId"] = message.ChannelId.ToString();
            queryBuilder["timestamp"] = message.Timestamp.ToString();

            var result = await httpClient.GetAsync($"{config.ApiUrl}/auto-response/{message.Content}?{queryBuilder}");

            if (!result.IsSuccessStatusCode) return null;

            var settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto};
            var response = JsonConvert.DeserializeObject<AutoResponse>(await result.Content.ReadAsStringAsync(), settings);

            return response;
        }
    }
}