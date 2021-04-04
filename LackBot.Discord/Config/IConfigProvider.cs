using System.Threading.Tasks;
using LackBot.Common.Models;

namespace LackBot.Discord.Config
{
    /// <summary>
    /// Provides a <see cref="ConfigData"/>.
    /// </summary>
    public interface IConfigProvider
    {
        /// <summary>
        /// Gets the config from the provided file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>The config.</returns>
        Task<ResultExtended<ConfigData>> Get(string path);
        
        /// <summary>
        /// Gets the config from cache, if present.
        /// </summary>
        /// <returns>The cached config, if present.</returns>
        Task<ResultExtended<ConfigData>> Get();

        /// <summary>
        /// Saves the config to the provided file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <param name="config">The config.</param>
        /// <returns>The result of the update.</returns>
        Task<Result> Update(string path, ConfigData config);
    }
}