using System.Threading.Tasks;
using LackBot.Common.Models;

namespace LackBot.Discord.Config
{
    public interface IConfigProvider
    {
        Task<ResultExtended<ConfigData>> Get(string path);
        
        Task<ResultExtended<ConfigData>> Get();

        Task<Result> Update(string path, ConfigData config);
    }
}