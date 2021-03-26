using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LackBot.Common.Models;
using LackBot.Common.Models.ScheduledMessage;

namespace LackBot.API.Services
{
    public interface IScheduledMessageService
    {
        public Task<IList<ScheduledMessage>> GetAllMessages();

        public Task<ScheduledMessage> AddScheduledMessage(ScheduledMessage message);

        public Task<ResultExtended<ScheduledMessage>> UpdateScheduledMessage(Guid id, ScheduledMessage message);

        public Task<ResultExtended<ScheduledMessage>> AddMessage(Guid id, string message);

        public Task<ResultExtended<ScheduledMessage>> UpdateMessage(Guid id, int messageIndex, string newMessage);

        public Task<ResultExtended<ScheduledMessage>> RemoveMessage(Guid id, int messageIndex);

        public Task<Result> RemoveScheduledMessage(Guid id);
    }
}