using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LackBot.API.Repositories;
using LackBot.Common.Models;
using LackBot.Common.Models.AutoReacts;
using LShort.Common.Database;
using LShort.Common.Logging;

namespace LackBot.API.Services.Implementation
{
    public class AutoReactService : IAutoReactService
    {
        private readonly IAutoReactRepository repository;
        private readonly IAppLogger logger;

        public AutoReactService(IAutoReactRepository repository, IAppLogger logger)
        {
            this.logger = logger.FromSource(GetType());
            this.repository = repository;
        }

        public async Task<IList<AutoReact>> GetMatchingReactions(MessageDetails message)
        {
            return await repository.FindMatchingReactions(message);
        }

        public async Task<IList<AutoReact>> GetAllReactions()
        {
            return await repository.GetAll();
        }

        public async Task<AutoReact> AddAutoReact(AutoReactBuilder reactBuilder)
        {
            var enhancedLogger = logger
                .WithProperty("phrase", reactBuilder.Phrase)
                .WithProperty("reactType", reactBuilder.Type);
            
            var react = reactBuilder.Build();

            var result = await repository.Insert(react);

            if (!result.IsSuccess())
            {
                enhancedLogger.Error("Failed to add reaction to database.", react);
                return null;
            }

            return react;
        }

        public async Task<ResultExtended<AutoReact>> UpdateReact(Guid id, string newReaction)
        {
            var enhancedLogger = logger
                .WithProperty("reactId", id)
                .WithProperty("newReact", newReaction);
            
            var autoReact = await repository.Get(id);

            if (autoReact is null)
            {
                enhancedLogger.Error("Failed to find AutoReact.");
                return ResultExtended<AutoReact>.Failure("Failed to find AutoReact.");
            }

            autoReact.Emoji = newReaction;
            
            var result = await repository.Update(autoReact);

            if (!result.IsSuccess())
            {
                enhancedLogger.Error("Failed to update reaction in existing AutoReact.", autoReact);
                return ResultExtended<AutoReact>.NoChange("Failed to update reaction in existing AutoReact.");
            }

            return ResultExtended<AutoReact>.Success(autoReact);
        }

        public Task<RepositoryResult> RemoveAutoReact(Guid id)
        {
            return repository.Delete(id);
        }
    }
}