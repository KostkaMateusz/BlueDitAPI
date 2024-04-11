using Bluedit.Domain.Entities.LikeEntities;
using MediatR;

namespace Bluedit.Application.Features.LikeFeature.Commands.DeleteLike;

public sealed class DeleteLikeRequest<T> : IRequest where T : LikeBase, new()
{
    public DeleteLikeRequest(Guid parentId, Guid userId)
    {
        UserId = userId;
        ParentId = parentId;
    }

    public Guid UserId { get; }
    public Guid ParentId { get; }
}