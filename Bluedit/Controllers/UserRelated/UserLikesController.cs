using AutoMapper;
using Bluedit.Application.Contracts;
using Bluedit.Application.DataModels.LikesDto;
using Bluedit.Domain.Entities.LikeEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bluedit.Controllers.UserRelated;

[Authorize]
[Route("api/users/{userId:guid}")]
[ApiController]
public class UserLikesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILikesRepository<PostLike> _postLikeRepository;
    private readonly ILikesRepository<ReplyLike> _replyLikeRepository;

    public UserLikesController(IMapper mapper, ILikesRepository<PostLike> postLikeRepository,
        ILikesRepository<ReplyLike> replyLikeRepository)
    {
        _postLikeRepository = postLikeRepository;
        _replyLikeRepository = replyLikeRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<List<LikesUserInfoDto>> GetLikesByUserId([FromRoute] Guid userId)
    {
        var userPostLikesList = await _postLikeRepository.GetLikesByUserIdAsync(userId);
        var userReplyLikesList = await _replyLikeRepository.GetLikesByUserIdAsync(userId);

        List<LikeBase> userCombinedLikes = [.. userPostLikesList, .. userReplyLikesList];

        return _mapper.Map<List<LikesUserInfoDto>>(userCombinedLikes);
    }
    
    [HttpGet("{resourceId:guid}")]
    public async Task<IActionResult> CheckIfUserLikeResource([FromRoute]Guid userId,[FromRoute] Guid resourceId)
    {
        if (await _postLikeRepository.CheckIfLikeExistAsync(userId, resourceId))
            return Ok();

        if (await _replyLikeRepository.CheckIfLikeExistAsync(userId, resourceId))
            return Ok();

        return NotFound();
    }
}