using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LackBot.API.Repositories;
using LackBot.Common.Models;
using LackBot.Common.Models.ScheduledMessage;
using LShort.Common.Database;
using LShort.Common.Logging;

namespace LackBot.API.Services.Implementation
{
    public class ScheduledMessageService : IScheduledMessageService
    {
        private readonly IScheduledMessageRepository repository;
        private readonly IAppLogger logger;

        public ScheduledMessageService(IScheduledMessageRepository repository, IAppLogger logger)
        {
            this.repository = repository;
            this.logger = logger.FromSource(GetType());
        }

        public async Task<IList<ScheduledMessage>> GetAllMessages()
        {
            return await repository.GetAll();
        }

        public async Task<ScheduledMessage> AddScheduledMessage(ScheduledMessage message)
        {
            var enhancedLogger = logger
                .WithProperty("channelId", message.ChannelId)
                .WithProperty("timeSchedule", message.TimeSchedule);
            
            var result = await repository.Insert(message);

            if (!result.IsSuccess())
            {
                enhancedLogger.Error("Failed to add ScheduledMessage to database.", message);
                return null;
            }

            return message;
        }

        public async Task<ResultExtended<ScheduledMessage>> UpdateScheduledMessage(Guid id, ScheduledMessage message)
        {
            var enhancedLogger = logger.WithProperty("scheduledMessageId", id);

            message.Id = id; // ensure id is correct

            var existingMessage = await repository.Get(id);

            if (existingMessage is null)
            {
                enhancedLogger.Error("Existing message not found when attempting to update.");
                return ResultExtended<ScheduledMessage>.Failure("Existing message not found when attempting to update.");
            }

            var result = await repository.Update(message);

            if (!result.IsSuccess())
            {
                enhancedLogger.Error("Failed to update ScheduledMessage.", message);
                return ResultExtended<ScheduledMessage>.NoChange("Failed to update ScheduledMessage.");
            }
            
            return ResultExtended<ScheduledMessage>.Success(message);
        }

        public async Task<ResultExtended<ScheduledMessage>> AddMessage(Guid id, string message)
        {
            var enhancedLogger = logger.WithProperty("scheduledMessageId", id);

            var existingMessage = await repository.Get(id);

            if (existingMessage is null)
            {
                enhancedLogger.Error("Existing message not found when attempting to update.");
                return ResultExtended<ScheduledMessage>.Failure("Existing message not found when attempting to update.");
            }
            
            existingMessage.Messages.Add(message);
            
            var result = await repository.Update(existingMessage);

            if (!result.IsSuccess())
            {
                enhancedLogger.Error("Failed to update ScheduledMessage.", existingMessage);
                return ResultExtended<ScheduledMessage>.NoChange("Failed to update ScheduledMessage.");
            }
            
            return ResultExtended<ScheduledMessage>.Success(existingMessage);
        }

        public async Task<ResultExtended<ScheduledMessage>> UpdateMessage(Guid id, int messageIndex, string newMessage)
        {
            var enhancedLogger = logger.WithProperty("scheduledMessageId", id)
                .WithProperty("messageIndex", messageIndex);

            var existingMessage = await repository.Get(id);

            if (existingMessage is null)
            {
                enhancedLogger.Error("Existing message not found when attempting to update.");
                return ResultExtended<ScheduledMessage>.Failure("Existing message not found when attempting to update.");
            }

            if (existingMessage.Messages.Count < messageIndex)
            {
                enhancedLogger.Warning("Index out of range. Attempting to append new message.");
                existingMessage.Messages.Add(newMessage);
            }
            else
            {
                existingMessage.Messages[messageIndex] = newMessage;
            }

            var result = await repository.Update(existingMessage);

            if (!result.IsSuccess())
            {
                enhancedLogger.Error("Failed to update ScheduledMessage.", existingMessage);
                return ResultExtended<ScheduledMessage>.NoChange("Failed to update ScheduledMessage.");
            }
            
            return ResultExtended<ScheduledMessage>.Success(existingMessage);
        }

        public async Task<ResultExtended<ScheduledMessage>> RemoveMessage(Guid id, int messageIndex)
        {
            var enhancedLogger = logger.WithProperty("scheduledMessageId", id)
                .WithProperty("messageIndex", messageIndex);

            var existingMessage = await repository.Get(id);

            if (existingMessage is null)
            {
                enhancedLogger.Error("Existing message not found when attempting to update.");
                return ResultExtended<ScheduledMessage>.Failure("Existing message not found when attempting to update.");
            }

            if (existingMessage.Messages.Count < messageIndex)
            {
                enhancedLogger.Error("Index out of range when attempting to remove message.");
                return ResultExtended<ScheduledMessage>.NoChange("Index out of range when attempting to remove message.");
            }
            
            existingMessage.Messages.RemoveAt(messageIndex);

            if (existingMessage.Messages.Count == 0)
            {
                enhancedLogger.Information("Removed last message. Attempting to delete.");
                var deleteResult = await RemoveScheduledMessage(existingMessage.Id);

                if (!deleteResult.IsSuccess())
                {
                    enhancedLogger.Error("Failed to delete message.");
                    return ResultExtended<ScheduledMessage>.NoChange("Failed to delete message.");
                }
                
                return ResultExtended<ScheduledMessage>.Success(existingMessage);
            }

            var result = await repository.Update(existingMessage);

            if (!result.IsSuccess())
            {
                enhancedLogger.Error("Failed to remove message.", existingMessage);
                return ResultExtended<ScheduledMessage>.NoChange("Failed to remove message.");
            }
            
            return ResultExtended<ScheduledMessage>.Success(existingMessage);
        }

        public async Task<Result> RemoveScheduledMessage(Guid id)
        {
            var result = await repository.Delete(id);

            if (result.IsSuccess()) return Result.Success;

            return Result.Failure;
        }
    }
}