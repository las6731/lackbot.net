using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace LackBot.Discord.Modules
{
    /// <summary>
    /// A command module that allows users to place Rock, Paper, Scissors against the bot.
    /// </summary>
    [Group("rps")]
    public class RockPaperScissorsModule : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// The choices (in English) that can be chosen from.
        /// </summary>
        private readonly IList<string> choices = new List<string> { "rock", "paper", "scissors" };
        
        /// <summary>
        /// The choices (as Emojis) that can be chosen from.
        /// </summary>
        private readonly IList<Emoji> choiceEmojis = new List<Emoji> { new("\uD83E\uDEA8"), new("🧻"), new("✂") };
        
        private static class Result
        {
            /// <summary>
            /// The message to send when the bot wins.
            /// </summary>
            public const string Win = "Wow, you're bad at this! :joy:";
            
            /// <summary>
            /// The message to send when it is a draw.
            /// </summary>
            public const string Draw = "It's a tie :rolling_eyes:";
            
            /// <summary>
            /// The message to send when the bot loses (the player wins).
            /// </summary>
            public const string Loss = "You cheated, didn't you! :rage:";
        }
        
        /// <summary>
        /// The command to play a game of Rock, Paper, Scissors.
        /// </summary>
        /// <param name="choice">The player's choice.</param>
        [Command]
        public async Task RockPaperScissors([Summary("Rock, paper, or scissors!")] string choice)
        {
            choice = choice.ToLower();

            if (!choices.Contains(choice))
            {
                await ReplyAsync(":pensive: That's not how you play rock paper scissors!");
                return;
            }

            var botChoiceIndex = new Random().Next(3);
            var playerChoiceIndex = choices.IndexOf(choice);

            var outcome = DetermineResult(playerChoiceIndex, botChoiceIndex);

            await ReplyAsync($"{choiceEmojis[botChoiceIndex]} - {outcome}");
        }

        /// <summary>
        /// Determines the result of a game of Rock, Paper, Scissors.
        /// </summary>
        /// <param name="playerChoice">The index of the player's choice.</param>
        /// <param name="botChoice">The index of the bot's choice.</param>
        /// <returns>The message to send that describes the result of the game.</returns>
        private string DetermineResult(int playerChoice, int botChoice)
        {
            if (playerChoice == botChoice) return Result.Draw;
            return playerChoice switch
            {
                0 => botChoice == 1 ? Result.Win : Result.Loss,
                1 => botChoice == 2 ? Result.Win : Result.Loss,
                2 => botChoice == 0 ? Result.Win : Result.Loss,
                _ => "Something broke :pensive:"
            };
        }
    }
}