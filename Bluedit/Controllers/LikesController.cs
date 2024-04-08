using AutoMapper;
using Bluedit.Application.Contracts;
using Bluedit.Application.DataModels.LikesDto;
using Bluedit.Domain.Entities.LikeEntities;
using Bluedit.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Bluedit.Controllers;

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
    
    protected async Task<ActionResult<LikesDto>> GetLikesWithUser(Guid parentId)
    {
        var likes = await _likeRepository.GetLikesByParentIdAsync(parentId);

        var likesDto = _mapper.Map<List<LikesDto>>(likes);

        return Ok(likesDto);
    }

    protected async Task<ActionResult> CreateLike(Guid parentId)
    {
        var userId = _userContextService.GetUserId;
        
        var likeExist=await _likeRepository.CheckIfLikeExistAsync(userId,parentId);
        if (likeExist)
            return Conflict();
        
        
        var newLike = new T() { UserId =userId, ParentId = parentId };
        await _likeRepository.AddLikeAsync(newLike);
        await _likeRepository.SaveChangesAsync();
        
        var likesDto = _mapper.Map<LikesDto>(newLike);
        
        return Created("",likesDto);
    }

    protected async Task<ActionResult> DeleteLike(Guid parentId)
    {
        var like = await _likeRepository.GetLike(parentId, _userContextService.GetUserId);

        if (like is null)
            return NotFound();

        _likeRepository.DeleteLike(like);
        await _likeRepository.SaveChangesAsync();
        
        return NoContent();
    }
}