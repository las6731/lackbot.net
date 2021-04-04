using System;
using System.Linq;
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
                .FirstOrDefault(x => x.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) != -1);
            
            return emote is null ? ResultExtended<IEmote>.Failure("Failed to find emote.") : ResultExtended<IEmote>.Success(emote);
        }
    }
}