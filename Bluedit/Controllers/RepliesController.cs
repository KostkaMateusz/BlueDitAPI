using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bluedit.Services.Repositories;
using Bluedit.Entities;
using Bluedit.Models.DataModels.ReplayDtos;
using Bluedit.Services.Authentication;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Bluedit.Controllers;


[Authorize]
[ApiController]
[Route("api/posts/{PostId}")]
public class RepliesController : ControllerBase
{
    private readonly IRepliesRepository _repliesRepository;
    private readonly IUserContextService _userContextService;
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public RepliesController(IRepliesRepository repliesRepository, IUserContextService userContextService, IPostRepository postRepository, IMapper mapper)
    {
        _repliesRepository = repliesRepository;
        _userContextService = userContextService;
        _postRepository = postRepository;
        _mapper = mapper;
    }

    [HttpGet("replies")]
    [AllowAnonymous]
    public async Task<ActionResult> GetPostreplies(Guid PostId)
    {
        var postExist=await _postRepository.PostWithGivenIdExist(PostId);
        if(postExist is false)
        {
            return NotFound();
        }

        var replies =await _repliesRepository.GetRepliesByParentPostId(PostId);

        var repliesDto = _mapper.Map<IEnumerable<ReplayDto>>(replies);

        return Ok(repliesDto);
    }

    [HttpPost("replies")]
    public async Task<ActionResult> createPostReply([FromRoute] Guid PostId, [FromBody] CreateReplayDto createPostReplayDto) 
    {
        var userId = _userContextService.GetUserId;

        if (await _postRepository.PostWithGivenIdExist(PostId) is not true)
        {
            return NotFound();
        }

        var newReplie = new Reply { Description= createPostReplayDto.Description, UserId= userId, ParentPostId = PostId };

        await _repliesRepository.Addreplay(newReplie);
        await _repliesRepository.SaveChangesAsync();

        return Ok();
    }    

    [AllowAnonymous]
    [HttpGet("reply")]
    public async Task<ActionResult> getReplay([FromQuery][Required] Guid replayID)
    {
        var replay=await _repliesRepository.GetReplayById(replayID);

        if(replay is null)
        {
            return NotFound();
        }

        var replayDto = _mapper.Map<ReplayDto>(replay);

        return Ok(replayDto);
    }


    [HttpPost("reply")]
    public async Task<IActionResult> createReplytoReplay([FromQuery][Required] Guid replayID, [FromBody] CreateReplayDto createReplayDto)
    {
        var replay = await _repliesRepository.GetReplayById(replayID);

        if (replay is null)
        {
            return NotFound();
        }

        var newSubReplay = _mapper.Map<SubReplay>(createReplayDto);

        newSubReplay.UserId = _userContextService.GetUserId;
        newSubReplay.ParentReplyId = replayID;

        await _repliesRepository.Addreplay(newSubReplay);
        await _repliesRepository.SaveChangesAsync();

        return Created("", newSubReplay);
    }
}