using AutoMapper;
using Bluedit.Application.Contracts;
using Bluedit.Application.DataModels.LikesDto;
using Bluedit.Domain.Entities.LikeEntities;
using MediatR;

namespace Bluedit.Application.Features.LikeFeature.Queries.GetLikesWithUser;

public class GetLikesWithUserQueryHandler<T> : IRequestHandler<GetLikesWithUserQuery<T>,List<LikesDto>> where T : LikeBase, new()
{
    private readonly ILikesRepository<T> _likeRepository;
    private readonly IMapper _mapper;
    
    
    public GetLikesWithUserQueryHandler(IMapper mapper, ILikesRepository<T> likeRepository)
    {
        _likeRepository = likeRepository;
        _mapper = mapper;
    }
    
    
    public async Task<List<LikesDto>> Handle(GetLikesWithUserQuery<T> request, CancellationToken cancellationToken)
    {
        var likes = await _likeRepository.GetLikesByParentIdAsync(request.ParentId);

        var likesDto = _mapper.Map<List<LikesDto>>(likes);

        return likesDto;
    }
}