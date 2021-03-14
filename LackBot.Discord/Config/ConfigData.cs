using System;

namespace LackBot.Discord.Config
{
    [Serializable]
    public class ConfigData
    {
        public Version Version { get; set; }
        public string Token { get; set; }
        
        public string ApiUrl { get; set; }

        public ConfigData()
        {
            Version = Program.AppVersion;
            Token = "PASTE TOKEN HERE";
            ApiUrl = "PASTE API URL HERE";
        }
    }
}