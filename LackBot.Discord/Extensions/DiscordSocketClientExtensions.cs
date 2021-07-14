using System;
using System.Linq;
using System.Text.RegularExpressions;
using Discord;
using Discord.WebSocket;
using LackBot.Common.Models;

namespace LackBot.Discord.Extensions
{
    public static class DiscordSocketClientExtensions
    {
        /// <summary>
        /// Attempts to find an emote from all guilds that the bot is a member of.
        /// </summary>
        /// <param name="client">The discord client.</param>
        /// <param name="name">The name of the emote to find.</param>
        /// <returns>The result of getting the emote, containing the emote if successful.</returns>
        public static ResultExtended<IEmote> GetEmote(this DiscordSocketClient client, string name)
        {
            var emote = client.Guilds
                .SelectMany(x => x.Emotes)
                .FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            
            return emote is null ? ResultExtended<IEmote>.Failure("Failed to find emote.") : ResultExtended<IEmote>.Success(emote);
        }
        
        /// <summary>
        /// Replace all custom emotes (such as :pog:) with the actual emote, if found by the bot.
        /// </summary>
        /// <param name="client">The discord client.</param>
        /// <param name="msg">The message.</param>
        /// <returns>The message with all found emotes replaced.</returns>
        public static string ReplaceEmojis(this DiscordSocketClient client, string msg)
        {
            var matches = Regex.Matches(msg, ":(.+?):")
                .GroupBy(match => new { match.Value })
                .Select(o => o.FirstOrDefault());

            foreach (Match match in matches)
            {
                var emoteName = match.Captures[0].Value.Trim(':');
                var emote = client.GetEmote(emoteName);

                if (!emote.IsSuccess) continue;

                msg = msg.Replace(match.Value, emote.Value.ToString());
            }

            return msg;
        }
    }
}
