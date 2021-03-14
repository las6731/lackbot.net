using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LackBot.Common.Models;
using LackBot.Common.Models.AutoResponses;
using LShort.Common.Database;

namespace LackBot.API.Services
{
    public interface IAutoResponseService
    {
        public Task<AutoResponse> GetMatchingResponse(MessageDetails message);

        public Task<IList<AutoResponse>> GetAllResponses();

        public Task<AutoResponse> AddAutoResponse(AutoResponseBuilder responseBuilder);

        public Task<ResultExtended<AutoResponse>> AddResponse(Guid id, string response);
        
        public Task<ResultExtended<AutoResponse>> UpdateResponse(Guid id, int responseIndex, string newResponse);

        public Task<ResultExtended<AutoResponse>> RemoveResponse(Guid id, int responseIndex);

        public Task<RepositoryResult> RemoveAutoResponse(Guid id);
    }
}