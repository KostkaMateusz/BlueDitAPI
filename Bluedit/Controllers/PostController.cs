using Bluedit.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bluedit.Controllers;


[ApiController]
[Route("api/{topic}/replays")]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;

    public PostController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [HttpPost]
    public Task<ActionResult> CreatePost()
    {
        throw new NotImplementedException();

    }
}
