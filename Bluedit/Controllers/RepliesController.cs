using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bluedit.Services.Repositories;
using Bluedit.Entities;
using Bluedit.Models.DataModels.ReplayDtos;
using Bluedit.Services.Authentication;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace Bluedit.Controllers;

[Authorize]
[ApiController]
[Route("api/posts/{PostId}/reply/{replayID}")]
public class RepliesController : ControllerBase
{
    private readonly IRepliesRepository _repliesRepository;
    private readonly IUserContextService _userContextService;
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public RepliesController(IRepliesRepository repliesRepository, IUserContextService userContextService, IPostRepository postRepository, IMapper mapper)
    {
        _repliesRepository = repliesRepository ?? throw new ArgumentNullException(nameof(repliesRepository));
        _userContextService = userContextService ?? throw new ArgumentNullException(nameof(userContextService));
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [AllowAnonymous]
    [HttpGet(Name = "GetReplayDetails")]
    public async Task<ActionResult<ReplayDto>> getReplay([FromRoute] Guid replayID)
    {
        var replay=await _repliesRepository.GetReplayById(replayID);

        if(replay is null)        
            return NotFound();        

        var replayDto = _mapper.Map<ReplayDto>(replay);

        return Ok(replayDto);
    }


    [HttpPost]
    public async Task<ActionResult<ReplayDto>> createReplytoReplay([FromRoute] Guid replayID, [FromBody] CreateReplayDto createReplayDto, [FromRoute] Guid PostId)
    {
        var replay = await _repliesRepository.GetReplayById(replayID);

        if (replay is null)        
            return NotFound();        

        var newSubReplay = _mapper.Map<SubReplay>(createReplayDto);

        newSubReplay.UserId = _userContextService.GetUserId;
        newSubReplay.ParentReplyId = replayID;

        await _repliesRepository.Addreplay(newSubReplay);
        await _repliesRepository.SaveChangesAsync();

        var replayDto=_mapper.Map<ReplayDto>(newSubReplay);

        return CreatedAtRoute("GetReplayDetails", new { PostId, replayID }, replayDto);        
    }

    [HttpDelete]
    public async Task<IActionResult> deleteReplyTree([FromRoute] Guid replayID)
    {
        var replay = await _repliesRepository.GetReplayById(replayID);
        if (replay is null)        
            return NotFound();        

        //Check root replay ownerhip
        var userId = _userContextService.GetUserId;
        if(replay.UserId != userId)        
            return Unauthorized($"User: {userId} is not authorized to delete replay:{replayID}");
        
        await _repliesRepository.DeleteReplayTree(replay);
        await _repliesRepository.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut]
    public async Task<ActionResult<ReplayDto>> updateReply([FromRoute] Guid replayID, [FromBody] UpdateReplyDto updateReply)
    {
        var replay = await _repliesRepository.GetReplayById(replayID);
        if (replay is null)        
            return NotFound();        

        //Check replay ownerhip
        var userId = _userContextService.GetUserId;
        if (replay.UserId != userId)        
            return Unauthorized($"User: {userId} is not authorized to modify replay:{replayID}");
        
        replay.Description = updateReply.Description;
        
        _repliesRepository.UpdateReply(replay);
        await _repliesRepository.SaveChangesAsync();

        var replyDto=_mapper.Map<ReplayDto>(replay);

        return Ok(replyDto);
    }

    [HttpPatch]
    public async Task<ActionResult<UpdateReplyDto>> PartialyUpdateReply([FromRoute] Guid replayID, [FromBody] JsonPatchDocument<UpdateReplyDto> patchDocument)
    {
        var replayFromRepo = await _repliesRepository.GetReplayById(replayID);
        
        if (replayFromRepo is null)        
            return NotFound();
        
        //Check replay ownerhip
        var userId = _userContextService.GetUserId;
        if (replayFromRepo.UserId != userId)        
            return Unauthorized($"User: {userId} is not authorized to modify replay:{replayID}");

        var replyToPatch = _mapper.Map<UpdateReplyDto>(replayFromRepo);
        patchDocument.ApplyTo(replyToPatch, ModelState);

        if (!TryValidateModel(replyToPatch))
            return ValidationProblem(ModelState);

        _mapper.Map(replyToPatch, replayFromRepo);

        _repliesRepository.UpdateReply(replayFromRepo);
        await _repliesRepository.SaveChangesAsync();

        return Ok(replyToPatch);
    }
}