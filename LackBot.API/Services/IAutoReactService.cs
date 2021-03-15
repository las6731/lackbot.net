using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LackBot.Common.Models;
using LackBot.Common.Models.AutoReacts;
using LackBot.Common.Models.AutoResponses;
using LShort.Common.Database;

namespace LackBot.API.Services
{
    public interface IAutoReactService
    {
        public Task<IList<AutoReact>> GetMatchingReactions(MessageDetails message);

        public Task<IList<AutoReact>> GetAllReactions();

        public Task<AutoReact> AddAutoReact(AutoReactBuilder responseBuilder);
        
        public Task<ResultExtended<AutoReact>> UpdateReact(Guid id, string newReaction);

        public Task<RepositoryResult> RemoveAutoReact(Guid id);
    }
}