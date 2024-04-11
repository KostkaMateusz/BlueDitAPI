using AutoMapper;
using Bluedit.Application.Contracts;
using Bluedit.Application.DataModels.LikesDto;
using Bluedit.Domain.Entities.LikeEntities;
using Bluedit.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Bluedit.Controllers.UserRelated;

public class UserLikesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILikesRepository<PostLike> _postLikeRepository;
    private readonly ILikesRepository<ReplyLike> _replyLikeRepository;
    private readonly IUserContextService _userContextService;

    public UserLikesController(IMapper mapper, ILikesRepository<PostLike> postLikeRepository,
        ILikesRepository<ReplyLike> replyLikeRepository, IUserContextService userContextService)
    {
        _postLikeRepository = postLikeRepository;
        _replyLikeRepository = replyLikeRepository;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<List<LikesUserInfoDto>> GetLikesByUserId(Guid userId)
    {
        var userPostLikesList = await _postLikeRepository.GetLikesByUserIdAsync(userId);
        var userReplyLikesList = await _replyLikeRepository.GetLikesByUserIdAsync(userId);

        List<LikeBase> userCombinedLikes = [.. userPostLikesList, .. userReplyLikesList];

        return _mapper.Map<List<LikesUserInfoDto>>(userCombinedLikes);
    }

    public async Task<IActionResult> CheckIfUserLikeResource(Guid userId, Guid resourceId)
    {
        if (await _postLikeRepository.CheckIfLikeExistAsync(userId, resourceId))
            return Ok();

        if (await _replyLikeRepository.CheckIfLikeExistAsync(userId, resourceId))
            return Ok();

        return NotFound();
    }


    //public async Task<IActionResult> ToggleLike()
    //{

    //}

    //Put method that works like togle like?
}