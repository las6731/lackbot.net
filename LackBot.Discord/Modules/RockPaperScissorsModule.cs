using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace LackBot.Discord.Modules
{
    [Group("rps")]
    public class RockPaperScissorsModule : ModuleBase<SocketCommandContext>
    {
        private readonly IList<string> choices = new List<string> { "rock", "paper", "scissors" };
        
        private readonly IList<Emoji> choiceEmojis = new List<Emoji> { new("\uD83E\uDEA8"), new("🧻"), new("✂") };
        
        private static class Result
        {
            public const string Win = "Wow, you're bad at this! :joy:";
            public const string Draw = "It's a tie :rolling_eyes:";
            public const string Loss = "You cheated, didn't you! :rage:";
        }
        
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

        private string DetermineResult(int playerChoice, int botChoice)
        {
            if (playerChoice == botChoice) return Result.Draw;
            switch (playerChoice)
            {
                case 0:
                    return botChoice == 1 ? Result.Win : Result.Loss;
                case 1:
                    return botChoice == 2 ? Result.Win : Result.Loss;
                case 2:
                    return botChoice == 0 ? Result.Win : Result.Loss;
            }

            return "Something broke :pensive:";
        }
    }
}