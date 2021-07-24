using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace LackBot.Discord.Modules
{
    /// <summary>
    /// A command module that allows users to roll dice.
    /// </summary>
    [Group("roll")]
    [Alias("dice")]
    public class RollDiceModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// The regex to match dice notation.
        /// </summary>
        private readonly Regex regex = new("^([0-9]*)d([0-9]+)$");
        
        /// <summary>
        /// The command to roll dice.
        /// </summary>
        /// <param name="notation">Dice notation describing the dice roll that the user wants.</param>
        [Command]
        public async Task RollDice([Summary("Dice notation, ex: 1d12, 2d6, etc")] string notation)
        {
            notation = notation.Trim().ToLower();

            var parsedNotation = await ParseNotation(notation);
            if (parsedNotation is null) return;

            var (dice, sides) = parsedNotation;
            var results = ExecuteRoll(dice, sides);

            await ReplyAsync(embed: BuildEmbed(dice, sides, results));
        }

        /// <summary>
        /// Parses the dice notation into the number of dice and number of sides.
        /// </summary>
        /// <param name="notation">The dice notation.</param>
        /// <returns>A tuple containing the number of dice, and the number of sides on each die.</returns>
        private async Task<Tuple<int, int>> ParseNotation(string notation)
        {
            if (!regex.IsMatch(notation))
            {
                await ReplyAsync(":pensive: You must use proper dice notation! ex: 1d12, 2d6, etc");
                return null;
            }

            var match = regex.Match(notation);
            
            if (match.Groups.Count != 3)
            {
                await ReplyAsync(":flushed: Somehow you broke the regex... impressive.");
                return null;
            }

            if (!int.TryParse(match.Groups[1].Value, out var dice))
                dice = 1;
            
            var sides = int.Parse(match.Groups[2].Value);

            if (dice <= 0 || sides < 0)
            {
                await ReplyAsync(
                    "Wait... did you really think you could use a non-positive integer? I'm disappointed in you.");
                return null;
            }

            return Tuple.Create(dice, sides);
        }

        /// <summary>
        /// Executes the dice roll.
        /// </summary>
        /// <param name="dice">The number of dice to roll.</param>
        /// <param name="sides">The number of sides that each die has.</param>
        /// <returns>A list containing the result of each die in the roll.</returns>
        private IList<int> ExecuteRoll(int dice, int sides)
        {
            var random = new Random();
            var results = new List<int>();

            for (var i = 0; i < dice; i++)
            {
                results.Add(random.Next(1, sides+1));
            }

            return results;
        }

        /// <summary>
        /// Builds the embed that will be sent to detail the results of the roll.
        /// </summary>
        /// <param name="dice">The number of dice rolled.</param>
        /// <param name="sides">The number of sides on each die.</param>
        /// <param name="results">The list of results for each die in the roll.</param>
        /// <returns></returns>
        private Embed BuildEmbed(int dice, int sides, IList<int> results)
        {
            var sum = results.Sum();

            double max = dice * sides;
            var color = (sum / max) switch
            {
                1 => Color.Gold,
                > 0.75 => Color.Green,
                > 0.5 => Color.DarkGreen,
                > 0.25 => Color.Red,
                _ => Color.DarkRed
            };
            
            var embed = new EmbedBuilder()
                .WithColor(color)
                .WithTitle("Dice Roll")
                .WithDescription($"{Context.User.Mention} rolled: **{sum}**")
                .WithFooter($"{dice}d{sides}")
                .WithCurrentTimestamp();

            for (var i = 0; i < results.Count; i++)
            {
                embed.AddField($"Die {i + 1}", results[i], true);
            }

            return embed.Build();
        }
    }
}