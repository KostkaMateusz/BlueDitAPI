using Bluedit.Application.DataModels.LikesDto;
using Bluedit.Application.Features.LikeFeature.Commands.CreateLike;
using Bluedit.Application.Features.LikeFeature.Commands.DeleteLike;
using Bluedit.Application.Features.LikeFeature.Queries.GetLikesWithUser;
using Bluedit.Domain.Entities.LikeEntities;
using Bluedit.Services.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bluedit.Controllers;

public abstract class LikesController<T> : ControllerBase where T : LikeBase, new()
{
    private readonly IMediator _mediator;
    private readonly IUserContextService _userContextService;

    protected LikesController(IUserContextService userContextService, IMediator mediator)
    {
        _userContextService = userContextService;
        _mediator = mediator;
    }

    protected async Task<ActionResult<LikesDto>> GetLikesWithUser(Guid parentId)
    {
        var likesDto = await _mediator.Send(new GetLikesWithUserQuery<T>(parentId));

        return Ok(likesDto);
    }

    protected async Task<ActionResult> CreateLike(Guid parentId)
    {
        var userId = _userContextService.GetUserId;

        var likesDto = await _mediator.Send(new CreateLikeRequest<T>(userId, parentId));

        return Created("", likesDto);
    }

    protected async Task<ActionResult> DeleteLike(Guid parentId)
    {
        var userId = _userContextService.GetUserId;

        await _mediator.Send(new DeleteLikeRequest<T>(parentId, userId));

        return NoContent();
    }
}