using System;
using System.IO;
using System.Threading.Tasks;
using LackBot.Common.Models;
using Newtonsoft.Json;

namespace LackBot.Discord.Config.Implementation
{
    public class ConfigProvider : IConfigProvider
    {
        private ConfigData Config;
        
        public async Task<ResultExtended<ConfigData>> Get(string path)
        {
            if (!File.Exists(path))
                return ResultExtended<ConfigData>.Failure($"File \"{path}\" not found.");

            try
            {
                using var stream = new StreamReader(path);
                var jsonString = await stream.ReadToEndAsync();

                var result = JsonConvert.DeserializeObject<ConfigData>(jsonString);

                Config = result;

                return ResultExtended<ConfigData>.Success(result);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception occured when trying to deserialize {path} from JSON.\n{e.Message}");
                
                return ResultExtended<ConfigData>.Failure(e.Message);
            }
        }

        public async Task<ResultExtended<ConfigData>> Get() => Config != null
            ? ResultExtended<ConfigData>.Success(Config)
            : ResultExtended<ConfigData>.Failure("Config file not loaded."); 

        public async Task<Result> Update(string path, ConfigData config)
        {
            try
            {
                var json = JsonConvert.SerializeObject(config, Formatting.Indented);

                await using var stream = new StreamWriter(path);
                await stream.WriteAsync(json);
                await stream.FlushAsync();

                return Result.Success;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception occured while trying to serialize {path} to JSON!\n{e.Message}");
                return Result.Failure;
            }
        }
    }
}