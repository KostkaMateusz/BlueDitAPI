using AutoMapper;
using Bluedit.Services.Authentication;
using Bluedit.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bluedit.Controllers;


[ApiController]
[Route("api/posts/{PostId}/{replayID}")]
public class SubRepliesController : ControllerBase
{
    private readonly IRepliesRepository _repliesRepository;
    private readonly IUserContextService _userContextService;
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public SubRepliesController(IRepliesRepository repliesRepository, IUserContextService userContextService, IPostRepository postRepository, IMapper mapper)
    {
        _repliesRepository = repliesRepository;
        _userContextService = userContextService;
        _postRepository = postRepository;
        _mapper = mapper;
    }



}
