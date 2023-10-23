using Bluedit.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Bluedit.Entities;
using Bluedit.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Bluedit.Models.DataModels.ReplayDtos;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using AutoMapper;
using Bluedit.Models.DataModels.PostDtos;

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
    [HttpGet("replies/{replayID}")]
    public async Task<ActionResult> getReplay([FromRoute] Guid replayID)
    {
        var replay=await _repliesRepository.GetReplayById(replayID);
        if(replay is null)
        {
            return NotFound();
        }

        var replayDto = _mapper.Map<ReplayDto>(replay);

        return Ok(replayDto);
    }


    [HttpPost("replies/{replayID}/{**repliesString}")]
    [AllowAnonymous]
    public async Task<IActionResult> createReplytoReplay(Guid parentId,string repliesString)
    {


        //var newReplie = new ReplyToReply { Description = "sdfsdf", UserId = Guid.Parse("d7f3db2c-f20d-47e6-9240-08dbd3eca8a2"), ParentReplyId = Guid.Parse("70AADFBA-1259-404E-FD38-08DBD3EE6FC1") };

        //await _repliesRepository.Addreplay(newReplie);
        //await _repliesRepository.SaveChangesAsync();

        return Ok(repliesString);
    }
    
    private void ReplayPathMethodResolver(string replayPath)
    {

    }
   

}
