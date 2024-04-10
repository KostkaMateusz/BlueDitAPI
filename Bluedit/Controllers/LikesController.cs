using AutoMapper;
using Bluedit.Application.Contracts;
using Bluedit.Application.DataModels.LikesDto;
using Bluedit.Application.Features.LikeFeature.Commands.CreateLike;
using Bluedit.Application.Features.LikeFeature.Queries.GetLikesWithUser;
using Bluedit.Domain.Entities.LikeEntities;
using Bluedit.Services.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bluedit.Controllers;

public abstract class LikesController<T> : ControllerBase where T : LikeBase, new()
{
    private readonly ILikesRepository<T> _likeRepository;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;
    private readonly IMediator _mediator;

    protected LikesController(IMapper mapper, ILikesRepository<T> likeRepository, IUserContextService userContextService, IMediator mediator)
    {
        _likeRepository = likeRepository;
        _mapper = mapper;
        _userContextService = userContextService;
        _mediator = mediator;
    }
    
    protected async Task<ActionResult<LikesDto>> GetLikesWithUser(Guid parentId)
    {
        var likesDto =await _mediator.Send(new GetLikesWithUserQuery<T>(parentId));

        return Ok(likesDto);
    }

    protected async Task<ActionResult> CreateLike(Guid parentId)
    {
        var userId = _userContextService.GetUserId;

        var likesDto =await _mediator.Send(new CreateLikeRequest<T>(userId, parentId));
        
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