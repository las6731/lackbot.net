using System;

namespace LackBot.Discord.Config
{
    /// <summary>
    /// The Discord bot configuration.
    /// </summary>
    [Serializable]
    public class ConfigData
    {
        /// <summary>
        /// The bot version that this config was created for. Mostly useful for if the config schema changes down the road.
        /// </summary>
        public Version Version { get; set; }
        
        /// <summary>
        /// The discord bot token.
        /// </summary>
        public string Token { get; set; }
        
        /// <summary>
        /// The url to access the accompanying API.
        /// </summary>
        public string ApiUrl { get; set; }

        public ConfigData()
        {
            Version = Program.AppVersion;
            Token = "PASTE TOKEN HERE";
            ApiUrl = "PASTE API URL HERE";
        }
    }
}