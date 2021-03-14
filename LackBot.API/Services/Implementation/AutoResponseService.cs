using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LackBot.API.Repositories;
using LackBot.Common.Models;
using LackBot.Common.Models.AutoResponses;
using LShort.Common.Database;
using LShort.Common.Logging;

namespace LackBot.API.Services.Implementation
{
    public class AutoResponseService : IAutoResponseService
    {
        private readonly IAutoResponseRepository repository;
        private readonly IAppLogger logger;

        public AutoResponseService(IAutoResponseRepository repository, IAppLogger logger)
        {
            this.logger = logger.FromSource(GetType());
            this.repository = repository;
        }

        public async Task<AutoResponse> GetMatchingResponse(MessageDetails message)
        {
            return await repository.FindMatchingResponse(message);
        }

        public async Task<IList<AutoResponse>> GetAllResponses()
        {
            return await repository.GetAll();
        }

        public async Task<AutoResponse> AddAutoResponse(AutoResponseBuilder responseBuilder)
        {
            var enhancedLogger = logger
                .WithProperty("phrase", responseBuilder.Phrase)
                .WithProperty("responseType", responseBuilder.Type);
            
            var response = responseBuilder.Build();

            var result = await repository.Insert(response);

            if (!result.IsSuccess())
            {
                enhancedLogger.Error("Failed to add response to database.", response);
                return null;
            }

            return response;
        }

        public async Task<ResultExtended<AutoResponse>> AddResponse(Guid id, string response)
        {
            var enhancedLogger = logger.WithProperty("responseId", id).WithProperty("newResponse", response);

            var autoResponse = await repository.Get(id);

            if (autoResponse is null)
            {
                enhancedLogger.Error("Failed to find AutoResponse.");
                return ResultExtended<AutoResponse>.Failure("Failed to find AutoResponse.");
            }
            
            autoResponse.Responses.Add(response);

            var result = await repository.Update(autoResponse);

            if (!result.IsSuccess())
            {
                enhancedLogger.Error("Failed to add response option to existing AutoResponse.", autoResponse);
                return ResultExtended<AutoResponse>.NoChange("Failed to add response option to existing AutoResponse.");
            }

            return ResultExtended<AutoResponse>.Success(autoResponse);
        }

        public async Task<ResultExtended<AutoResponse>> UpdateResponse(Guid id, int responseIndex, string newResponse)
        {
            var enhancedLogger = logger
                .WithProperty("responseId", id)
                .WithProperty("responseIndex", responseIndex)
                .WithProperty("newResponse", newResponse);
            
            var autoResponse = await repository.Get(id);

            if (autoResponse is null)
            {
                enhancedLogger.Error("Failed to find AutoResponse.");
                return ResultExtended<AutoResponse>.Failure("Failed to find AutoResponse.");
            }

            if (autoResponse.Responses.Count <= responseIndex)
            {
                enhancedLogger.Warning("Attempting to update existing response, but response index is out of bounds.");
                autoResponse.Responses.Add(newResponse);
            }
            else
            {
                autoResponse.Responses[responseIndex] = newResponse;
            }
            
            var result = await repository.Update(autoResponse);

            if (!result.IsSuccess())
            {
                enhancedLogger.Error("Failed to update response option in existing AutoResponse.", autoResponse);
                return ResultExtended<AutoResponse>.NoChange("Failed to update response option in existing AutoResponse.");
            }

            return ResultExtended<AutoResponse>.Success(autoResponse);
        }

        public async Task<ResultExtended<AutoResponse>> RemoveResponse(Guid id, int responseIndex)
        {
            var enhancedLogger = logger
                .WithProperty("responseId", id)
                .WithProperty("responseIndex", responseIndex);
            
            var autoResponse = await repository.Get(id);

            if (autoResponse is null)
            {
                enhancedLogger.Error("Failed to find AutoResponse.");
                return ResultExtended<AutoResponse>.Failure("Failed to find AutoResponse.");
            }

            if (autoResponse.Responses.Count <= responseIndex)
            {
                enhancedLogger.Error("Attempting to remove response option, but response index is out of bounds.");
                return ResultExtended<AutoResponse>.Failure("Response index is out of bounds.");
            }
            
            autoResponse.Responses.RemoveAt(responseIndex);

            if (autoResponse.Responses.Count == 0)
            {
                enhancedLogger.Information("Removed only option in AutoResponse. The AutoResponse will be deleted.");

                var deleteResult = await RemoveAutoResponse(id);

                if (!deleteResult.IsSuccess())
                {
                    enhancedLogger.Error("Failed to delete AutoResponse.");
                    return ResultExtended<AutoResponse>.NoChange("Failed to delete AutoResponse.");
                }

                return ResultExtended<AutoResponse>.Success(autoResponse);
            }

            var result = await repository.Update(autoResponse);
            
            if (!result.IsSuccess())
            {
                enhancedLogger.Error("Failed to update response option in existing AutoResponse.", autoResponse);
                return ResultExtended<AutoResponse>.NoChange("Failed to update response option in existing AutoResponse.");
            }

            return ResultExtended<AutoResponse>.Success(autoResponse);
        }

        public Task<RepositoryResult> RemoveAutoResponse(Guid id)
        {
            return repository.Delete(id);
        }
    }
}