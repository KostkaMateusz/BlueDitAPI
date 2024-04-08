using AutoMapper;
using Bluedit.Application.DataModels.LikesDto;
using Bluedit.Domain.Entities.LikeEntities;
using Bluedit.Persistence.Repositories.LikeRepo;
using Bluedit.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bluedit.Controllers.ReplyRelated;

[ApiController]
[Route("api/topics/{topicName}/posts/{postId:guid}/replies/{replyId:guid}/likes")]
public class ReplyLikesController : LikesController<ReplyLike>
{
    public ReplyLikesController(IMapper mapper, ILikesRepository<ReplyLike> likeRepository, IUserContextService userContextService) : base(mapper, likeRepository, userContextService)
    {
    }
    
    [HttpGet]
    public new async Task<ActionResult<LikesDto>> GetLikesWithUser([FromRoute] Guid postId,[FromRoute] Guid replyId)
    {
        return await base.GetLikesWithUser(replyId);
    }
        
    [Authorize]
    [HttpPost]
    public new async Task<ActionResult> CreateLike([FromRoute] Guid postId,[FromRoute] Guid replyId)
    {
        return await base.CreateLike(replyId);
    }
        
    [Authorize]
    [HttpDelete]
    public new async Task<ActionResult> DeleteLike([FromRoute] Guid postId,[FromRoute] Guid replyId)
    {
        return await base.DeleteLike(replyId);
    }
}
