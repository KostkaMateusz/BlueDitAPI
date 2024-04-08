using AutoMapper;
using Bluedit.Application.DataModels.LikesDto;
using Bluedit.Domain.Entities.LikeEntities;
using Bluedit.Persistence.Repositories.LikeRepo;
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
}