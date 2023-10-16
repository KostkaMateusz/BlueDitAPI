using Bluedit.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Bluedit.Entities;

namespace Bluedit.Controllers;


[ApiController]
[Route("api/reply")]
public class RepliesController : ControllerBase
{
    private readonly IRepliesRepository _repliesRepository;

    public RepliesController(IRepliesRepository repliesRepository)
    {
        _repliesRepository = repliesRepository;
    }


    [HttpPost("{parentId}")]
    public async Task<IActionResult> createPostReply(Guid parentId) 
    {
        var newReplie = new Reply { Description="sdfsdf", UserId=Guid.Parse("2BFBE79C-F1B9-4D0D-0C59-08DB7AEF0F4F"), ParentPostId = Guid.Parse("3f7700a8-590c-4d82-1a39-08db9202302f") };

        await _repliesRepository.Addreplay(newReplie);
        await _repliesRepository.SaveChangesAsync();

        return Ok();
    }

}
