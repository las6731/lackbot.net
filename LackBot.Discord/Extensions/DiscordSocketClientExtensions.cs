using System;
using System.Linq;
using Discord;
using Discord.WebSocket;
using LackBot.Common.Models;

namespace LackBot.Discord.Extensions
{
    public static class DiscordSocketClientExtensions
    {
        public static ResultExtended<IEmote> GetEmote(this DiscordSocketClient client, string name)
        {
            var emote = client.Guilds
                .SelectMany(x => x.Emotes)
                .FirstOrDefault(x => x.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) != -1);
            
            return emote is null ? ResultExtended<IEmote>.Failure("Failed to find emote.") : ResultExtended<IEmote>.Success(emote);
        }
    }
}