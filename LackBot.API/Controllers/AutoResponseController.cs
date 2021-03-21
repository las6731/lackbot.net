using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LackBot.API.Services;
using LackBot.Common.Models;
using LackBot.Common.Models.AutoResponses;
using LShort.Common.Database;
using Microsoft.AspNetCore.Mvc;

namespace LackBot.API.Controllers
{
    [ApiController]
    [Route("api/v1/auto-response")]
    public class AutoResponseController : Controller
    {
        private readonly IAutoResponseService service;
        
        // TODO: add any appropriate validations?

        public AutoResponseController(IAutoResponseService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IList<AutoResponse>> GetAll()
        {
            return await service.GetAllResponses();
        }

        [HttpGet]
        [Route("{content}")]
        public async Task<AutoResponse> Get(string content, ulong authorId = default, DateTimeOffset timestamp = default, ulong channelId = default)
        {
            var message = new MessageDetails
            {
                Content = content,
                AuthorId = authorId,
                Timestamp = timestamp,
                ChannelId = channelId
            };
            
            return await service.GetMatchingResponse(message);
        }

        [HttpPost]
        public async Task<ActionResult<AutoResponse>> AddAutoResponse(AutoResponseBuilder responseBuilder)
        {
            var result = await service.AddAutoResponse(responseBuilder);

            if (result is null)
                return BadRequest("Unable to add response.");

            return result;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<AutoResponse>> AddResponse(Guid id, [FromBody] string response)
        {
            var result = await service.AddResponse(id, response);

            if (!result.IsSuccess)
                return result.Result == Result.Failure ? NotFound(result.Error) : BadRequest(result.Error);

            return result.Value;
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<ActionResult<AutoResponse>> ReplaceAutoResponse(Guid id, AutoResponseBuilder responseBuilder)
        {
            var result = await service.ReplaceAutoResponse(id, responseBuilder);

            if (!result.IsSuccess)
                return result.Result == Result.Failure ? NotFound(result.Error) : BadRequest(result.Error);

            return result.Value;
        }

        [HttpPut]
        [Route("{id}/{responseIndex}")]
        public async Task<ActionResult<AutoResponse>> UpdateResponse(Guid id, int responseIndex, [FromBody] string newResponse)
        {
            var result = await service.UpdateResponse(id, responseIndex, newResponse);

            if (!result.IsSuccess)
                return result.Result == Result.Failure ? NotFound(result.Error) : BadRequest(result.Error);

            return result.Value;
        }

        [HttpDelete]
        [Route("{id}/{responseIndex}")]
        public async Task<ActionResult<AutoResponse>> RemoveResponse(Guid id, int responseIndex)
        {
            var result = await service.RemoveResponse(id, responseIndex);

            if (!result.IsSuccess)
                return result.Result == Result.Failure ? NotFound(result.Error) : BadRequest(result.Error);

            return result.Value;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> RemoveAutoResponse(Guid id)
        {
            var result = await service.RemoveAutoResponse(id);

            if (!result.IsSuccess())
                return new BadRequestResult();

            return Ok();
        }
    }
}