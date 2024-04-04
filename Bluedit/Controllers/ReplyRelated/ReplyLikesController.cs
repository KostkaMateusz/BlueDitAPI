using AutoMapper;
using Bluedit.Application.DataModels.LikesDto;
using Bluedit.Domain.Entities.LikeEntities;
using Bluedit.Persistence.Repositories.LikeRepo;
using Bluedit.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bluedit.Controllers.ReplyRelated;

[ApiController]
[Route("api/topics/{topicName}/posts/{PostId}/reply/{replyId}/likes")]
public class ReplyLikesController : ControllerBase
{
    private readonly ILikesRepository<ReplyLike> _replyLikeRepository;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public ReplyLikesController(IMapper mapper, ILikesRepository<ReplyLike> replyLikeRepository, IUserContextService userContextService)
    {

        _replyLikeRepository = replyLikeRepository;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    [HttpGet]
    public async Task<List<ReplyLikesDto>> GetReplyLikesWithUser(Guid ReplyId)
    {
        var postLikes = await _replyLikeRepository.GetLikesByParentIdAsync(ReplyId);

        var replyLikesDto = _mapper.Map<List<ReplyLikesDto>>(postLikes);

        return replyLikesDto;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateReplyike(Guid replyId)
    {
        var newLike = new ReplyLike() { UserId = _userContextService.GetUserId, ParentId = replyId };
        await _replyLikeRepository.AddLikeAsync(newLike);

        return Created();
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult> DeleteReplyLike(Guid ReplyId)
    {
        var replyLike = await _replyLikeRepository.GetLike(ReplyId, _userContextService.GetUserId);

        if (replyLike is null)
            return NotFound();

        _replyLikeRepository.DeleteLike(replyLike);

        return NoContent();
    }
}
