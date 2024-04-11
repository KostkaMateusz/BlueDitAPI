using Bluedit.Application.DataModels.LikesDto;
using Bluedit.Domain.Entities.LikeEntities;
using Bluedit.Services.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bluedit.Controllers.ReplyRelated;

[ApiController]
[Route("api/topics/{topicName}/posts/{postId:guid}/replies/{replyId:guid}/likes")]
public class ReplyLikesController : LikesController<ReplyLike>
{
    public ReplyLikesController(IUserContextService userContextService, IMediator mediator) : base(userContextService,
        mediator)
    {
    }

    [HttpGet]
    public async Task<ActionResult<LikesDto>> GetLikesWithUser([FromRoute] Guid postId, [FromRoute] Guid replyId)
    {
        return await base.GetLikesWithUser(replyId);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateLike([FromRoute] Guid postId, [FromRoute] Guid replyId)
    {
        return await base.CreateLike(replyId);
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult> DeleteLike([FromRoute] Guid postId, [FromRoute] Guid replyId)
    {
        return await base.DeleteLike(replyId);
    }
}