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
[Route("api/topics/{topicName}/posts/{PostId}/reply")]
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
    [HttpGet("{replayID}", Name = "GetReplayDetails")]
    public async Task<ActionResult<ReplayDto>> GetReplay([FromRoute] Guid replayID)
    {
        var replay=await _repliesRepository.GetReplayById(replayID);

        if(replay is null)        
            return NotFound();        

        var replayDto = _mapper.Map<ReplayDto>(replay);

        return Ok(replayDto);
    }


    [HttpPost]
    public async Task<ActionResult<ReplayDto>> CreateReplytoReplay([FromBody] CreateSingleReplyDto createReplayDto, [FromRoute] Guid PostId, [FromRoute] string topicName)
    {
        ReplyBase? parentReply=null;
        Post? parentPost=null;

        parentReply = await _repliesRepository.GetReplayById(createReplayDto.ParentId);

        if (parentReply is null)
        {
            parentPost = await _postRepository.GetPostByIdAsync(createReplayDto.ParentId);
        }

        if (parentReply is null && parentPost is null)
        {
            return NotFound();
        }

        Guid? replyId = null;

        if (parentReply is not null)
        {
            var newReply = new SubReplay() { UserId = _userContextService.GetUserId, Description = createReplayDto.Description };
            newReply.ParentReplyId = parentReply.ReplyId;
            await _repliesRepository.Addreplay(newReply);
            replyId = newReply.ReplyId;
        }
        else if (parentPost is not null)
        {
            var newReply = new Reply() { UserId = _userContextService.GetUserId, Description = createReplayDto.Description };
            newReply.ParentPostId = parentPost.PostId;
            await _repliesRepository.Addreplay(newReply);
            replyId = newReply.ReplyId;
        }         

        await _repliesRepository.SaveChangesAsync();

        return CreatedAtRoute("GetReplayDetails", new { PostId=PostId, replayID=replyId, topicName= topicName }, createReplayDto);        
    }

    [HttpDelete("{replayID}")]
    public async Task<IActionResult> DeleteReplyTree([FromRoute] Guid replayID)
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

    [HttpPut("{replayID}")]
    public async Task<ActionResult<ReplayDto>> UpdateReply([FromRoute] Guid replayID, [FromBody] UpdateReplyDto updateReply)
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

    [HttpPatch("{replayID}")]
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