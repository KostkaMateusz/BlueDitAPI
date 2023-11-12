using AutoMapper;
using Bluedit.Domain.Entities.ReplyEntities;
using Bluedit.Models.DataModels.ReplayDtos;
using Bluedit.Services.Authentication;
using Bluedit.Services.Repositories.PostRepo;
using Bluedit.Services.Repositories.ReplyRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Bluedit.Controllers;


[ApiController]
[Route("api/topics/{topicName}/posts/{PostId}/replies")]
[Authorize]
public partial class RepliesCollection : ControllerBase
{
    private readonly IRepliesRepository _repliesRepository;
    private readonly IUserContextService _userContextService;
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;
    private readonly Regex _guidRegex;

    [GeneratedRegex("(?<=\\S*)(?:\\{{0,1}(?:[0-9a-fA-F]){8}-(?:[0-9a-fA-F]){4}-(?:[0-9a-fA-F]){4}-(?:[0-9a-fA-F]){4}-(?:[0-9a-fA-F]){12}\\}{0,1})(?=\\S*)")]
    private static partial Regex GuidRegex();

    public RepliesCollection(IRepliesRepository repliesRepository, IUserContextService userContextService, IPostRepository postRepository, IMapper mapper)
    {
        _repliesRepository = repliesRepository ?? throw new ArgumentNullException(nameof(repliesRepository));
        _userContextService = userContextService ?? throw new ArgumentNullException(nameof(userContextService));
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _guidRegex = GuidRegex();
    }
    private string? LastRegexMatch(string subRepliesPath, Regex regex)
    {
        MatchCollection guidMatch = regex.Matches(subRepliesPath);

        var lastMatchValue = guidMatch.Last().Value;

        return lastMatchValue;
    }

    [HttpGet(Name = "GetPostreplies")]
    [AllowAnonymous]
    public async Task<ActionResult<ReplyDto>> GetPostreplies(Guid PostId)
    {
        var postExist = await _postRepository.PostWithGivenIdExist(PostId);
        if (postExist is false)        
            return NotFound();        

        var replies = await _repliesRepository.GetRepliesByParentPostId(PostId);

        var repliesDto = _mapper.Map<IEnumerable<ReplyDto>>(replies);

        return Ok(repliesDto);
    }

    [HttpPost(Name = "CreatePostReply")]
    public async Task<ActionResult<CreateReplyDto>> CreatePostReply([FromRoute] Guid PostId, [FromBody] CreateReplyDto createPostReplayDto, [FromRoute] string topicName)
    {
        var userId = _userContextService.GetUserId;

        if (await _postRepository.PostWithGivenIdExist(PostId) is not true)
            return NotFound();

        var newReplie = new Reply { Description = createPostReplayDto.Description, UserId = userId, ParentPostId = PostId };

        await _repliesRepository.Addreplay(newReplie);
        await _repliesRepository.SaveChangesAsync();

        return CreatedAtRoute("GetPostreplies", routeValues: new { PostId, topicName }, createPostReplayDto);
    }


    [AllowAnonymous]
    [HttpGet("{**repliesPath}", Name = "GetRepliesCollection")]
    public async Task<ActionResult<IEnumerable<ReplyDto>>> GetRepliesCollection([FromRoute] string repliesPath)
    {
        var parentGuidstring = LastRegexMatch(repliesPath, _guidRegex);

        Guid lastGuid;

        if (Guid.TryParse(parentGuidstring, out lastGuid) is false)        
            return BadRequest();
        
        //add exist check
        var replies= await _repliesRepository.GetSubRepliesByParentReplayId(lastGuid);

        var repliesDto = _mapper.Map<IEnumerable<ReplyDto>>(replies);
        
        return Ok(repliesDto);
    }
            
    [HttpPost("{**repliesPath}")]
    public async Task<ActionResult<ReplyDto>> Get2Replies([FromRoute] string repliesPath, [FromBody] CreateReplyDto createReplayDto, [FromRoute] Guid PostId, [FromRoute] string topicName)
    {
        var parentGuidstring = LastRegexMatch(repliesPath, _guidRegex);

        Guid subReplayGUID;

        if (Guid.TryParse(parentGuidstring, out subReplayGUID) is false)        
            return BadRequest();        

        var subReplay = await _repliesRepository.GetReplyById(subReplayGUID);

        if(subReplay is null)        
            return NotFound();        
        
        var newSubreply = _mapper.Map<SubReplay>(createReplayDto);

        newSubreply.UserId = _userContextService.GetUserId;
        newSubreply.ParentReplyId = subReplayGUID;

        await _repliesRepository.Addreplay(newSubreply);
        await _repliesRepository.SaveChangesAsync();

        var replyDto = _mapper.Map<ReplyDto>(newSubreply);

        return CreatedAtRoute("GetRepliesCollection",new { topicName, PostId, repliesPath }, replyDto);
    }
}
