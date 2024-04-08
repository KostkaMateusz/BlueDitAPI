using System.Text.RegularExpressions;
using AutoMapper;
using Bluedit.Application.DataModels.ReplayDtos;
using Bluedit.Domain.Entities.ReplyEntities;
using Bluedit.Persistence.Repositories.PostRepo;
using Bluedit.Persistence.Repositories.ReplyRepo;
using Bluedit.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace Bluedit.Controllers.ReplyRelated;


[ApiController]
[Route("api/topics/{topicName}/posts/{postId:guid}/replies/{**repliesPath}")]
[Authorize]
public partial class RepliesController : ControllerBase
{
    private readonly IRepliesRepository _repliesRepository;
    private readonly IUserContextService _userContextService;
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;
    private readonly Regex _guidRegex;

    [GeneratedRegex(@"(?<=\S*)(?:\{{0,1}(?:[0-9a-fA-F]){8}-(?:[0-9a-fA-F]){4}-(?:[0-9a-fA-F]){4}-(?:[0-9a-fA-F]){4}-(?:[0-9a-fA-F]){12}\}{0,1})(?=\S*)")]
    private static partial Regex GuidRegex();

    public RepliesController(IRepliesRepository repliesRepository, IUserContextService userContextService, IPostRepository postRepository, IMapper mapper)
    {
        _repliesRepository = repliesRepository ?? throw new ArgumentNullException(nameof(repliesRepository));
        _userContextService = userContextService ?? throw new ArgumentNullException(nameof(userContextService));
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _guidRegex = GuidRegex();
    }
    private string LastRegexMatch(string subRepliesPath, Regex regex)
    {
        MatchCollection guidMatch = regex.Matches(subRepliesPath);

        var lastMatchValue = guidMatch.Last().Value;

        return lastMatchValue;
    }

    [AllowAnonymous]
    [HttpGet(Name = "GetRepliesCollection")]
    public async Task<ActionResult<IEnumerable<ReplyDto>>> GetRepliesCollection([FromRoute] Guid postId,[FromRoute] string? repliesPath)
    {
        var postExist = await _postRepository.PostWithGivenIdExistAsync(postId);
        if (postExist is false)
            return NotFound();

        if (string.IsNullOrEmpty(repliesPath))
        {
            var replies  = await _repliesRepository.GetRepliesByParentPostId(postId); 
            var repliesDto = _mapper.Map<IEnumerable<ReplyDto>>(replies);
            
            return Ok(repliesDto);
        }
        else
        {
            var parentGuidString = LastRegexMatch(repliesPath, _guidRegex);

            if (Guid.TryParse(parentGuidString, out var lastGuid) is false)
                return BadRequest();
            
            var replies = await _repliesRepository.GetSubRepliesByParentReplayId(lastGuid);
            var repliesDto = _mapper.Map<IEnumerable<ReplyDto>>(replies);
           
            return Ok(repliesDto);
        }

    }

    [HttpPost(Name = "GetReplies")]
    public async Task<ActionResult<ReplyDto>> Get2Replies([FromRoute] string topicName,[FromRoute] Guid postId,[FromRoute] string? repliesPath, [FromBody] CreateReplyDto createReplayDto)
    {
        var userId = _userContextService.GetUserId;
        
        if (await _postRepository.PostWithGivenIdExistAsync(postId) is not true)
            return NotFound();

        if (string.IsNullOrEmpty(repliesPath))
        {
            var newReply = new Reply { Description = createReplayDto.Description, UserId = userId, ParentPostId = postId };
        
            await _repliesRepository.AddReply(newReply);
            await _repliesRepository.SaveChangesAsync();
        
            return CreatedAtRoute("GetReplies", routeValues: new { PostId = postId, topicName }, createReplayDto);
        }
        else
        {
            var parentGuidString = LastRegexMatch(repliesPath, _guidRegex);

            if (Guid.TryParse(parentGuidString, out var subReplayGuid) is false)
                return BadRequest();

            var subReplay = await _repliesRepository.GetReplyById(subReplayGuid);

            if (subReplay is null)
                return NotFound();

            var newSubreply = _mapper.Map<SubReplay>(createReplayDto);

            newSubreply.UserId = userId;
            newSubreply.ParentReplyId = subReplayGuid;

            await _repliesRepository.AddReply(newSubreply);
            await _repliesRepository.SaveChangesAsync();

            var replyDto = _mapper.Map<ReplyDto>(newSubreply);

            return CreatedAtRoute("GetReplies", new { topicName, PostId = postId, repliesPath }, replyDto);
        }
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteReplyTree([FromRoute] string repliesPath)
    {
        var parentGuidString = LastRegexMatch(repliesPath, _guidRegex);

        if (Guid.TryParse(parentGuidString, out var replyId) is false)
            return BadRequest();
        
        var replay = await _repliesRepository.GetReplyById(replyId);
        if (replay is null)
            return NotFound();

        //Check root reply ownership
        var userId = _userContextService.GetUserId;
        if (replay.UserId != userId)
            return Unauthorized($"User: {userId} is not authorized to delete replay:{replyId}");

        await _repliesRepository.DeleteReplyTree(replay);
        await _repliesRepository.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpPut]
    public async Task<ActionResult<ReplyDto>> UpdateReply([FromRoute] string repliesPath, [FromBody] UpdateReplyDto updateReply)
    {
        var parentGuidString = LastRegexMatch(repliesPath, _guidRegex);

        if (Guid.TryParse(parentGuidString, out var replyId) is false)
            return BadRequest();
        
        var reply = await _repliesRepository.GetReplyById(replyId);
        if (reply is null)
            return NotFound();

        //Check reply ownership
        var userId = _userContextService.GetUserId;
        if (reply.UserId != userId)
            return Unauthorized($"User: {userId} is not authorized to modify replay:{replyId}");

        reply.Description = updateReply.Description;

        _repliesRepository.UpdateReply(reply);
        await _repliesRepository.SaveChangesAsync();

        var replyDto = _mapper.Map<ReplyDto>(reply);

        return Ok(replyDto);
    }
    
    [HttpPatch]
    public async Task<ActionResult<UpdateReplyDto>> PartiallyUpdateReply([FromRoute] string repliesPath, [FromBody] JsonPatchDocument<UpdateReplyDto> patchDocument)
    {
        var parentGuidString = LastRegexMatch(repliesPath, _guidRegex);

        if (Guid.TryParse(parentGuidString, out var replyId) is false)
            return BadRequest();
        
        var replayFromRepo = await _repliesRepository.GetReplyById(replyId);

        if (replayFromRepo is null)
            return NotFound();

        //Check replay ownership
        var userId = _userContextService.GetUserId;
        if (replayFromRepo.UserId != userId)
            return Unauthorized($"User: {userId} is not authorized to modify reply:{replyId}");

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
