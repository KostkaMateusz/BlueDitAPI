using AutoMapper;
using Bluedit.Application.DataModels.LikesDto;
using Bluedit.Domain.Entities.LikeEntities;
using Bluedit.Persistence.Repositories.LikeRepo;
using Bluedit.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bluedit.Controllers.PostRelated;

[ApiController]
[Route("api/topics/{topicName}/posts/{postId}/likes")]
public class PostLikesController : ControllerBase
{
    private readonly ILikesRepository<PostLike> _postLikeRepository;

    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public PostLikesController(IMapper mapper, ILikesRepository<PostLike> postLikeRepository, IUserContextService userContextService)
    {
        _postLikeRepository = postLikeRepository;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    [HttpGet]
    public async Task<List<PostLikesDto>> GetPostLikesWithUser(Guid PostId)
    {
        var postLikes = await _postLikeRepository.GetLikesByParentIdAsync(PostId);

        var postLikesDto = _mapper.Map<List<PostLikesDto>>(postLikes);

        return postLikesDto;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreatePostLike(Guid postId)
    {
        var newLike = new PostLike() { UserId = _userContextService.GetUserId, ParentId = postId };
        await _postLikeRepository.AddLikeAsync(newLike);

        return Created();
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult> DeletePostLike(Guid PostId)
    {
        var postLike = await _postLikeRepository.GetLike(PostId, _userContextService.GetUserId);

        if (postLike is null)
            return NotFound();

        _postLikeRepository.DeleteLike(postLike);

        return NoContent();
    }
}