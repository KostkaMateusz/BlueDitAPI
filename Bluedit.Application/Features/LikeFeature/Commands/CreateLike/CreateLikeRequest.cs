using Bluedit.Application.DataModels.LikesDto;
using Bluedit.Domain.Entities.LikeEntities;
using MediatR;

namespace Bluedit.Application.Features.LikeFeature.Commands.CreateLike;

public sealed class CreateLikeRequest<T> : IRequest<LikesDto> where T : LikeBase, new()
{
    public Guid UserId { get; }
    public Guid ParentId { get; }

    public CreateLikeRequest(Guid userId,Guid parentId)
    {
        UserId = userId;
        ParentId = parentId;
    }
}