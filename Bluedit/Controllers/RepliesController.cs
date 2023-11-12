using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bluedit.Domain.Entities;
using Bluedit.Models.DataModels.ReplayDtos;
using Bluedit.Services.Authentication;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Bluedit.Services.Repositories.PostRepo;
using Bluedit.Services.Repositories.ReplyRepo;
using Bluedit.Domain.Entities.ReplyEntities;

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
    [HttpGet("{replyId}", Name = "GetReplayDetails")]
    public async Task<ActionResult<ReplyDto>> GetReplay([FromRoute] Guid replyId)
    {
        var replay=await _repliesRepository.GetReplayById(replyId);

        if(replay is null)        
            return NotFound();        

        var replayDto = _mapper.Map<ReplyDto>(replay);

        return Ok(replayDto);
    }


    [HttpPost("{replyId}")]
    public async Task<ActionResult<ReplyDto>> CreateReplytoReplay([FromBody] CreateSingleReplyDto createReplayDto, [FromRoute] Guid PostId, [FromRoute] string topicName)
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

        return CreatedAtRoute("GetReplayDetails", new { PostId=PostId, replyId=replyId, topicName= topicName }, createReplayDto);        
    }

    [HttpDelete("{replyId}")]
    public async Task<IActionResult> DeleteReplyTree([FromRoute] Guid replyId)
    {
        var replay = await _repliesRepository.GetReplayById(replyId);
        if (replay is null)        
            return NotFound();        

        //Check root replay ownerhip
        var userId = _userContextService.GetUserId;
        if(replay.UserId != userId)        
            return Unauthorized($"User: {userId} is not authorized to delete replay:{replyId}");
        
        await _repliesRepository.DeleteReplayTree(replay);
        await _repliesRepository.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{replyId}")]
    public async Task<ActionResult<ReplyDto>> UpdateReply([FromRoute] Guid replyId, [FromBody] UpdateReplyDto updateReply)
    {
        var replay = await _repliesRepository.GetReplayById(replyId);
        if (replay is null)        
            return NotFound();        

        //Check replay ownerhip
        var userId = _userContextService.GetUserId;
        if (replay.UserId != userId)        
            return Unauthorized($"User: {userId} is not authorized to modify replay:{replyId}");
        
        replay.Description = updateReply.Description;
        
        _repliesRepository.UpdateReply(replay);
        await _repliesRepository.SaveChangesAsync();

        var replyDto=_mapper.Map<ReplyDto>(replay);

        return Ok(replyDto);
    }

    [HttpPatch("{replyId}")]
    public async Task<ActionResult<UpdateReplyDto>> PartialyUpdateReply([FromRoute] Guid replyId, [FromBody] JsonPatchDocument<UpdateReplyDto> patchDocument)
    {
        var replayFromRepo = await _repliesRepository.GetReplayById(replyId);
        
        if (replayFromRepo is null)        
            return NotFound();
        
        //Check replay ownerhip
        var userId = _userContextService.GetUserId;
        if (replayFromRepo.UserId != userId)        
            return Unauthorized($"User: {userId} is not authorized to modify replay:{replyId}");

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