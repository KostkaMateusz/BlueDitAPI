using AutoMapper;
using Bluedit.Application.DataModels.LikesDto;
using Bluedit.Domain.Entities.LikeEntities;
using Bluedit.Persistence.Repositories.LikeRepo;
using Bluedit.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bluedit.Controllers;

[ApiController]
public abstract class LikesController<T> : ControllerBase where T : LikeBase, new()
{
    private readonly ILikesRepository<T> _likeRepository;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    protected LikesController(IMapper mapper, ILikesRepository<T> likeRepository, IUserContextService userContextService)
    {
        _likeRepository = likeRepository;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    [HttpGet]
    public async Task<List<PostLikesDto>> GetLikesWithUser(Guid postId)
    {
        var postLikes = await _likeRepository.GetLikesByParentIdAsync(postId);

        var postLikesDto = _mapper.Map<List<PostLikesDto>>(postLikes);

        return postLikesDto;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateLike(Guid postId)
    {
        var userId = _userContextService.GetUserId;
        
        var likeExist=await _likeRepository.CheckIfLikeExistAsync(userId,postId);
        if (likeExist)
            return Conflict();
        
        
        var newLike = new T() { UserId =userId, ParentId = postId };
        await _likeRepository.AddLikeAsync(newLike);
        await _likeRepository.SaveChangesAsync();
        
        return Created();
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult> DeleteLike(Guid postId)
    {
        var postLike = await _likeRepository.GetLike(postId, _userContextService.GetUserId);

        if (postLike is null)
            return NotFound();

        _likeRepository.DeleteLike(postLike);
        await _likeRepository.SaveChangesAsync();
        
        return NoContent();
    }
}