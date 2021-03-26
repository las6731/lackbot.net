using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LackBot.API.Services;
using LackBot.Common.Models;
using LackBot.Common.Models.AutoReacts;
using LShort.Common.Database;
using Microsoft.AspNetCore.Mvc;

namespace LackBot.API.Controllers
{
    [ApiController]
    [Route("api/v1/auto-reacts")]
    public class AutoReactController : Controller
    {
        private readonly IAutoReactService service;
        
        // TODO: add any appropriate validations?

        public AutoReactController(IAutoReactService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IList<AutoReact>> GetAll()
        {
            return await service.GetAllReactions();
        }

        [HttpGet]
        [Route("{content}")]
        public async Task<IList<AutoReact>> Get(string content, ulong authorId = default, DateTimeOffset timestamp = default, ulong channelId = default)
        {
            var message = new MessageDetails
            {
                Content = content,
                AuthorId = authorId,
                Timestamp = timestamp,
                ChannelId = channelId
            };
            
            return await service.GetMatchingReactions(message);
        }

        [HttpPost]
        public async Task<ActionResult<AutoReact>> AddAutoReact(AutoReactBuilder reactBuilder)
        {
            var result = await service.AddAutoReact(reactBuilder);

            if (result is null)
                return BadRequest("Unable to add react.");

            return result;
        }
        
        [HttpPost]
        [Route("{id}")]
        public async Task<ActionResult<AutoReact>> ReplaceAutoReact(Guid id, AutoReactBuilder reactBuilder)
        {
            var result = await service.ReplaceAutoReact(id, reactBuilder);

            if (!result.IsSuccess)
                return result.Result == Result.Failure ? NotFound(result.Error) : BadRequest(result.Error);

            return result.Value;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<AutoReact>> UpdateReact(Guid id, [FromBody] string newReaction)
        {
            var result = await service.UpdateReact(id, newReaction);

            if (!result.IsSuccess)
                return result.Result == Result.Failure ? NotFound(result.Error) : BadRequest(result.Error);

            return result.Value;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> RemoveAutoReact(Guid id)
        {
            var result = await service.RemoveAutoReact(id);

            return result.IsSuccess() ? Ok() : BadRequest();
        }
    }
}