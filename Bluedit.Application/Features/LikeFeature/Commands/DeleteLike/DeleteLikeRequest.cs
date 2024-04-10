using Bluedit.Application.DataModels.LikesDto;
using Bluedit.Domain.Entities.LikeEntities;
using MediatR;

namespace Bluedit.Application.Features.LikeFeature.Commands.DeleteLike;

public sealed class DeleteLikeRequest<T> : IRequest where T : LikeBase, new()
{
    public Guid UserId { get; }
    public Guid ParentId { get; }

    public DeleteLikeRequest(Guid parentId,Guid userId)
    {
        UserId = userId;
        ParentId = parentId;
    }
}