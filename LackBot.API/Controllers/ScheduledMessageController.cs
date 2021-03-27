using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LackBot.API.Services;
using LackBot.Common.Models;
using LackBot.Common.Models.ScheduledMessage;
using Microsoft.AspNetCore.Mvc;

namespace LackBot.API.Controllers
{
    [ApiController]
    [Route("api/v1/scheduled-messages")]
    public class ScheduledMessageController: Controller
    {
        private readonly IScheduledMessageService service;

        public ScheduledMessageController(IScheduledMessageService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IList<ScheduledMessage>> GetAll()
        {
            return await service.GetAllMessages();
        }

        [HttpPost]
        public async Task<ActionResult<ScheduledMessage>> AddScheduledMessage(ScheduledMessage message)
        {
            var result = await service.AddScheduledMessage(message);

            if (result is null)
                return BadRequest("Unable to add message.");

            return result;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ScheduledMessage>> UpdateScheduledMessage(Guid id, ScheduledMessage message)
        {
            message.Id = id;

            var result = await service.UpdateScheduledMessage(id, message);
            
            if (!result.IsSuccess)
                return result.Result == Result.Failure ? NotFound(result.Error) : BadRequest(result.Error);

            return result.Value;
        }
        
        [HttpPost]
        [Route("{id}")]
        public async Task<ActionResult<ScheduledMessage>> AddMessage(Guid id, [FromBody] string message)
        {
            var result = await service.AddMessage(id, message);
            
            if (!result.IsSuccess)
                return result.Result == Result.Failure ? NotFound(result.Error) : BadRequest(result.Error);

            return result.Value;
        }
        
        [HttpPut]
        [Route("{id}/{messageIndex}")]
        public async Task<ActionResult<ScheduledMessage>> UpdateMessage(Guid id, int messageIndex, [FromBody] string newMessage)
        {
            var result = await service.UpdateMessage(id, messageIndex, newMessage);
            
            if (result.IsSuccess)
                return result.Result == Result.Failure ? NotFound(result.Error) : BadRequest(result.Error);

            return result.Value;
        }
        
        [HttpDelete]
        [Route("{id}/{messageIndex}")]
        public async Task<ActionResult<ScheduledMessage>> DeleteMessage(Guid id, int messageIndex)
        {
            var result = await service.RemoveMessage(id, messageIndex);
            
            if (!result.IsSuccess)
                return result.Result == Result.Failure ? NotFound(result.Error) : BadRequest(result.Error);

            return result.Value;
        }
        
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteScheduledMessage(Guid id)
        {
            var result = await service.RemoveScheduledMessage(id);

            return result.IsSuccess() ? Ok() : BadRequest();
        }
    }
}