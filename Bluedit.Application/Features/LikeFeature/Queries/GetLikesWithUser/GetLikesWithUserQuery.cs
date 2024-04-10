using Bluedit.Application.DataModels.LikesDto;
using MediatR;

namespace Bluedit.Application.Features.LikeFeature.Queries.GetLikesWithUser;

public sealed class GetLikesWithUserQuery<T> : IRequest<List<LikesDto>>
{
    public Guid ParentId { get; }

    public GetLikesWithUserQuery(Guid parentId)
    {
        ParentId = parentId;
    }
}