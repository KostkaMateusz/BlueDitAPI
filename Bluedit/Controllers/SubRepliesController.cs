using AutoMapper;
using Bluedit.Entities;
using Bluedit.Models.DataModels.ReplayDtos;
using Bluedit.Services.Authentication;
using Bluedit.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Bluedit.Controllers;


[ApiController]
[Route("api/posts/{PostId}/replies")]
[Authorize]
public class SubRepliesController : ControllerBase
{
    private readonly IRepliesRepository _repliesRepository;
    private readonly IUserContextService _userContextService;
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;
    private readonly Regex _guidRegex;
    //private readonly Regex _subRepliesRegex;

    public SubRepliesController(IRepliesRepository repliesRepository, IUserContextService userContextService, IPostRepository postRepository, IMapper mapper)
    {
        _repliesRepository = repliesRepository;
        _userContextService = userContextService;
        _postRepository = postRepository;
        _mapper = mapper;
        _guidRegex = new Regex("(?<=\\S*)(?:\\{{0,1}(?:[0-9a-fA-F]){8}-(?:[0-9a-fA-F]){4}-(?:[0-9a-fA-F]){4}-(?:[0-9a-fA-F]){4}-(?:[0-9a-fA-F]){12}\\}{0,1})(?=\\S*)");
        //_subRepliesRegex = new Regex("(?<=)subreplies");
    }

    [AllowAnonymous]
    [HttpGet("{**repliesChain}")]
    public async Task<ActionResult<IEnumerable<ReplayDto>>> getReplies([FromRoute] string repliesChain)
    {
        var parentGuidstring = lastRegexMatch(repliesChain, _guidRegex);

        Guid lastGuid;

        if (Guid.TryParse(parentGuidstring, out lastGuid) is false)
        {
            return BadRequest();
        }
        //add exist check
        var replies= await _repliesRepository.GetSubRepliesByParentReplayId(lastGuid);

        var repliesDto = _mapper.Map<IEnumerable<ReplayDto>>(replies);
        
        return Ok(repliesDto);
    }
            
    [HttpPost("{**repliesChain}")]
    public async Task<ActionResult> get2Replies([FromRoute] string repliesChain, [FromBody] CreateReplayDto createReplayDto)
    {
        var parentGuidstring = lastRegexMatch(repliesChain, _guidRegex);

        Guid subReplayGUID;

        if (Guid.TryParse(parentGuidstring, out subReplayGUID) is false)
        {
            return BadRequest();
        }

        var subReplay = await _repliesRepository.GetSubReplyById(subReplayGUID);

        if(subReplay is null)
        {
            return NotFound();
        }
        
        var newSubreply = _mapper.Map<SubReplay>(createReplayDto);

        newSubreply.UserId = _userContextService.GetUserId;
        newSubreply.ParentReplyId = subReplayGUID;

        await _repliesRepository.Addreplay(newSubreply);
        await _repliesRepository.SaveChangesAsync();

        return Ok(_mapper.Map<ReplayDto>(newSubreply));
    }

    private string? lastRegexMatch(string subRepliesPath, Regex regex)
    {
        MatchCollection guidMatch = regex.Matches(subRepliesPath);

        var lastMatch = guidMatch.Last();
        var lastMatchValue = lastMatch.Value;

        return lastMatchValue;
    }

}
