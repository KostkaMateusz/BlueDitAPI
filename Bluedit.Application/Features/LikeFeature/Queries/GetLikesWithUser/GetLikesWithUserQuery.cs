using Bluedit.Application.DataModels.LikesDto;
using MediatR;

namespace Bluedit.Application.Features.LikeFeature.Queries.GetLikesWithUser;

public class GetLikesWithUserQuery<T> : IRequest<List<LikesDto>>
{
    public Guid ParentId { get; init; }
}