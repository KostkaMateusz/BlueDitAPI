using AutoMapper;
using Bluedit.Application.DataModels.LikesDto;
using Bluedit.Domain.Entities.LikeEntities;
using Bluedit.Persistence.Repositories.LikeRepo;
using Bluedit.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Bluedit.Controllers.UserRelated;


public class UserLikesController : ControllerBase
{
    private readonly ILikesRepository<PostLike> _postLikeRepository;
    private readonly ILikesRepository<ReplyLike> _replyLikeRepository;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public UserLikesController(IMapper mapper, ILikesRepository<PostLike> postLikeRepository, ILikesRepository<ReplyLike> replyLikeRepository, IUserContextService userContextService)
    {
        _postLikeRepository = postLikeRepository;
        _replyLikeRepository = replyLikeRepository;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<List<LikesUserInfoDto>> GetLikesByUserId(Guid UserId)
    {
        var userPostLikesList = await _postLikeRepository.GetLikesByUserIdAsync(UserId);
        var userReplyLikesList = await _replyLikeRepository.GetLikesByUserIdAsync(UserId);

        List<LikeBase> userCombinedLikes = [.. userPostLikesList, .. userReplyLikesList];

        return _mapper.Map<List<LikesUserInfoDto>>(userCombinedLikes);
    }

    public async Task<IActionResult> CheckIfUserLikeResource(Guid UserId, Guid resourceiD)
    {
        if (await _postLikeRepository.CheckIfLikeExistAsync(UserId, resourceiD))
            return Ok();

        if (await _replyLikeRepository.CheckIfLikeExistAsync(UserId, resourceiD))
            return Ok();

        return NotFound();
    }


    //public async Task<IActionResult> ToggleLike()
    //{

    //}

    //Put method that works like togle like?
}
