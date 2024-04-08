using AutoMapper;
using Bluedit.Application.Contracts;
using Bluedit.Application.DataModels.LikesDto;
using Bluedit.Domain.Entities.LikeEntities;
using Bluedit.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bluedit.Controllers.PostRelated;

[ApiController]
[Route("api/topics/{topicName}/posts/{postId:guid}/likes")]
public class PostLikesController : LikesController<PostLike>
{
    public PostLikesController(IMapper mapper, ILikesRepository<PostLike> likeRepository, IUserContextService userContextService) : base(mapper, likeRepository, userContextService)
    {
    }

    [HttpGet]
    public new async Task<ActionResult<LikesDto>> GetLikesWithUser([FromRoute] Guid postId)
    {
        return await base.GetLikesWithUser(postId);
    }
    
    [Authorize]
    [HttpPost]
    public new async Task<ActionResult> CreateLike([FromRoute] Guid postId)
    {
        return await base.CreateLike(postId);
    }
    
    [Authorize]
    [HttpDelete]
    public new async Task<ActionResult> DeleteLike([FromRoute] Guid postId)
    {
        return await base.DeleteLike(postId);
    }
    
    
}